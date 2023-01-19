namespace postItSharp.Models;

public class Picture
{
  public int Id { get; set; }
  public string CreatorId { get; set; }
  public string ImgUrl { get; set; }
  public int AlbumId { get; set; }
  public Account Creator { get; set; }
}
