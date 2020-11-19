using System.ComponentModel.DataAnnotations;

namespace Elisab.DAL.Context
{
  public class GalleryLogin
  {
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }
}
