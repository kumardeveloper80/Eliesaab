using System.Data.Entity;

namespace Elisab.DAL.Context
{
  public class ElisabDbContext : DbContext
  {
    public ElisabDbContext() : base("Elisab_Context")
    {
      var ensureDLLIsCopied =
      System.Data.Entity.SqlServer.SqlProviderServices.Instance;
    }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<FashionShow> FashionShows { get; set; }
    public DbSet<SecondPage> SecondPages { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<MainPage> MainPages { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<SectionMedia> SectionMedias { get; set; }
    public DbSet<ImageGallery> ImageGalleries { get; set; }
    public DbSet<GalleryLogin> GalleryLogins { get; set;  }
    public DbSet<CountDownPage> CountDownPages { get; set; }
  }
}
