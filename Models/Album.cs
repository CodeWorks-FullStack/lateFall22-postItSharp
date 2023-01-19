
namespace postItSharp.Models;
public class Album
{
  //   [Required] 
  //  [Range(0, 10)]
  public int Id { get; set; }
  [Required]
  public string Title { get; set; }
  public string CoverImg { get; set; } = "https://images.unsplash.com/photo-1575485670541-824ff288aaf8?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1074&q=80";
  public string Category { get; set; }
  public bool Archived { get; set; }

  public string CreatorId { get; set; }
  //   TODO we will add more here in a second

  public Account Creator { get; set; }
}

public class MyAlbum : Album
{
  public int AlbumMemberId { get; set; }
}