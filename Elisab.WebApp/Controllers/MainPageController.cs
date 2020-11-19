using Elisab.BAL.Repository;
using Elisab.Controllers;
using System.Web.Mvc;

namespace AdminLTE1.Controllers
{
  public class MainPageController : BaseController
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
