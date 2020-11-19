using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elisab.DAL.Context
{
  public class SectionMedia
  {
    [Key]
    public int Id { get; set; }
    public int SectionId { get; set; }
    public string MediaName { get; set; }
    public string PosterImageName { get; set; }
  }
}
