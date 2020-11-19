namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201909054 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FashionShows", "ShowDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FashionShows", "ShowDate", c => c.DateTime(nullable: false));
        }
    }
}
