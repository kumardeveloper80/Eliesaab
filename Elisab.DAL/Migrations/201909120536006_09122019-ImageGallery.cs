namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _09122019ImageGallery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageGalleries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FashionShowId = c.Int(nullable: false),
                        Image = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ImageGalleries");
        }
    }
}
