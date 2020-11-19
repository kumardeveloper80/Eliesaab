using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elisab.DAL.Context
{
  public class CountDownPage
  {
    [Key]
    public int Id { get; set; }
    public int FashionShowId { get; set; }
    public string HeaderLogo { get; set; }
    public string MainContent { get; set; }
    public string MainBgImg { get; set; }
    public string MainInnerImg { get; set; }
    public string FooterBgImg { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public int? DeletedBy { get; set; }
  }
}
