namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201909192 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SectionMedias", "PosterImageName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SectionMedias", "PosterImageName");
        }
    }
}
