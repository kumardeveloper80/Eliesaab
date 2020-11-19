using Elisab.BAL.Repository;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using Elisab.WebApp.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http;

namespace Elisab.Controllers
{
  [RoutePrefix("api/AdminApi")]
  public class AdminAPIController : ApiController
  {
    #region :: GLOBAL VARIABLES

    int adminId = 0;
    AdminRespository _adminRespository;
    FashionShowsRepository _fashionShowsRepository;
    AddressRepository _addressRepository;
    SecondPageRepository _secondPageRepository;
    MainPageRepository _mainPageRepository;
    SectionRepository _sectionRepository;
    ImageGalleryRepository _imageGalleryRepository;
    CountDownPageRepository _countDownPageRepository;

    ElisabDbContext _DBContext;

    #endregion

    #region :: CONSTRUCTOR

    public AdminAPIController()
    {
      if (HttpContext.Current.Session["AdminId"] != null)
      {
        adminId = Convert.ToInt32(HttpContext.Current.Session["AdminId"]);
        _DBContext = new ElisabDbContext();
      }
    }

    #endregion

    #region :: ACTION

    /// <summary>
    /// API for Admin authentication
    /// </summary>
    /// <param name="login_VM"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("Authentication")]
    public IHttpActionResult Authentication([FromBody]Login_VM login)
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      try
      {
        if (login != null && login.Password != null)
        {
          _adminRespository = new AdminRespository();
          login.Password = Helper.Encrypt(login.Password);

          var result = _adminRespository.Authentication(login);
          if (result > 0)
          {
            commonResponse.status = Helper.success_code;
            HttpContext.Current.Session["AdminId"] = result;
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.invalidLogin;
          }
        }
        else
        {
          commonResponse.status = Helper.failure_code;
          commonResponse.message = Helper.invalidLogin;
        }
      }
      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for ForgotPassword
    /// </summary>
    /// <param name="login_VM"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("ForgotPassword")]
    public IHttpActionResult ForgotPassword([FromBody]Login_VM login)
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      if (login.Email != null)
      {
        _adminRespository = new AdminRespository();
        string EmailBody = string.Empty;

        var result = _adminRespository.GetByEmail(login);
        if (result != null)
        {
          result.Password = Helper.Decrypt(result.Password);
          EmailBody = Helper.emailBody;
          EmailBody += "Username:" + result.Username + "<br/>";
          EmailBody += "Password:" + result.Password + "<br/><br/><br/>";
          if (SendForgotPasswordEmail(result.Email, EmailBody) == 1)
          {
            commonResponse.status = Helper.success_code;
            commonResponse.message = Helper.emailSent_success;
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.emailSent_error;
          }
        }
        else
        {
          commonResponse.status = Helper.failure_code;
          commonResponse.message = Helper.emailSent_error;
        }
      }
      else
      {
        commonResponse.status = Helper.failure_code;
        commonResponse.message = Helper.invalidEmail;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Add fashion Show
    /// </summary>
    /// <param name="FashionShows_VM"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddFashionShow")]
    public IHttpActionResult AddFashionShow(FashionShows_VM fashionShows)
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();

      if (adminId > 0)
      {
        try
        {
          if (fashionShows != null)
          {
            fashionShows.UserId = adminId;
            _fashionShowsRepository = new FashionShowsRepository();
            commonResponse.status = _fashionShowsRepository.Add(fashionShows);
            if (commonResponse.status == Helper.success_code)
            {
              commonResponse.message = Helper.FashionShow + Helper.recordAdd_success;
            }
            else
            {
              commonResponse.status = Helper.failure_code;
              commonResponse.message = Helper.FashionShow + Helper.recordAdd_unsuccess;
            }
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.FashionShow + Helper.recordAdd_unsuccess;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Update fashion Shows
    /// </summary>
    /// <param name="FashionShows_VM"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("UpdateFashionShow")]
    public IHttpActionResult UpdateFashionShow(FashionShows_VM fashionShows)
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      if (adminId > 0)
      {
        try
        {
          if (fashionShows != null)
          {
            fashionShows.UserId = adminId;
            _fashionShowsRepository = new FashionShowsRepository();
            commonResponse.status = _fashionShowsRepository.Update(fashionShows);
            if (commonResponse.status == 1)
            {
              commonResponse.message = Helper.FashionShow + Helper.recordUpdate_success;
            }
            else
            {
              commonResponse.status = Helper.failure_code;
              commonResponse.message = Helper.FashionShow + Helper.recordUpdate_unsuccess;
            }
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.FashionShow + Helper.recordUpdate_unsuccess;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Delete fashion Show
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("DeleteFashionShow")]
    public IHttpActionResult DeleteFashionShow(int Id)
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      if (adminId > 0)
      {
        try
        {
          if (Id > 0)
          {
            _fashionShowsRepository = new FashionShowsRepository();
            commonResponse.status = _fashionShowsRepository.Delete(Id, adminId);
            if (commonResponse.status == 1)
            {
              commonResponse.message = Helper.FashionShow + Helper.recordDelete_success;
            }
            else
            {
              commonResponse.status = Helper.failure_code;
              commonResponse.message = Helper.FashionShow + Helper.recordDelete_unsuccess;
            }
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.FashionShow + Helper.recordDelete_unsuccess;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Get fashion Show by Id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetFashionShow")]
    public IHttpActionResult GetFashionShow(int Id)
    {
      CommonResponse<FashionShows_VM> commonResponse = new CommonResponse<FashionShows_VM>();
      if (adminId > 0)
      {
        try
        {
          if (Id > 0)
          {
            _fashionShowsRepository = new FashionShowsRepository();
            var result = _fashionShowsRepository.Get(Id);
            if (result != null)
            {
              commonResponse.status = Helper.success_code;
              commonResponse.dataenum = result;
            }
            else
            {
              commonResponse.status = Helper.failure_code;
              commonResponse.message = Helper.FashionShow + Helper.recordNotFound;
            }
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.FashionShow + Helper.recordNotFound;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Get Add fashion Shows
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllFashionShow")]
    public IHttpActionResult GetAllFashionShow()
    {
      CommonResponse<string> commonResponse = new CommonResponse<string>();
      if (adminId > 0)
      {
        try
        {
          _fashionShowsRepository = new FashionShowsRepository();
          var result = _fashionShowsRepository.GetAll();
          if (result != null && result.Count > 0)
          {
            commonResponse.status = Helper.success_code;
            commonResponse.dataenum = GetRazorViewAsString(result, "~/Views/FashionShow/FashionShow_PartialView.cshtml");
          }
          else
          {
            commonResponse.status = Helper.failure_code;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Change the Fashion show IsActive Status by Id
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="IsActive"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("UpdateFashionShowActive")]
    public IHttpActionResult UpdateFashionShowActive(int Id, bool IsActive)
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      if (adminId > 0)
      {
        try
        {
          if (Id > 0)
          {
            _fashionShowsRepository = new FashionShowsRepository();
            commonResponse.status = _fashionShowsRepository.UpdateIsActiveById(Id, IsActive, adminId);
            if (commonResponse.status == 1)
            {
              commonResponse.message = Helper.FashionShow + Helper.recordUpdate_success;
            }
            else
            {
              commonResponse.status = Helper.failure_code;
              commonResponse.message = Helper.FashionShow + Helper.recordUpdate_unsuccess;
            }
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.FashionShow + Helper.recordUpdate_unsuccess;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// Function for Save/Update Address
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("SaveAddress")]
    public IHttpActionResult SaveAddress()
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      try
      {
        if (HttpContext.Current.Session["AdminId"] != null)
        {
          Address _address = new Address();
          _address.Id = Convert.ToInt32(HttpContext.Current.Request.Form["addressId"]);
          _address.HeaderText = HttpContext.Current.Request.Form["headerText"];
          _address.FashionShowId = Convert.ToInt32(HttpContext.Current.Request.Form["fashionShowId"]);
          _address.IsActive = Convert.ToBoolean(HttpContext.Current.Request.Form["isActive"]);

          if (_address != null)
          {
            #region :: File Upload

            if (HttpContext.Current.Request.Files.Count > 0)
            {
              _address.ImageName = fileToUpolad(HttpContext.Current.Request.Files[0]);
            }

            #endregion

            #region :: Address Add/Update

            if (_address.Id > 0) //Update Address
            {
              _address.UpdatedDate = DateTime.Now;
              _address.UpdatedBy = adminId;
              var oldAddress = _DBContext.Address.Find(_address.Id);
              if (oldAddress != null)
              {
                if (HttpContext.Current.Request.Files.Count == 0)
                {
                  _address.ImageName = oldAddress.ImageName;
                }
                else
                {
                  fileToDelete(oldAddress.ImageName);
                }
                _address.CreatedDate = oldAddress.CreatedDate;
                _address.CreatedBy = oldAddress.CreatedBy;
              }
              _DBContext.Address.AddOrUpdate(_address);
              commonResponse.status = _DBContext.SaveChanges();
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.AddressSection + Helper.recordUpdate_success;
              }
              else
              {
                commonResponse.status = Helper.failure_code;
                commonResponse.message = Helper.AddressSection + Helper.recordUpdate_unsuccess;
              }
            }
            else // Add Address
            {
              _address.CreatedDate = DateTime.Now;
              _address.CreatedBy = adminId;
              _address = _DBContext.Address.Add(_address);
              commonResponse.status = _DBContext.SaveChanges();
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.AddressSection + Helper.recordAdd_success;
              }
              else
              {
                commonResponse.status = Helper.failure_code;
                commonResponse.message = Helper.AddressSection + Helper.recordAdd_unsuccess;
              }
            }

            #endregion
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.AddressSection + Helper.recordAdd_unsuccess;
          }
        }
        else
        {
          commonResponse.status = Helper.sessionout_code;
          commonResponse.message = Helper.session_out;
        }
      }
      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Get Address by Fashion show Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAddressByFashionShowId")]
    public IHttpActionResult GetAddressByFashionShowId(int id)
    {
      CommonResponse<Address_VM> commonResponse = new CommonResponse<Address_VM>();
      if (adminId > 0)
      {
        try
        {
          _addressRepository = new AddressRepository();
          var result = _addressRepository.GetByFashionShowId(id);
          if (result != null)
          {
            commonResponse.status = Helper.success_code;
            commonResponse.dataenum = result;
          }
          else
          {
            commonResponse.status = Helper.failure_code;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Get Second Page by Fashion show Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetSecondPageByFashionShowId")]
    public IHttpActionResult GetSecondPageByFashionShowId(int id)
    {
      CommonResponse<SecondPage_VM> commonResponse = new CommonResponse<SecondPage_VM>();
      if (adminId > 0)
      {
        try
        {
          _secondPageRepository = new SecondPageRepository();
          var result = _secondPageRepository.GetByFashionShowId(id);
          if (result != null)
          {
            commonResponse.status = Helper.success_code;
            commonResponse.dataenum = result;
          }
          else
          {
            commonResponse.status = Helper.failure_code;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Get MainPage by Fashion show Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetMainPageByFashionShowId")]
    public IHttpActionResult GetMainPageByFashionShowId(int id)
    {
      CommonResponse<MainPage_VM> commonResponse = new CommonResponse<MainPage_VM>();
      if (adminId > 0)
      {
        try
        {
          _mainPageRepository = new MainPageRepository();
          var result = _mainPageRepository.GetByFashionShowId(id);
          if (result != null)
          {
            commonResponse.status = Helper.success_code;
            commonResponse.dataenum = result;
          }
          else
          {
            commonResponse.status = Helper.failure_code;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Get ImageGallery by Fashion show Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetImageGalleryByFashionShowId")]
    public IHttpActionResult GetImageGalleryByFashionShowId(int id)
    {
      CommonResponse<List<ImageGallery_VM>> commonResponse = new CommonResponse<List<ImageGallery_VM>>();

      try
      {
        _imageGalleryRepository = new ImageGalleryRepository();
        List<ImageGallery_VM> result = new List<ImageGallery_VM>();
        result = _imageGalleryRepository.GetByFashionShowId(id);
        if (result != null)
        {
          commonResponse.status = Helper.success_code;
          commonResponse.dataenum = result;
        }
        else
        {
          commonResponse.status = Helper.failure_code;
        }
        commonResponse.dataenum = result.ToList();
      }
      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// Function for Save/Update ImageGallery
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("SaveImageGallery")]
    public IHttpActionResult SaveImageGallery()
    {
      CommonResponse<ImageGallery> commonResponse = new CommonResponse<ImageGallery>();
      try
      {
        if (HttpContext.Current.Session["AdminId"] != null)
        {
          string fPath = string.Empty;
          string directoryPath = string.Empty;
          ImageGallery _imageGallery = new ImageGallery();
          _imageGallery.Id = Convert.ToInt32(HttpContext.Current.Request.Form["imageId"]);
          _imageGallery.FashionShowId = Convert.ToInt32(HttpContext.Current.Request.Form["fashionShowId"]);

          if (_imageGallery != null)
          {
            #region :: File Upload

            if (HttpContext.Current.Request.Files.Count > 0) //File Upload
            {
              HttpPostedFile file = HttpContext.Current.Request.Files[0];
              string Folder = Convert.ToString(ConfigurationManager.AppSettings["GalleryImg"]);
              directoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
              fPath = Helper.GenerateRandomString(8) + Path.GetExtension(file.FileName);
              _imageGallery.Image = fPath;
              fPath = Path.Combine(directoryPath, fPath);
              if (!Directory.Exists(directoryPath))
              {
                Directory.CreateDirectory(directoryPath);
              }
              file.SaveAs(fPath);

              //save thumbnail
              Image image = Image.FromFile(fPath);
              Image thumb = image.GetThumbnailImage(240,360, () => false, IntPtr.Zero);
              string ThumbPath = Helper.GenerateRandomString(8) + "_thumb" + Path.GetExtension(file.FileName);
              _imageGallery.ThumbImage = ThumbPath;

              string ThumbfPath = Path.Combine(directoryPath, ThumbPath);
              thumb.Save(ThumbfPath);

            }
            #endregion

            #region :: Image Add/Update

            if (_imageGallery.Id > 0) //Update Image
            {
              _imageGallery.UpdatedDate = DateTime.Now;
              _imageGallery.UpdatedBy = adminId;

              var oldImageGallery = _DBContext.ImageGalleries.Find(_imageGallery.Id);
              if (oldImageGallery != null)
              {
                if (HttpContext.Current.Request.Files.Count == 0)
                {
                  _imageGallery.Image = oldImageGallery.Image;
                  _imageGallery.ThumbImage = oldImageGallery.ThumbImage;
                }
                else
                {
                  if (Directory.Exists(directoryPath) && File.Exists(Path.Combine(directoryPath, oldImageGallery.Image, oldImageGallery.ThumbImage)))
                  {
                    File.Delete(Path.Combine(directoryPath, oldImageGallery.Image, oldImageGallery.ThumbImage));
                  }
                }
                _imageGallery.CreatedDate = oldImageGallery.CreatedDate;
                _imageGallery.CreatedBy = oldImageGallery.CreatedBy;
              }
              _DBContext.ImageGalleries.AddOrUpdate(_imageGallery);
              commonResponse.status = _DBContext.SaveChanges();
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.ImageGallery + Helper.recordUpdate_success;
              }
              else
              {
                commonResponse.status = Helper.failure_code;
                commonResponse.message = Helper.ImageGallery + Helper.recordUpdate_unsuccess;
              }
            }
            else // Add Image
            {
              _imageGallery.CreatedDate = DateTime.Now;
              _imageGallery.CreatedBy = adminId;
              _imageGallery = _DBContext.ImageGalleries.Add(_imageGallery);
              commonResponse.status = _DBContext.SaveChanges();
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.ImageGallery + Helper.recordAdd_success;
              }
              else
              {
                commonResponse.status = Helper.failure_code;
                commonResponse.message = Helper.ImageGallery + Helper.recordAdd_unsuccess;
              }
            }
            commonResponse.dataenum = _imageGallery;

            #endregion
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.ImageGallery + Helper.recordAdd_unsuccess;
          }
        }
        else
        {
          commonResponse.status = Helper.sessionout_code;
          commonResponse.message = Helper.session_out;
        }
      }
      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Delete image
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("DeleteImage")]
    public IHttpActionResult DeleteImage(int id)
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      try
      {
        if (id > 0)
        {
          _imageGalleryRepository = new ImageGalleryRepository();
          commonResponse.status = _imageGalleryRepository.Delete(id);
          if (commonResponse.status == 1)
          {
            commonResponse.message = Helper.ImageGallery + Helper.recordDelete_success;
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.ImageGallery + Helper.recordDelete_unsuccess;
          }
        }
        else
        {
          commonResponse.status = Helper.failure_code;
          commonResponse.message = Helper.ImageGallery + Helper.recordDelete_unsuccess;
        }
      }
      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }


    /// <summary>
    /// Function for Save/Update Main Page
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("SaveMainPage")]
    public IHttpActionResult SaveMainPage()
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      try
      {
        if (HttpContext.Current.Session["AdminId"] != null)
        {
          MainPage _mainPage = new MainPage();
          _mainPage.Id = Convert.ToInt32(HttpContext.Current.Request.Form["mainPageId"]);
          _mainPage.FashionShowId = Convert.ToInt32(HttpContext.Current.Request.Form["fashionShowId"]);
          _mainPage.ContentText = HttpContext.Current.Request.Form["contentText"];
                    _mainPage.IsActive = Convert.ToBoolean(HttpContext.Current.Request.Form["isActive"]);

          if (_mainPage != null)
          {
            #region :: File Upload

            if (HttpContext.Current.Request.Files.Count > 0) //File Upload
            {

              if (HttpContext.Current.Request.Files["backImg"] != null)
              {
                _mainPage.BackgroundImage = fileToUpolad(HttpContext.Current.Request.Files["backImg"]);
              }

              if (HttpContext.Current.Request.Files["innerImg"] != null)
              {
                _mainPage.InnerImage = fileToUpolad(HttpContext.Current.Request.Files["innerImg"]);
              }

              if (HttpContext.Current.Request.Files["logoImg"] != null)
              {
                _mainPage.LogoImage = fileToUpolad(HttpContext.Current.Request.Files["logoImg"]);
              }

            }
            #endregion

            #region :: MainPage Add/Update

            if (_mainPage.Id > 0) //Update Main Page
            {
              _mainPage.UpdatedDate = DateTime.Now;
              _mainPage.UpdatedBy = adminId;

              var oldMainPage = _DBContext.MainPages.Find(_mainPage.Id);

              if (oldMainPage != null)
              {

                if (_mainPage.BackgroundImage == null)
                {
                  _mainPage.BackgroundImage = oldMainPage.BackgroundImage;
                }
                else
                {
                  fileToDelete(oldMainPage.BackgroundImage);
                }

                if (_mainPage.InnerImage == null)
                {
                  _mainPage.InnerImage = oldMainPage.InnerImage;
                }
                else
                {
                  fileToDelete(oldMainPage.InnerImage);
                }

                if (_mainPage.LogoImage == null)
                {
                  _mainPage.LogoImage = oldMainPage.LogoImage;
                }
                else
                {
                  fileToDelete(oldMainPage.LogoImage);
                }

                _mainPage.CreatedDate = oldMainPage.CreatedDate;
                _mainPage.CreatedBy = oldMainPage.CreatedBy;
              }

              _DBContext.MainPages.AddOrUpdate(_mainPage);
              commonResponse.status = _DBContext.SaveChanges();
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.MainPage + Helper.recordUpdate_success;
              }
              else
              {
                commonResponse.message = Helper.MainPage + Helper.recordUpdate_unsuccess;
                commonResponse.status = Helper.failure_code;
              }
            }
            else // Add MainPage
            {
              _mainPage.CreatedDate = DateTime.Now;
              _mainPage.CreatedBy = adminId;
              _mainPage = _DBContext.MainPages.Add(_mainPage);
              commonResponse.status = _DBContext.SaveChanges();
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.MainPage + Helper.recordAdd_success;
              }
              else
              {
                commonResponse.message = Helper.MainPage + Helper.recordAdd_unsuccess;
                commonResponse.status = Helper.failure_code;
              }
            }
            #endregion
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.MainPage + Helper.recordAdd_unsuccess;
          }
        }
        else
        {
          commonResponse.status = Helper.sessionout_code;
          commonResponse.message = Helper.session_out;
        }
      }

      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// Function for Save/Update Second Page
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("SaveSecondPage")]
    public IHttpActionResult SaveSecondPage()
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      try
      {
        if (HttpContext.Current.Session["AdminId"] != null)
        {
          string fPath = string.Empty;
          string directoryPath = string.Empty;
          _secondPageRepository = new SecondPageRepository();

          SecondPage_VM secondPage = new SecondPage_VM();
          secondPage.Id = Convert.ToInt32(HttpContext.Current.Request.Form["secondPageId"]);
          secondPage.FashionShowId = Convert.ToInt32(HttpContext.Current.Request.Form["fashionShowId"]);
          secondPage.HtmlContent1 = Convert.ToString(HttpContext.Current.Request.Form["HtmlContent1"]);
          secondPage.HtmlContent2 = Convert.ToString(HttpContext.Current.Request.Form["HtmlContent2"]);
          secondPage.HtmlContent3 = Convert.ToString(HttpContext.Current.Request.Form["HtmlContent3"]);
          secondPage.HtmlContent4 = Convert.ToString(HttpContext.Current.Request.Form["HtmlContent4"]);
          secondPage.HtmlContent5 = Convert.ToString(HttpContext.Current.Request.Form["HtmlContent5"]);
                    secondPage.IsActive = Convert.ToBoolean(HttpContext.Current.Request.Form["isActive"]);
                    secondPage.IsShowAtLast = Convert.ToBoolean(HttpContext.Current.Request.Form["isShowAtLast"]);

                    if (secondPage != null)
          {
            #region :: File Upload

            if (HttpContext.Current.Request.Files.Count > 0)
            {
              if (secondPage.HtmlContent1 != null)
              {
                if (HttpContext.Current.Request.Files["div_0"] != null)
                {
                  secondPage.Image1 = fileToUpolad(HttpContext.Current.Request.Files["div_0"]);
                }
              }

              if (secondPage.HtmlContent2 != null)
              {
                if (HttpContext.Current.Request.Files["div_1"] != null)
                {
                  secondPage.Image2 = fileToUpolad(HttpContext.Current.Request.Files["div_1"]);
                }
              }

              if (secondPage.HtmlContent3 != null)
              {
                if (HttpContext.Current.Request.Files["div_2"] != null)
                {
                  secondPage.Image3 = fileToUpolad(HttpContext.Current.Request.Files["div_2"]);
                }
              }

              if (secondPage.HtmlContent4 != null)
              {
                if (HttpContext.Current.Request.Files["div_3"] != null)
                {
                  secondPage.Image4 = fileToUpolad(HttpContext.Current.Request.Files["div_3"]);
                }
              }

              if (secondPage.HtmlContent5 != null)
              {
                if (HttpContext.Current.Request.Files["div_4"] != null)
                {
                  secondPage.Image5 = fileToUpolad(HttpContext.Current.Request.Files["div_4"]);
                }
              }
            }

            #endregion

            #region :: Second Page Add/Update

            if (secondPage.Id > 0)
            {
              var oldSecondPage = _DBContext.SecondPages.Where(x => x.FashionShowId == secondPage.FashionShowId).FirstOrDefault();

              if (oldSecondPage != null)
              {
                if (secondPage.Image1 == null && secondPage.HtmlContent1 != null)
                {
                  secondPage.Image1 = oldSecondPage.Image1;
                }
                else
                {
                  fileToDelete(oldSecondPage.Image1);
                }

                if (secondPage.Image2 == null && secondPage.HtmlContent2 != null)
                {
                  secondPage.Image2 = oldSecondPage.Image2;
                }
                else
                {
                  fileToDelete(oldSecondPage.Image2);
                }

                if (secondPage.Image3 == null && secondPage.HtmlContent3 != null)
                {
                  secondPage.Image3 = oldSecondPage.Image3;
                }
                else
                {
                  fileToDelete(oldSecondPage.Image3);
                }

                if (secondPage.Image4 == null && secondPage.HtmlContent4 != null)
                {
                  secondPage.Image4 = oldSecondPage.Image4;
                }
                else
                {
                  fileToDelete(oldSecondPage.Image4);
                }

                if (secondPage.Image5 == null && secondPage.HtmlContent5 != null)
                {
                  secondPage.Image5 = oldSecondPage.Image5;
                }
                else
                {
                  fileToDelete(oldSecondPage.Image5);
                }
              }

              commonResponse.status = _secondPageRepository.Update(secondPage.Id, secondPage, adminId);
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.SecondPage + Helper.recordUpdate_success;
              }
              else
              {
                commonResponse.status = Helper.failure_code;
                commonResponse.message = Helper.SecondPage + Helper.recordUpdate_unsuccess;
              }
            }
            else // Add Second Page
            {
              commonResponse.status = _secondPageRepository.Add(secondPage, adminId);
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.SecondPage + Helper.recordAdd_success;
              }
              else
              {
                commonResponse.status = Helper.failure_code;
                commonResponse.message = Helper.SecondPage + Helper.recordAdd_unsuccess;
              }
            }

            #endregion
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.SecondPage + Helper.recordAdd_unsuccess;
          }
        }
        else
        {
          commonResponse.status = Helper.sessionout_code;
          commonResponse.message = Helper.session_out;
        }
      }
      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// Function for Save/Update Section
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("SaveSection")]
    public IHttpActionResult SaveSection()
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      try
      {
        if (HttpContext.Current.Session["AdminId"] != null)
        {
          _sectionRepository = new SectionRepository();
          List<string> multiImageName = new List<string>();
          List<string> oldmultiImageName = new List<string>();

          Section_VM section_VM = new Section_VM();
          SectionMedia_VM sectionMedia_VM = new SectionMedia_VM();
          section_VM.Id = Convert.ToInt32(HttpContext.Current.Request.Form["sectionId"]);
          section_VM.Description = Convert.ToString(HttpContext.Current.Request.Form["description"]);
          section_VM.FashionShowId = Convert.ToInt32(HttpContext.Current.Request.Form["fashionShowId"]);
          section_VM.MediaType = Convert.ToString(HttpContext.Current.Request.Form["mediaType"]);
          sectionMedia_VM.Id = Convert.ToInt32(HttpContext.Current.Request.Form["sectionMediaId"]);

          bool isPosterImgChange = HttpContext.Current.Request.Form["isPosterImgChange"] != null ? Convert.ToBoolean(HttpContext.Current.Request.Form["isPosterImgChange"]) : false;
          bool isVideoChange = HttpContext.Current.Request.Form["isVideoChange"] != null ? Convert.ToBoolean(HttpContext.Current.Request.Form["isVideoChange"]) : false;
          bool isSingleImgChange = HttpContext.Current.Request.Form["isSingleImgChange"] != null ? Convert.ToBoolean(HttpContext.Current.Request.Form["isSingleImgChange"]) : false;

          if (section_VM != null)
          {
            #region :: File Upload
            if (HttpContext.Current.Request.Files.Count > 0)
            {
              for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
              {
                if (HttpContext.Current.Request.Files.AllKeys[i] == "posterImg")
                {
                  sectionMedia_VM.PosterImageName = fileToUpolad(HttpContext.Current.Request.Files[i]);
                }
                else if (HttpContext.Current.Request.Files.AllKeys[i] == "video")
                {
                  sectionMedia_VM.MediaName = fileToUpolad(HttpContext.Current.Request.Files[i]);
                }
                else if (HttpContext.Current.Request.Files.AllKeys[i] == "singleImage")
                {
                  sectionMedia_VM.MediaName = fileToUpolad(HttpContext.Current.Request.Files[i]);
                }
                else if (HttpContext.Current.Request.Files.AllKeys[i] == "multiImg")
                {
                  multiImageName.Add(fileToUpolad(HttpContext.Current.Request.Files[i]));
                }
              }
            }
            #endregion

            #region :: Section Add/Update

            if (section_VM.Id > 0 && sectionMedia_VM.Id > 0)
            {
              int ret = 0;
              var oldSection = _DBContext.Sections.Find(section_VM.Id);
              var oldsectionMedia = _DBContext.SectionMedias.Where(x => x.SectionId == section_VM.Id).ToList();
              sectionMedia_VM.SectionId = section_VM.Id;

              #region :: Old File Delete

              if (oldSection != null && oldSection.MediaType != section_VM.MediaType)
              {
                if (oldsectionMedia.Any())
                {
                  foreach (var file in oldsectionMedia)
                  {
                    fileToDelete(file.MediaName);
                    if (file.PosterImageName != null && file.PosterImageName != "")
                    {
                      fileToDelete(file.PosterImageName);
                    }
                  }
                }
              }

              #endregion

              if (section_VM.MediaType == Helper.VideoType)
              {
                ret = _sectionRepository.UpdateSection(section_VM.Id, adminId, section_VM);

                if (isPosterImgChange)
                {
                  fileToDelete(Convert.ToString(oldsectionMedia[0].PosterImageName));
                }
                else
                {
                  sectionMedia_VM.PosterImageName = Convert.ToString(oldsectionMedia[0].PosterImageName);
                }

                if (isVideoChange)
                {
                  fileToDelete(Convert.ToString(oldsectionMedia[0].MediaName));
                }
                else
                {
                  sectionMedia_VM.MediaName = Convert.ToString(oldsectionMedia[0].MediaName);
                }

                if (oldSection != null && oldSection.MediaType == section_VM.MediaType)
                {
                  ret = _sectionRepository.UpdateSectionMedia(sectionMedia_VM.Id, sectionMedia_VM);
                }
                else
                {
                  _DBContext.SectionMedias.RemoveRange(oldsectionMedia);
                  _DBContext.SaveChanges();

                  ret = _sectionRepository.AddSectionMedia(sectionMedia_VM);
                }
              }
              else if (section_VM.MediaType == Helper.TextType || section_VM.MediaType == Helper.ImageTxtType)
              {
                ret = _sectionRepository.UpdateSection(section_VM.Id, adminId, section_VM);
                if (oldSection != null && (oldSection.MediaType == section_VM.MediaType))
                {
                  if (isSingleImgChange)
                  {
                    fileToDelete(Convert.ToString(oldsectionMedia[0].MediaName));
                  }
                  else
                  {
                    sectionMedia_VM.MediaName = Convert.ToString(oldsectionMedia[0].MediaName);
                  }
                  ret = _sectionRepository.UpdateSectionMedia(sectionMedia_VM.Id, sectionMedia_VM);
                }
                else
                {
                  _DBContext.SectionMedias.RemoveRange(oldsectionMedia);
                  _DBContext.SaveChanges();
                 ret = _sectionRepository.AddSectionMedia(sectionMedia_VM);
                }
              }
              else if (section_VM.MediaType == Helper.ImageType)
              {
                ret = _sectionRepository.UpdateSection(section_VM.Id, adminId, section_VM);

                oldmultiImageName = HttpContext.Current.Request.Form["oldImages"].Split(',').ToList();
                List<string> saveOldFileList = new List<string>();
                if (oldsectionMedia.Any())
                {
                  var deleteOldFileList = oldsectionMedia.Where(x => !oldmultiImageName.Any(y => y == x.MediaName)).ToList();
                  saveOldFileList = oldsectionMedia.Where(x => oldmultiImageName.Any(y => y == x.MediaName)).Select(x => x.MediaName).ToList();

                  foreach (var delete in deleteOldFileList)
                  {
                    fileToDelete(delete.MediaName);
                  }
                  _DBContext.SectionMedias.RemoveRange(oldsectionMedia);
                  _DBContext.SaveChanges();
                }

                multiImageName = multiImageName.Concat(saveOldFileList).ToList();
                List<SectionMedia> _sectionMediaList = new List<SectionMedia>();
                foreach (var fileName in multiImageName)
                {
                  var obj = new SectionMedia();
                  obj.SectionId = section_VM.Id;
                  obj.MediaName = fileName;
                  _sectionMediaList.Add(obj);
                }
                _DBContext.SectionMedias.AddRange(_sectionMediaList);
                ret = _DBContext.SaveChanges();
                
              }

              if (ret > 0)
              {
                commonResponse.status = Helper.success_code;
                commonResponse.message = Helper.Section + Helper.recordUpdate_success;
              }
              else
              {
                commonResponse.status = Helper.failure_code;
                commonResponse.message = Helper.Section + Helper.recordUpdate_unsuccess;
              }
            }
            else
            {
              section_VM.Sequence = 1;
              var oldSection = _sectionRepository.GetByFashionShowId(section_VM.FashionShowId);
              if (oldSection.Any())
              {
                section_VM.Sequence = oldSection.Last().Sequence + 1;
              }

              int result = _sectionRepository.AddSection(adminId, section_VM);
              if (result > 0)
              {
                sectionMedia_VM.SectionId = result;
                if (section_VM.MediaType != Helper.ImageType)
                {
                  result = _sectionRepository.AddSectionMedia(sectionMedia_VM);
                }
                else
                {
                  List<SectionMedia> _sectionMediaList = new List<SectionMedia>();
                  foreach (var fileName in multiImageName)
                  {
                    var obj = new SectionMedia();
                    obj.SectionId = sectionMedia_VM.SectionId;
                    obj.MediaName = fileName;
                    obj.PosterImageName = sectionMedia_VM.PosterImageName;
                    _sectionMediaList.Add(obj);
                    _DBContext.SectionMedias.AddRange(_sectionMediaList);
                    result = _DBContext.SaveChanges();
                  }
                }
              }

              if (result > 0)
              {
                commonResponse.status = Helper.success_code;
                commonResponse.message = Helper.Section + Helper.recordAdd_success;
              }
              else
              {
                commonResponse.status = Helper.failure_code;
                commonResponse.message = Helper.Section + Helper.recordAdd_unsuccess;
              }
            }

            #endregion
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.Section + Helper.recordAdd_unsuccess;
          }
        }
        else
        {
          commonResponse.status = Helper.sessionout_code;
          commonResponse.message = Helper.session_out;
        }
      }
      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// Function for get Section list by fashion show Id
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    [Route("GetSectionList")]
    public IHttpActionResult GetSectionList(int Id)
    {
      CommonResponse<string> commonResponse = new CommonResponse<string>();
      try
      {
        if (HttpContext.Current.Session["AdminId"] != null)
        {
          SectionMedia _sectionMedia = new SectionMedia();
          if (Id > 0)
          {
            _sectionRepository = new SectionRepository();
            var result = _sectionRepository.GetByFashionShowId(Id);
            commonResponse.dataenum = GetRazorViewAsString(result, "~/Views/Section/SectionList_PartialView.cshtml");
            commonResponse.status = Helper.success_code;
          }
          else
          {
            commonResponse.status = Helper.failure_code;
          }
        }
        else
        {
          commonResponse.status = Helper.sessionout_code;
          commonResponse.message = Helper.session_out;
        }
      }
      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Delete Section
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("DeleteSection")]
    public IHttpActionResult DeleteSection(int Id)
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      if (adminId > 0)
      {
        try
        {
          if (Id > 0)
          {
            _sectionRepository = new SectionRepository();
            commonResponse.status = _sectionRepository.Delete(Id, adminId);
            if (commonResponse.status == Helper.success_code)
            {
              commonResponse.message = Helper.Section + Helper.recordDelete_success;
            }
            else
            {
              commonResponse.status = Helper.failure_code;
              commonResponse.message = Helper.Section + Helper.recordDelete_unsuccess;
            }
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.Section + Helper.recordDelete_unsuccess;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Get Section by Id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetSectionById")]
    public IHttpActionResult GetSectionById(int Id)
    {
      CommonResponse<Section_VM> commonResponse = new CommonResponse<Section_VM>();
      if (adminId > 0)
      {
        try
        {
          if (Id > 0)
          {
            _sectionRepository = new SectionRepository();
            var result = _sectionRepository.GetById(Id);
            if (result != null)
            {
              commonResponse.status = Helper.success_code;
              commonResponse.dataenum = result;
            }
            else
            {
              commonResponse.status = Helper.failure_code;
              commonResponse.message = Helper.Section + Helper.recordNotFound;
            }
          }
          else
          {
            commonResponse.status = Helper.failure_code;
          }
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Update the Section Sequence
    /// </summary>
    /// <param name="FashionShows_VM"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("UpdateSectionSequence")]
    public IHttpActionResult UpdateSectionSequence([FromBody] SequenceList list)
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      if (list != null && list.sequences.Count > 0)
      {
        try
        {
          _sectionRepository = new SectionRepository();
          _sectionRepository.UpdateSequnce(list.sequences);
          commonResponse.status = Helper.success_code;
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// API for Get Count Down Page by Fashion show Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetCountDownPage")]
    public IHttpActionResult GetCountDownPage(int id)
    {
      CommonResponse<CountDownPage_VM> commonResponse = new CommonResponse<CountDownPage_VM>();
      if (adminId > 0)
      {
        try
        {
          _countDownPageRepository = new CountDownPageRepository();
          var result = _countDownPageRepository.GetByFashionShowId(id);
          if(result.Id > 0)
          {
            commonResponse.status = Helper.success_code;
          }
          else
          {
            commonResponse.status = Helper.failure_code;
          }
          commonResponse.dataenum = result;
        }
        catch (Exception ex)
        {
          commonResponse.status = Helper.error_code;
          commonResponse.message = ex.Message;
        }
      }
      else
      {
        commonResponse.status = Helper.sessionout_code;
        commonResponse.message = Helper.session_out;
      }
      return Ok(commonResponse);
    }

    /// <summary>
    /// Function for Save/Update Count Down Page
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("SaveCountDownPage")]
    public IHttpActionResult SaveCountDownPage()
    {
      CommonResponse<int> commonResponse = new CommonResponse<int>();
      try
      {
        if (HttpContext.Current.Session["AdminId"] != null)
        {
          CountDownPage _countDownPage = new CountDownPage();
          _countDownPage.Id = Convert.ToInt32(HttpContext.Current.Request.Form["countDownPageId"]);
          _countDownPage.FashionShowId = Convert.ToInt32(HttpContext.Current.Request.Form["fashionShowId"]);
          _countDownPage.MainContent = Convert.ToString(HttpContext.Current.Request.Form["mainContent"]);

          if (_countDownPage != null)
          {
            #region :: File Upload

            if (HttpContext.Current.Request.Files.Count > 0) //File Upload
            {
              if (HttpContext.Current.Request.Files["headerLogo"] != null)
              {
                _countDownPage.HeaderLogo = fileToUpolad(HttpContext.Current.Request.Files["headerLogo"]);
              }

              if (HttpContext.Current.Request.Files["mainBgImg"] != null)
              {
                _countDownPage.MainBgImg = fileToUpolad(HttpContext.Current.Request.Files["mainBgImg"]);
              }

              if (HttpContext.Current.Request.Files["innerImg"] != null)
              {
                _countDownPage.MainInnerImg = fileToUpolad(HttpContext.Current.Request.Files["innerImg"]);
              }

              if (HttpContext.Current.Request.Files["footerBgImg"] != null)
              {
                _countDownPage.FooterBgImg = fileToUpolad(HttpContext.Current.Request.Files["footerBgImg"]);
              }

            }
            #endregion

            #region :: Count-Down Page Add/Update

            if (_countDownPage.Id > 0) //Update Count Down Page
            {
              _countDownPage.UpdatedDate = DateTime.Now;
              _countDownPage.UpdatedBy = adminId;

              var oldCountDownPage = _DBContext.CountDownPages.Find(_countDownPage.Id);

              if (oldCountDownPage != null)
              {
                if (_countDownPage.HeaderLogo == null)
                {
                  _countDownPage.HeaderLogo = oldCountDownPage.HeaderLogo;
                }
                else
                {
                  fileToDelete(oldCountDownPage.HeaderLogo);
                }

                if (_countDownPage.MainBgImg == null)
                {
                  _countDownPage.MainBgImg = oldCountDownPage.MainBgImg;
                }
                else
                {
                  fileToDelete(oldCountDownPage.MainBgImg);
                }

                if (_countDownPage.MainInnerImg == null)
                {
                  _countDownPage.MainInnerImg = oldCountDownPage.MainInnerImg;
                }
                else
                {
                  fileToDelete(oldCountDownPage.MainInnerImg);
                }

                if (_countDownPage.FooterBgImg == null)
                {
                  _countDownPage.FooterBgImg = oldCountDownPage.FooterBgImg;
                }
                else
                {
                  fileToDelete(oldCountDownPage.FooterBgImg);
                }

                _countDownPage.CreatedDate = oldCountDownPage.CreatedDate;
                _countDownPage.CreatedBy = oldCountDownPage.CreatedBy;
              }

              _DBContext.CountDownPages.AddOrUpdate(_countDownPage);
              commonResponse.status = _DBContext.SaveChanges();
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.CountDownPage + Helper.recordUpdate_success;
              }
              else
              {
                commonResponse.message = Helper.CountDownPage + Helper.recordUpdate_unsuccess;
                commonResponse.status = Helper.failure_code;
              }
            }
            else // Add Count Down Page
            {
              _countDownPage.CreatedDate = DateTime.Now;
              _countDownPage.CreatedBy = adminId;
              _countDownPage = _DBContext.CountDownPages.Add(_countDownPage);
              commonResponse.status = _DBContext.SaveChanges();
              if (commonResponse.status == Helper.success_code)
              {
                commonResponse.message = Helper.CountDownPage + Helper.recordAdd_success;
              }
              else
              {
                commonResponse.message = Helper.CountDownPage + Helper.recordAdd_unsuccess;
                commonResponse.status = Helper.failure_code;
              }
            }
            #endregion
          }
          else
          {
            commonResponse.status = Helper.failure_code;
            commonResponse.message = Helper.CountDownPage + Helper.recordAdd_unsuccess;
          }
        }
        else
        {
          commonResponse.status = Helper.sessionout_code;
          commonResponse.message = Helper.session_out;
        }
      }
      catch (Exception ex)
      {
        commonResponse.status = Helper.error_code;
        commonResponse.message = ex.Message;
      }
      return Ok(commonResponse);
    }


    #endregion

    #region :: NON-ACTION

    /// <summary>
    /// Function for send forgorpassword email
    /// </summary>
    /// <param name="Email"></param>
    /// <param name="EmailBody"></param>
    /// <returns></returns>
    [NonAction]
    public int SendForgotPasswordEmail(string Email, string EmailBody)
    {
      int nRet = 0;
      try
      {
        try
        {
          var SmtpClient = Convert.ToString(ConfigurationManager.AppSettings["SmtpClient"]);
          var Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
          var UserName = Convert.ToString(ConfigurationManager.AppSettings["UserName"]);
          var Password = Convert.ToString(ConfigurationManager.AppSettings["Password"]);
          var EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SSL"]);
          var FromAddress = Convert.ToString(ConfigurationManager.AppSettings["From"]);

          SmtpClient client = new SmtpClient(SmtpClient, Port);
          client.UseDefaultCredentials = false;
          client.Credentials = new NetworkCredential(UserName, Password);
          if (EnableSSL)
          {
            client.EnableSsl = true;
          }
          MailMessage mailMessage = new MailMessage();
          mailMessage.From = new MailAddress(FromAddress);
          mailMessage.To.Add(Email);
          mailMessage.Body = EmailBody;
          mailMessage.IsBodyHtml = true;
          mailMessage.Subject = "Admin Password";
          mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
          client.Send(mailMessage);
          nRet = 1;
        }
        catch (SmtpException mailex)
        {
          nRet = -1;
        }
      }
      catch (Exception ex)
      {
        nRet = 0;
      }
      return nRet;
    }

    [NonAction]
    public string fileToUpolad(HttpPostedFile file)
    {
      string fPath = string.Empty;
      string directoryPath = string.Empty;
      string fileName = string.Empty;

      string Folder = Convert.ToString(ConfigurationManager.AppSettings["UploadImg"]);
      directoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
      fPath = Helper.GenerateRandomString(8) + Path.GetExtension(file.FileName);
      fileName = fPath;
      fPath = Path.Combine(directoryPath, fPath);
      if (!Directory.Exists(directoryPath))
      {
        Directory.CreateDirectory(directoryPath);
      }
      file.SaveAs(fPath);

      return fileName;
    }

    [NonAction]
    public void fileToDelete(string fileName)
    {
      if (fileName != null)
      {
        string Folder = Convert.ToString(ConfigurationManager.AppSettings["UploadImg"]);
        string directoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
        if (Directory.Exists(directoryPath) && File.Exists(Path.Combine(directoryPath, fileName)))
        {
          File.Delete(Path.Combine(directoryPath, fileName));
        }
      }
    }

    [NonAction]
    public static string GetRazorViewAsString(object model, string filePath)
    {
      var st = new StringWriter();
      var context = new HttpContextWrapper(HttpContext.Current);
      var routeData = new System.Web.Routing.RouteData();
      var controllerContext = new System.Web.Mvc.ControllerContext(new System.Web.Routing.RequestContext(context, routeData), new SectionController());
      var razor = new System.Web.Mvc.RazorView(controllerContext, filePath, null, false, null);
      razor.Render(new System.Web.Mvc.ViewContext(controllerContext, razor, new System.Web.Mvc.ViewDataDictionary(model), new System.Web.Mvc.TempDataDictionary(), st), st);
      return st.ToString();
    }


    #endregion
  }
}
