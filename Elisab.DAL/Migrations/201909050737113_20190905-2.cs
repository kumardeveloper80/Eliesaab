namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201909052 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FashionShows", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.FashionShows", "CreatedBy", c => c.Int());
            AlterColumn("dbo.FashionShows", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.FashionShows", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.FashionShows", "DeletedDate", c => c.DateTime());
            AlterColumn("dbo.FashionShows", "DeletedBy", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FashionShows", "DeletedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.FashionShows", "DeletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FashionShows", "UpdatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.FashionShows", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FashionShows", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.FashionShows", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
