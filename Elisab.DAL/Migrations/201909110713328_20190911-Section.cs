namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20190911Section : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SectionMedias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionId = c.Int(nullable: false),
                        MediaName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FashionShowId = c.Int(nullable: false),
                        Description = c.String(),
                        MediaType = c.String(),
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
            DropTable("dbo.Sections");
            DropTable("dbo.SectionMedias");
        }
    }
}
