using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elisab.DAL.Context
{
  public class Section
  {
    [Key]
    public int Id { get; set; }
    public int FashionShowId { get; set; }
    public string Description { get; set; }
    public string MediaType { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public int? DeletedBy { get; set; }
    public int? Sequence { get; set; }
  }
}
