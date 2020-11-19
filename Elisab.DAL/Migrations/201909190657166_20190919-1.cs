namespace Elisab.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201909191 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sections", "Sequence", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sections", "Sequence");
        }
    }
}
