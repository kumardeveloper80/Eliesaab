using AutoMapper;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elisab.BAL.Repository
{
  public class FashionShowsRepository
  {
    ElisabDbContext _context;

    public FashionShowsRepository()
    {
      _context = new ElisabDbContext();
    }

    public FashionShows_VM Get(int id)
    {
      var result = _context.FashionShows.Where(x => x.Id == id).FirstOrDefault();
      if (result != null)
      {
        //result.ShowDate = result.ShowDate.Date;
        return Mapper.Map<FashionShows_VM>(result);
      }
      return null;
    }

    public List<FashionShows_VM> GetAll()
    {
      var output = new List<FashionShows_VM>();
      var result = _context.FashionShows.Where(x => x.DeletedDate == null).ToList();
      if (result.Any())
      {
        output = Mapper.Map<List<FashionShows_VM>>(result);
      }
      return output;
    }

    public int Add(FashionShows_VM fashionShows_VM)
    {
      if (fashionShows_VM.IsActive == true)
      {
        UpdateIsActive();
      }

      var obj = Mapper.Map<FashionShow>(fashionShows_VM);
      obj.CreatedDate = DateTime.Now;
      obj.CreatedBy = fashionShows_VM.UserId;
      _context.FashionShows.Add(obj);
      return _context.SaveChanges();
    }

    public int Update(FashionShows_VM fashionShows_VM)
    {
      var result = _context.FashionShows.Where(x => x.Id == fashionShows_VM.Id).FirstOrDefault();
      if (result != null)
      {
        if (fashionShows_VM.IsActive == true)
        {
          UpdateIsActive();
        }

        result.Name = fashionShows_VM.Name;
        result.ShowDate = fashionShows_VM.ShowDate;
        result.ShowTime = fashionShows_VM.ShowTime;
        result.IsActive = fashionShows_VM.IsActive;
        result.UpdatedDate = DateTime.Now;
        result.UpdatedBy = fashionShows_VM.UserId;
        return _context.SaveChanges();
      }
      return 0;
    }

    public int Delete(int Id, int DeletedBy)
    {
      var result = _context.FashionShows.Where(x => x.Id == Id).FirstOrDefault();
      if (result != null)
      {
        result.DeletedDate = DateTime.Now;
        result.DeletedBy = DeletedBy;
        return _context.SaveChanges();
      }
      return 0;
    }

    public void UpdateIsActive()
    {
      var list = _context.FashionShows.Where(x => x.IsActive == true && x.DeletedDate == null).ToList();
      if (list.Any())
      {
        foreach (var item in list)
        {
          item.IsActive = false;
          _context.SaveChanges();
        }
      }
    }

    public int UpdateIsActiveById(int Id, bool IsActive, int UpdatedBy)
    {
      UpdateIsActive();
      var result = _context.FashionShows.Where(x => x.DeletedDate == null && x.Id == Id).FirstOrDefault();
      if (result != null)
      {
        result.IsActive = IsActive;
        result.UpdatedBy = UpdatedBy;
        result.UpdatedDate = DateTime.Now;
        return _context.SaveChanges();
      }
      return 0;
    }

    public LandingPage_VM GetLandingPage(int Id)
    {
      try
      {
        var fashionShow = new List<FashionShow>();
        bool isFuture = false;
        if (Id > 0)
        {
          fashionShow = _context.FashionShows.Where(x => x.DeletedDate == null && x.Id == Id).ToList();
          isFuture = false;
        }
        else
        {
          fashionShow = _context.FashionShows.Where(x => x.IsActive == true && x.DeletedDate == null).ToList();
          if (fashionShow != null)
          {
            if (Convert.ToDateTime(fashionShow.FirstOrDefault().ShowDate) > DateTime.Now)
            {
              isFuture = true;
            }
          }
         
        }

        if (fashionShow.Any())
        {

          var output = new LandingPage_VM();
         
          output = (from fs in fashionShow

                        join mp in _context.MainPages.Where(x=>x.IsActive) on fs.Id equals mp.FashionShowId into mainPage
                        from mp in mainPage.DefaultIfEmpty()

                        join sp in _context.SecondPages.Where(x => x.IsActive) on fs.Id equals sp.FashionShowId into secondPage
                        from sp in secondPage.DefaultIfEmpty()

                        join address in _context.Address.Where(x => x.IsActive) on fs.Id equals address.FashionShowId into addressPage
                        from address in addressPage.DefaultIfEmpty()

                        //join section in _context.Sections on fs.Id equals section.FashionShowId into sectionPage
                        //from section in sectionPage.DefaultIfEmpty()

                        select new LandingPage_VM
                        {
                          fashionId = fs.Id,
                          mainPage = mp != null ? new MainPage_VM()
                          {
                            BackgroundImage = mp.BackgroundImage != null ? mp.BackgroundImage : string.Empty,
                            LogoImage = mp.LogoImage != null ? mp.LogoImage : string.Empty,
                            InnerImage = mp.InnerImage != null ? mp.InnerImage : string.Empty,
                            ContentText = mp.ContentText != null ? mp.ContentText : string.Empty,
                          } : null,
                          secondPage = sp != null ? new SecondPage_VM()
                          {
                            HtmlContent1 = sp.HtmlContent1 != null ? sp.HtmlContent1 : string.Empty,
                            HtmlContent2 = sp.HtmlContent2 != null ? sp.HtmlContent2 : string.Empty,
                            HtmlContent3 = sp.HtmlContent3 != null ? sp.HtmlContent3 : string.Empty,
                            HtmlContent4 = sp.HtmlContent4 != null ? sp.HtmlContent4 : string.Empty,
                            HtmlContent5 = sp.HtmlContent5 != null ? sp.HtmlContent5 : string.Empty,
                            Image1 = sp.Image1 != null ? sp.Image1 : string.Empty,
                            Image2 = sp.Image2 != null ? sp.Image2 : string.Empty,
                            Image3 = sp.Image3 != null ? sp.Image3 : string.Empty,
                            Image4 = sp.Image4 != null ? sp.Image4 : string.Empty,
                            Image5 = sp.Image5 != null ? sp.Image4 : string.Empty,
                              IsShowAtLast = sp.IsShowAtLast,
                          } : null,
                          addressPage = address != null ? new Address_VM()
                          {
                            HeaderText = address.HeaderText != null ? address.HeaderText : string.Empty,
                            ImageName = address.ImageName != null ? address.ImageName : string.Empty,
                          } : null,

                          //sectionPage = section != null ?
                          //(from sec in _context.Sections

                          // join sm in _context.SectionMedias on sec.Id equals sm.SectionId
                          // orderby section.Sequence

                          // where section.DeletedDate == null && section.Id == sec.Id

                          // select new Section_VM
                          // {
                          //   MediaType = section.MediaType,
                          //   Description = section.Description != null ? section.Description : string.Empty,
                          //   sectionMedia = (from sm in _context.SectionMedias
                          //                   where sm.SectionId == section.Id
                          //                   select new SectionMedia_VM
                          //                   {
                          //                     MediaName = sm.MediaName,
                          //                     PosterImageName = sm.PosterImageName != null ? sm.PosterImageName : string.Empty,
                          //                   }).ToList()
                          // }).ToList() : null

                        }).FirstOrDefault();

          if(output == null)
          {
            output = new LandingPage_VM();
          }

          output.sectionPage = (from fs in fashionShow
                                join section in _context.Sections on fs.Id equals section.FashionShowId
                                orderby section.Sequence
                                where section.DeletedDate == null
                                select new Section_VM
                                {
                                  MediaType = section.MediaType,
                                  Description = section.Description != null ? section.Description : string.Empty,
                                  sectionMedia = (from sm in _context.SectionMedias
                                                  where sm.SectionId == section.Id
                                                  select new SectionMedia_VM
                                                  {
                                                    MediaName = sm.MediaName,
                                                    PosterImageName = sm.PosterImageName != null ? sm.PosterImageName : string.Empty,
                                                  }).ToList()
                                }).ToList();

          output.IsFutureEvent = isFuture;
          return output;
        }
        else
        {
          return null;
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }
  }
}
