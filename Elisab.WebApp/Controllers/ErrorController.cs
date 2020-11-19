using System.Web.Mvc;

namespace Elisab.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        //[HttpGet]
        public ActionResult InternalServerError()
        {
            return View();
        }

        //[HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}
