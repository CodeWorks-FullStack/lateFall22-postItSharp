namespace postItSharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumsController : ControllerBase
{
  private readonly AlbumsService _albumsService;
  private readonly Auth0Provider _auth0provider;
  private readonly PicturesService _picturesService;
  private readonly CollaboratorsService _collabService;

  public AlbumsController(AlbumsService albumsService, Auth0Provider auth0provider, PicturesService picturesService, CollaboratorsService collabService)
  {
    _albumsService = albumsService;
    _auth0provider = auth0provider;
    _picturesService = picturesService;
    _collabService = collabService;
  }

  [HttpGet]
  public async Task<ActionResult<List<Album>>> Get()
  {
    try
    {
      Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
      // NOTE don't forget to add the ? if you are accessing userInfo but do not required a user to be logged in
      List<Album> albums = _albumsService.Get(userInfo?.Id);
      return Ok(albums);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Album>> GetOne(int id)
  {
    try
    {
      Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
      Album album = _albumsService.GetOne(id, userInfo?.Id);
      return Ok(album);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{id}/pictures")]
  public async Task<ActionResult<List<Picture>>> GetPictures(int id)
  {
    try
    {
      Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
      List<Picture> pictures = _picturesService.GetPicturesByAlbum(id, userInfo?.Id);
      return Ok(pictures);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{id}/collaborators")]
  public async Task<ActionResult<List<Collaborator>>> GetCollaborators(int id)
  {
    try
    {
      Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
      List<Collaborator> collabs = _collabService.GetCollaborators(id, userInfo?.Id);
      return Ok(collabs);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
      throw;
    }
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Album>> Create([FromBody] Album albumData)
  {
    try
    {
      Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
      albumData.CreatorId = userInfo.Id;
      Album album = _albumsService.Create(albumData);
      album.Creator = userInfo;
      return Ok(album);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<ActionResult<string>> Remove(int id)
  {
    try
    {
      Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
      string message = _albumsService.Remove(id, userInfo.Id);
      return Ok(message);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}
