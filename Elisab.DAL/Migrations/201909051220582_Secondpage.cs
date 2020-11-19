namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Secondpage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SecondPages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FashionShowId = c.Int(nullable: false),
                        HtmlContent1 = c.String(),
                        Image1 = c.String(),
                        HtmlContent2 = c.String(),
                        Image2 = c.String(),
                        HtmlContent3 = c.String(),
                        Image3 = c.String(),
                        HtmlContent4 = c.String(),
                        Image4 = c.String(),
                        HtmlContent5 = c.String(),
                        Image5 = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SecondPages");
        }
    }
}
