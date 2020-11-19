using AutoMapper;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using System.Collections.Generic;
using System.Linq;

namespace Elisab.BAL.Repository
{
  public class ImageGalleryRepository
  {
    ElisabDbContext _context;

    public ImageGalleryRepository()
    {
      _context = new ElisabDbContext();
    }

    public List<ImageGallery_VM> GetByFashionShowId(int id)
    {

      List<ImageGallery_VM> result = (from f in _context.FashionShows
                                      join g in _context.ImageGalleries on f.Id equals g.FashionShowId
                                      where f.Id == id
                                      select new ImageGallery_VM
                                      {
                                        Id = g.Id,
                                        FashionShowId = g.FashionShowId,
                                        ThumbImage = g.ThumbImage,
                                        Image = g.Image
                                      }).ToList();
      
      if (result != null)
      {
        return Mapper.Map<List<ImageGallery_VM>>(result);
      }
      return null;
    }

    public int Delete(int id)
    {
      var result = _context.ImageGalleries.Where(x => x.Id == id).FirstOrDefault();
      if (result != null)
      {
        _context.ImageGalleries.Remove(result);
        return _context.SaveChanges();
      }
      return 0;
    }
  }
}
