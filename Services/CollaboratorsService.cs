namespace postItSharp.Services;

public class CollaboratorsService
{
  private readonly CollaboratorRepository _repo;
  private readonly AlbumsService _albumsService;

  public CollaboratorsService(CollaboratorRepository repo, AlbumsService albumsService)
  {
    _repo = repo;
    _albumsService = albumsService;
  }

  internal int Create(AlbumMember albumMemberData)
  {
    Album album = _albumsService.GetOne(albumMemberData.AlbumId, albumMemberData.AccountId);

    int id = _repo.Create(albumMemberData);

    return id;
  }

  internal List<Collaborator> GetCollaborators(int albumId, string userId)
  {
    Album album = _albumsService.GetOne(albumId, userId);

    List<Collaborator> collabs = _repo.GetCollaborators(albumId);
    return collabs;
  }

  internal List<MyAlbum> GetMyAlbums(string accountId)
  {
    List<MyAlbum> myAlbums = _repo.GetMyAlbums(accountId);
    return myAlbums;
  }
}
