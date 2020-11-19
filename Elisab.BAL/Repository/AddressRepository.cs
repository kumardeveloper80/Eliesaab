using AutoMapper;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using System.Linq;

namespace Elisab.BAL.Repository
{
  public class AddressRepository
  {
    ElisabDbContext _context;

    public AddressRepository()
    {
      _context = new ElisabDbContext();
    }

    public Address_VM GetByFashionShowId(int id)
    {
      var result = _context.Address.Where(x => x.FashionShowId == id).FirstOrDefault();
      if (result != null)
      {
        return Mapper.Map<Address_VM>(result);
      }
      return null ;
    }
  }
}
