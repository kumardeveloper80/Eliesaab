using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elisab.DAL.Context
{

  public class SecondPage
  {
        [Key]
        public int Id { get; set; }
        public int FashionShowId { get; set; }
        public string HtmlContent1 { get; set; }
        public string Image1 { get; set; }
        public string HtmlContent2 { get; set; }
        public string Image2 { get; set; }
        public string HtmlContent3 { get; set; }
        public string Image3 { get; set; }
        public string HtmlContent4 { get; set; }
        public string Image4 { get; set; }
        public string HtmlContent5 { get; set; }
        public string Image5 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsShowAtLast { get; set; }
    }
}
