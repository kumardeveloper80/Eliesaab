using Elisab.BAL.Repository;
using Elisab.WebApp.Helper;
using System.Web.Mvc;

namespace Elisab.Controllers
{
  public class SectionController : BaseController
  {
    FashionShowsRepository _fashionShowsRepository;

    public ActionResult List()
    {
      _fashionShowsRepository = new FashionShowsRepository();
      ViewData["fashionShow"] = _fashionShowsRepository.GetAll();
      ViewData["mediadType"] = Helper.type();
      return View();
    }
  }
}
