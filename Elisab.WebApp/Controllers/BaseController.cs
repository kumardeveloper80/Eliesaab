using System.Web.Mvc;

namespace Elisab.Controllers
{
  public class BaseController : Controller
  {
    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      var UserId = Session["AdminId"];
      if (UserId == null)
      {
        HttpContext.Response.Redirect("/Home/Login");
      }
    }
  }
}
