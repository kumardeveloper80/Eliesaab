using Elisab.BAL.Repository;
using Elisab.BAL.ViewModel;
using Elisab.WebApp.Helper;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace AdminLTE1.Controllers
{

  public class GalleryImageController : Controller
  {
    ImageGalleryRepository imageGalleryRepository = null;
   
    public GalleryImageController()
    {
      imageGalleryRepository = new ImageGalleryRepository();
     
    }
   
    public ActionResult Index(string id)
    {

      var ids = HttpUtility.UrlDecode(id);
      List<ImageGallery_VM> data = new List<ImageGallery_VM>();

      try
      {
        var fashionId = Convert.ToInt32(Helper.DecodeServerName(ids));
        data = imageGalleryRepository.GetByFashionShowId(fashionId); ;
      }
      catch (Exception ex)
      {

      }
      return View(data);
    }
  }
}
