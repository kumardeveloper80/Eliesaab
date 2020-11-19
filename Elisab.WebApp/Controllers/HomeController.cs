using System.Web.Mvc;
using Elisab.BAL.Repository;

namespace Elisab.WebApp.Controllers
{
  public class HomeController : Controller
  {
    FashionShowsRepository _fashionShowsRepository;
    public ActionResult Login()
    {
      Session.Abandon();
      return View();
    }

    public ActionResult Index()
    {
      var UserId = Session["AdminId"];
      if (UserId != null)
      {
        _fashionShowsRepository = new FashionShowsRepository();
        ViewData["counter"] = _fashionShowsRepository.GetAll().Count;
        return View();
      }
      return RedirectToAction("Login", "Home");
    }

    public ActionResult LogOut()
    {
      Session.Abandon();
      return RedirectToAction("Login", "Home");
    }

  }
}
