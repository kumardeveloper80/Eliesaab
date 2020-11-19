namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _09112019MainPage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MainPages", "InnerImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MainPages", "InnerImage");
        }
    }
}
