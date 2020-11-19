using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elisab.BAL.Repository
{
 public class GalleryLoginRepository
  {
    ElisabDbContext _context;

    public GalleryLoginRepository()
    {
      _context = new ElisabDbContext();
    }
    public int Authenticate(GalleryLogin_VM login)
    {
      var result = _context.GalleryLogins.Where(x => x.Username == login.Username).FirstOrDefault();
      if (result != null && result.Password == login.Password)
      {
        return result.Id;
      }
      return 0;
    }
  }
}
