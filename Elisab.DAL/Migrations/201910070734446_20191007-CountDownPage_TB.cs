namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20191007CountDownPage_TB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CountDownPages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FashionShowId = c.Int(nullable: false),
                        HeaderText = c.String(),
                        MainContent = c.String(),
                        MainBgImg = c.String(),
                        MainInnerImg = c.String(),
                        FooterBgImg = c.String(),
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
            DropTable("dbo.CountDownPages");
        }
    }
}
