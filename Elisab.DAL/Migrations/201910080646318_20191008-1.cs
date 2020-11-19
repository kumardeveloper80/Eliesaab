namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201910081 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FashionShows", "ShowTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FashionShows", "ShowTime");
        }
    }
}
