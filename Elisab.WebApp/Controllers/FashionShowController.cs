using Elisab.BAL.Repository;
using System.Web.Mvc;

namespace Elisab.Controllers
{
  public class FashionShowController : BaseController
  {
    FashionShowsRepository _fashionShowsRepository;

    public ActionResult List()
    {
      return View();
    }

    public ActionResult Save(int Id = 0)
    {
      _fashionShowsRepository = new FashionShowsRepository();
      return View(_fashionShowsRepository.Get(Id));
    }
  }
}
