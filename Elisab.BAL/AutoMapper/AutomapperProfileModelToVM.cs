using AutoMapper;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;

namespace Elisab.BAL.AutoMapper
{
  public class AutomapperProfileModelToVM : Profile
  {
    public AutomapperProfileModelToVM()
    {
      CreateMap<FashionShow, FashionShows_VM>();
      CreateMap<Address, Address_VM>();
      CreateMap<SecondPage, SecondPage_VM>();
      CreateMap<MainPage, MainPage_VM>();
      CreateMap<Section, Section_VM>();
      CreateMap<ImageGallery, ImageGallery_VM>();
      CreateMap<GalleryLogin, GalleryLogin_VM>();
      CreateMap<CountDownPage, CountDownPage_VM>();
    }
  }
}
