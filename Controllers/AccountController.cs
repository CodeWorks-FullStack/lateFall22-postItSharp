namespace postItSharp.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
  private readonly AccountService _accountService;
  private readonly Auth0Provider _auth0Provider;
  private readonly CollaboratorsService _collabService;

  public AccountController(AccountService accountService, Auth0Provider auth0Provider, CollaboratorsService collabService)
  {
    _accountService = accountService;
    _auth0Provider = auth0Provider;
    _collabService = collabService;
  }

  [HttpGet]
  [Authorize]
  public async Task<ActionResult<Account>> Get()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      return Ok(_accountService.GetOrCreateProfile(userInfo));
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("albums")]
  [Authorize]
  public async Task<ActionResult<List<MyAlbum>>> GetAlbums()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      List<MyAlbum> myAlbums = _collabService.GetMyAlbums(userInfo.Id);
      return myAlbums;
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}
