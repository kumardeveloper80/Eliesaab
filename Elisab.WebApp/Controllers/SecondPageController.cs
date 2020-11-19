using Elisab.BAL.Repository;
using System.Web.Mvc;

namespace Elisab.Controllers
{
  public class SecondPageController : BaseController
  {
    FashionShowsRepository _fashionShowsRepository;

    public ActionResult Index()
    {
      _fashionShowsRepository = new FashionShowsRepository();
      ViewData["fashionShow"] = _fashionShowsRepository.GetAll();
      return View();
    }
  }
}
