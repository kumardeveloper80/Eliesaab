namespace Elisab.DAL.Migrations
{
  using Elisab.DAL.Context;
  using System.Data.Entity.Migrations;
  using System.Linq;

  public sealed class Configuration : DbMigrationsConfiguration<ElisabDbContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = true;
    }

    protected override void Seed(ElisabDbContext context)
    {
      //  This method will be called after migrating to the latest version.

      //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
      //  to avoid creating duplicate seed data. E.g.

      if (!context.Admins.Any())
      {
        context.Admins.AddOrUpdate(
        new Admin
        {
          Username = "testadmin",
          Password = "ATYMPFxr19nCirnAWqG99gtRqOIQYKjtYJOTbPE71hg=",
          Email = "test@test.com"
        });
      }
    }
  }
}
