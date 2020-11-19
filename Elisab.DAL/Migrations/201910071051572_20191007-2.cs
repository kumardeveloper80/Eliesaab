namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201910072 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CountDownPages", "HeaderLogo", c => c.String());
            DropColumn("dbo.CountDownPages", "HeaderText");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CountDownPages", "HeaderText", c => c.String());
            DropColumn("dbo.CountDownPages", "HeaderLogo");
        }
    }
}
