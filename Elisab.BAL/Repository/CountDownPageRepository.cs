using AutoMapper;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using System.Linq;

namespace Elisab.BAL.Repository
{
  public class CountDownPageRepository
  {
    ElisabDbContext _context;

    public CountDownPageRepository()
    {
      _context = new ElisabDbContext();
    }

    public CountDownPage_VM GetByFashionShowId(int id)
    {
      CountDownPage_VM countDownPage_VM = new CountDownPage_VM();
      var result = _context.CountDownPages.Where(x => x.FashionShowId == id).FirstOrDefault();
      if (result != null)
      {
        countDownPage_VM = Mapper.Map<CountDownPage_VM>(result);
      }
      countDownPage_VM.ShowDate = _context.FashionShows.Where(x => x.Id == id).Select(x => x.ShowDate + " " + x.ShowTime).FirstOrDefault();
      return countDownPage_VM;
    }

    public CountDownPage_VM GetPreview(int id)
    {
      var countDownPage = new CountDownPage_VM();
      if (id > 0)
      {
        countDownPage = (from fs in _context.FashionShows
                         join cd in _context.CountDownPages on fs.Id equals cd.FashionShowId
                         where fs.DeletedDate == null && fs.Id == id
                         select new CountDownPage_VM()
                         {
                           ShowDate = fs.ShowDate + " " + fs.ShowTime,
                           HeaderLogo = cd.HeaderLogo,
                           MainContent = cd.MainContent,
                           MainBgImg = cd.MainBgImg,
                           MainInnerImg = cd.MainInnerImg,
                           FooterBgImg = cd.FooterBgImg
                         }).FirstOrDefault();
      }
      else
      {
        countDownPage = (from fs in _context.FashionShows
                         join cd in _context.CountDownPages on fs.Id equals cd.FashionShowId
                         where fs.DeletedDate == null && fs.IsActive == true
                         select new CountDownPage_VM()
                         {
                           ShowDate = fs.ShowDate + " " + fs.ShowTime,
                           HeaderLogo = cd.HeaderLogo,
                           MainContent = cd.MainContent,
                           MainBgImg = cd.MainBgImg,
                           MainInnerImg = cd.MainInnerImg,
                           FooterBgImg = cd.FooterBgImg
                         }).FirstOrDefault();
      }

      return countDownPage;
    }
  }
}
