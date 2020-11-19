namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _09042019_FashionShow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FashionShows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShowDate = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        DeletedDate = c.DateTime(nullable: false),
                        DeletedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FashionShows");
        }
    }
}
