namespace postItSharp.Repositories;

public class CollaboratorRepository
{
  private readonly IDbConnection _db;

  public CollaboratorRepository(IDbConnection db)
  {
    _db = db;
  }

  internal int Create(AlbumMember albumMemberData)
  {
    string sql = @"
    INSERT INTO albumMembers
    (albumId, accountId)
    VALUES
    (@albumId, @accountId);
    SELECT LAST_INSERT_Id();
    ";
    int id = _db.ExecuteScalar<int>(sql, albumMemberData);
    return id;
  }

  // internal List<Collaborator> GetCollaborators(int albumId)
  // {
  //   string sql = @"
  //   SELECT
  //   ac.*,
  //   am.*
  //   FROM albumMembers am
  //   JOIN accounts ac ON ac.id = am.accountId
  //   WHERE am.albumId = @albumId;
  //   ";
  //   return _db.Query<Collaborator, AlbumMember, Collaborator>(sql, (collab, albumMember) =>
  //   {
  //     collab.AlbumMemberId = albumMember.Id;
  //     return collab;
  //   }, new { albumId }).ToList();
  // }
  // NOTE the as method only works when you are adding simple data, NOT combining objects
  internal List<Collaborator> GetCollaborators(int albumId)
  {
    string sql = @"
    SELECT
    ac.*,
    am.id AS albumMemberId
    FROM albumMembers am
    JOIN accounts ac ON ac.id = am.accountId
    WHERE am.albumId = @albumId;
    ";
    return _db.Query<Collaborator>(sql, new { albumId }).ToList();
  }

  internal List<MyAlbum> GetMyAlbums(string accountId)
  {
    string sql = @"
    SELECT
    ab.*,
    am.*,
    cr.*
    FROM albumMembers am
    JOIN albums ab ON ab.id = am.albumId
    JOIN accounts cr ON ab.creatorId = cr.id
    WHERE am.accountId = @accountId; 
    ";
    List<MyAlbum> myAlbums = _db.Query<MyAlbum, AlbumMember, Account, MyAlbum>(sql, (ab, am, cr) =>
    {
      ab.AlbumMemberId = am.Id;
      ab.Creator = cr;
      return ab;
    }, new { accountId }).ToList();

    return myAlbums;
  }
}
