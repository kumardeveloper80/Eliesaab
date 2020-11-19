using System;
using System.ComponentModel.DataAnnotations;

namespace Elisab.DAL.Context
{
  public class FashionShow
  {
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShowDate { get; set; }
    public string ShowTime { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public int? DeletedBy { get; set; }
  }
}
