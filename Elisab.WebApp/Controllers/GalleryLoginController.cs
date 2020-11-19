using Elisab.BAL.Repository;
using Elisab.BAL.ViewModel;
using Elisab.WebApp.Helper;
using System;
using System.Web;
using System.Web.Mvc;

namespace AdminLTE1.Controllers
{
  public class GalleryLoginController : Controller
  {
    GalleryLoginRepository galleryLoginRepository = null;
    public GalleryLoginController()
    {
      galleryLoginRepository = new GalleryLoginRepository();
    }

    public ActionResult Index(string id = null)
    {

      return View();
    }

    

    [HttpPost]
    public ActionResult Authenticateuser(FormCollection form)
    {
      var login = new GalleryLogin_VM();
      login.Username = Convert.ToString(form["txtUserName"]);
      login.Password = Convert.ToString(form["txtPassword"]);
      var fascode = form["fascode"];
      fascode = Convert.ToString(HttpUtility.UrlDecode(fascode));
      if (login != null && login.Password != null)
      {
        login.Password = Helper.Encrypt(login.Password);
        var result = galleryLoginRepository.Authenticate(login);
        if (result != 0)
        {
          return RedirectToAction("Index", "GalleryImage", new { id = fascode });
        }
        else
        {
         // Session["Loginfailed"] = true;
          return RedirectToAction("Index", "GalleryLogin", new { id = fascode });
        }
      }
      return RedirectToAction("Index");
    }
  }
}
