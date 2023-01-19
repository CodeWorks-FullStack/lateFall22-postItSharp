namespace postItSharp.Services;

public class PicturesService
{
  private readonly PicturesRepository _repo;

  private readonly AlbumsService _albumService;

  public PicturesService(PicturesRepository repo, AlbumsService albumService)
  {
    _repo = repo;
    _albumService = albumService;
  }

  internal Picture Create(Picture pictureData)
  {
    Picture picture = _repo.Create(pictureData);
    return picture;
  }

  internal List<Picture> GetPicturesByAlbum(int albumId, string userId)
  {
    // NOTE we get the album because that method has error handling and access control on it already
    Album album = _albumService.GetOne(albumId, userId);

    List<Picture> pictures = _repo.GetPicturesByAlbum(albumId);
    return pictures;
  }

  internal string Remove(int id, string userId)
  {
    Picture original = _repo.GetOne(id);
    if (original == null)
    {
      throw new Exception("not Picture to Delete at that id");
    }
    if (original.CreatorId != userId)
    {
      throw new Exception("Nacho Picture");
    }
    _repo.Remove(id);
    return $"Picture at {id} was removed";
  }
}
