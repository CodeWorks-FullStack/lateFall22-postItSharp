namespace postItSharp.Repositories;

public class AlbumsRepository
{
  private readonly IDbConnection _db;

  public AlbumsRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Album Create(Album albumData)
  {
    string sql = @"
    INSERT INTO albums
    (title, category, coverImg, creatorId)
    VALUES
    (@title, @category, @coverImg, @creatorId);
    SELECT LAST_INSERT_ID();
    ";
    int id = _db.ExecuteScalar<int>(sql, albumData);
    albumData.Id = id;
    return albumData;
  }

  internal List<Album> Get()
  {
    string sql = @"
    SELECT
    al.*,
    ac.*
    FROM albums al
    JOIN accounts ac ON ac.id = al.creatorId;
    ";
    //                      <1st select, 2nd select, return type> |(data from rows)
    List<Album> albums = _db.Query<Album, Account, Album>(sql, (album, account) =>
    {
      // attach account data from the matching row to album
      album.Creator = account;
      // return joined data
      return album;
    }).ToList();

    return albums;
  }

  internal Album GetOne(int id)
  {
    string sql = @"
     SELECT
    al.*,
    ac.*
    FROM albums al
    JOIN accounts ac ON ac.id = al.creatorId
    WHERE al.id = @id;
    ";
    return _db.Query<Album, Account, Album>(sql, (album, account) =>
    {
      album.Creator = account;
      return album;
    }, new { id }).FirstOrDefault();

  }


  internal bool Update(Album update)
  {
    string sql = @"
    UPDATE albums
    SET
    title = @title,
    coverImg = @coverImg,
    archived = @archived
    WHERE id = @id;
    ";
    int rows = _db.Execute(sql, update);
    return rows > 0;
  }
  internal void Remove(int id)
  {
    string sql = @"
    DELETE FROM albums
    WHERE id = @id;
    ";
    _db.Execute(sql, new { id });
  }
}
