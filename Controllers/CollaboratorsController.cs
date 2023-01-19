namespace postItSharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollaboratorsController : ControllerBase
{
  private readonly CollaboratorsService _collabService;
  private readonly Auth0Provider _auth0provider;

  public CollaboratorsController(CollaboratorsService collabService, Auth0Provider auth0provider)
  {
    _collabService = collabService;
    _auth0provider = auth0provider;
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Collaborator>> Create([FromBody] AlbumMember albumMemberData)
  {
    try
    {
      // NOTE calling the userInfo a collaborator instead of an account to re-use the userInfo data to return to client
      Collaborator userInfo = await _auth0provider.GetUserInfoAsync<Collaborator>(HttpContext);
      albumMemberData.AccountId = userInfo.Id;
      int id = _collabService.Create(albumMemberData);
      userInfo.AlbumMemberId = id;
      return Ok(userInfo);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }


  }

}
