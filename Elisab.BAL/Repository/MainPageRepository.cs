using AutoMapper;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elisab.BAL.Repository
{
  public class MainPageRepository
  {
    ElisabDbContext _context;

    public MainPageRepository()
    {
      _context = new ElisabDbContext();
    }

    public MainPage_VM GetByFashionShowId(int id)
    {
      var result = _context.MainPages.Where(x => x.FashionShowId == id).FirstOrDefault();
      if (result != null)
      {
        return Mapper.Map<MainPage_VM>(result);
      }
      return null;
    }
  }
}
