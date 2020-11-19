using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elisab.DAL.Context
{
  public class Address
  {
    [Key]
        public int Id { get; set; }
        public int FashionShowId { get; set; }
        public string HeaderText { get; set; }
        public string ImageName { get; set; }
        public string Link { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
