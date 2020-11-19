namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _09162019ImageGallery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageGalleries", "ThumbImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageGalleries", "ThumbImage");
        }
    }
}
