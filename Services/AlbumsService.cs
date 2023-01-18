namespace postItSharp.Services;

public class AlbumsService
{
  private readonly AlbumsRepository _repo;

  public AlbumsService(AlbumsRepository repo)
  {
    _repo = repo;
  }

  internal Album Create(Album albumData)
  {
    Album album = _repo.Create(albumData);
    // if you wanted to get it from the sql table again, your created could return the id of the last insert and you could run a get one using that id.
    return album;
  }

  internal List<Album> Get(string userId)
  {
    List<Album> albums = _repo.Get();
    // NOTE -----------------------get all albums that are public OR the requestor is the creator
    List<Album> filtered = albums.FindAll(a => a.Archived == false || a.CreatorId == userId);
    // SOMETHING here for archived albums
    return filtered;
  }

  internal Album GetOne(int id, string userId)
  {
    Album album = _repo.GetOne(id);
    if (album == null)
    {
      throw new Exception("No Album at that id");
    }
    if (album.Archived == true && album.CreatorId != userId)
    {
      throw new Exception("you don't own that");
    }
    // TODO access control
    return album;
  }

  internal string Remove(int id, string userId)
  {
    Album original = GetOne(id, userId);
    if (original.CreatorId != userId)
    {
      throw new Exception("Nacho album.");
    }

    // NOTE soft delete
    // original.Archived = !original.Archived;
    // _repo.Update(original);
    // return $"{original.Title} has been {(original.Archived ? "archived" : "unearthed")}";

    // NOTE regular delete
    _repo.Remove(id);
    return $"{original.Title} has been exterminated";
  }
}
