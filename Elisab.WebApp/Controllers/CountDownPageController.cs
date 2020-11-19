using Elisab.BAL.Repository;
using System.Web.Mvc;

namespace Elisab.Controllers
{
  public class CountDownPageController : BaseController
  {
    FashionShowsRepository _fashionShowsRepository;

    public ActionResult Setting()
    {
      _fashionShowsRepository = new FashionShowsRepository();
      ViewData["fashionShow"] = _fashionShowsRepository.GetAll();
      return View();
    }
  }
}
