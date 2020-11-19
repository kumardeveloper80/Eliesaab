using AutoMapper;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using System;
using System.Linq;

namespace Elisab.BAL.Repository
{
  public class SecondPageRepository
  {
    ElisabDbContext _context;

    public SecondPageRepository()
    {
      _context = new ElisabDbContext();
    }

    public SecondPage_VM GetByFashionShowId(int id)
    {
      var result = _context.SecondPages.Where(x => x.FashionShowId == id).FirstOrDefault();
      if (result != null)
      {
        return Mapper.Map<SecondPage_VM>(result);
      }
      return null;
    }

    public int Add(SecondPage_VM secondPage, int CreatedBy)
    {
      var second = Mapper.Map<SecondPage>(secondPage);
      second.CreatedBy = CreatedBy;
      second.CreatedDate = DateTime.Now;
      _context.SecondPages.Add(second);
      return _context.SaveChanges();
    }

    public int Update(int id, SecondPage_VM secondPage, int UpdatedBy)
    {
      var result = _context.SecondPages.Where(x => x.Id == id).FirstOrDefault();
      if (result != null)
      {
        result.HtmlContent1 = secondPage.HtmlContent1;
        result.HtmlContent2 = secondPage.HtmlContent2;
        result.HtmlContent3 = secondPage.HtmlContent3;
        result.HtmlContent4 = secondPage.HtmlContent4;
        result.HtmlContent5 = secondPage.HtmlContent5;
        result.Image1 = secondPage.Image1;
        result.Image2 = secondPage.Image2;
        result.Image3 = secondPage.Image3;
        result.Image4 = secondPage.Image4;
        result.Image5 = secondPage.Image5;
                result.IsActive = secondPage.IsActive;
                result.IsShowAtLast = secondPage.IsShowAtLast;
                result.UpdatedBy = UpdatedBy;
        result.UpdatedDate = DateTime.Now;
        return _context.SaveChanges();
      }
      return 0;
    }
  }
}
