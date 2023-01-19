namespace postItSharp.Repositories;

public class PicturesRepository
{
  private readonly IDbConnection _db;

  public PicturesRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Picture Create(Picture pictureData)
  {
    string sql = @"
    INSERT INTO pictures
    (imgUrl, albumId, creatorId)
    VALUES
    (@imgUrl, @albumId, @creatorId);
    SELECT LAST_INSERT_ID();
    ";
    int id = _db.ExecuteScalar<int>(sql, pictureData);
    pictureData.Id = id;
    return pictureData;
  }

  internal Picture GetOne(int id)
  {
    string sql = @"
    SELECT
    *
    FROM pictures
    WHERE id = @id;
    ";
    return _db.Query<Picture>(sql, new { id }).FirstOrDefault();
  }

  internal List<Picture> GetPicturesByAlbum(int albumId)
  {
    string sql = @"
    SELECT
    p.*,
    a.*
    FROM pictures p
    JOIN accounts a ON p.creatorId = a.id
    WHERE albumId = @albumId;
    ";
    List<Picture> pictures = _db.Query<Picture, Account, Picture>(sql, (picture, account) =>
    {
      picture.Creator = account;
      return picture;
    }, new { albumId }).ToList();

    return pictures;
  }

  internal void Remove(int id)
  {
    string sql = @"
    DELETE FROM pictures
    WHERE id = @id;
    ";
    _db.Execute(sql, new { id });
  }
}
