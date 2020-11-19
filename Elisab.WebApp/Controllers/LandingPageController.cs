using Elisab.BAL.Repository;
using System.Web.Mvc;

namespace Elisab.Controllers
{
  public class LandingPageController : Controller
  {
    FashionShowsRepository _fashionShowsRepository;
    CountDownPageRepository _countDownPageRepository;
    public LandingPageController()
    {
      _fashionShowsRepository = new FashionShowsRepository();
      _countDownPageRepository = new CountDownPageRepository();
    }

    public ActionResult Index(int id = 0)
    {

      var rec = _fashionShowsRepository.GetLandingPage(id);
      if (rec.IsFutureEvent == true)
      {
        return RedirectToAction("CountDown");
      }
      else
      {
        return View(rec);
      }
    }

    public ActionResult CountDown(int id = 0)
    {
      return View(_countDownPageRepository.GetPreview(id));
    }
  }
}
