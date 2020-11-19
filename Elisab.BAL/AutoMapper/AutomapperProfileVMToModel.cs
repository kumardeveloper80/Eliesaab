using AutoMapper;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;

namespace Elisab.BAL.AutoMapper
{
  public class AutomapperProfileVMToModel : Profile
  {
    public AutomapperProfileVMToModel()
    {
      CreateMap<FashionShows_VM, FashionShow>();
      CreateMap<Address_VM, Address>();
      CreateMap<SecondPage_VM, SecondPage>();
      CreateMap<MainPage_VM, MainPage>();
      CreateMap<Section_VM, Section>();
      CreateMap<ImageGallery_VM , ImageGallery>();
      CreateMap<GalleryLogin_VM , GalleryLogin>();
      CreateMap<CountDownPage_VM, CountDownPage>();
    }
  }
}
