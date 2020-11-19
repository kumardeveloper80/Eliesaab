namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _09042019_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "Email", c => c.String());
            AlterColumn("dbo.Admins", "Username", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Admins", "Username", c => c.Int(nullable: false));
            DropColumn("dbo.Admins", "Email");
        }
    }
}
