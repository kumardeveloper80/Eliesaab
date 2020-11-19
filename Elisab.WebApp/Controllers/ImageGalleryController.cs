using Elisab.BAL.Repository;
using System.Web.Mvc;

namespace AdminLTE1.Controllers
{
  public class ImageGalleryController : Controller
  {
    FashionShowsRepository _fashionShowsRepository = new FashionShowsRepository();
   
    public ActionResult List()
    {
      _fashionShowsRepository = new FashionShowsRepository();
      ViewData["fashionShow"] = _fashionShowsRepository.GetAll();
      return View();
    }
  }
}
