namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201909051 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FashionShows", "ShowDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FashionShows", "ShowDate", c => c.String());
        }
    }
}
