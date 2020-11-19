using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using System.Linq;

namespace Elisab.BAL.Repository
{
    public class AdminRespository
    {
        ElisabDbContext _context;

        public AdminRespository()
        {
            _context = new ElisabDbContext();
        }

        public int Authentication(Login_VM login)
        {
            var result = _context.Admins.Where(x => x.Username == login.Username).FirstOrDefault();
            if (result != null && result.Password == login.Password)
            {
                return result.Id;
            }
            return 0;
        }

        public Login_VM GetByEmail(Login_VM login)
        {
            var result = _context.Admins.Where(x => x.Email == login.Email).FirstOrDefault();
            if (result != null)
            {
                login.Username = result.Username;
                login.Password = result.Password;
                return login;
            }
            return null; ;
        }
    }
}
