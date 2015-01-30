namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDB3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Location", "Region_LocationId", "dbo.Location");
            DropIndex("dbo.Location", new[] { "Region_LocationId" });
            DropColumn("dbo.Location", "Region_LocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Location", "Region_LocationId", c => c.Int());
            CreateIndex("dbo.Location", "Region_LocationId");
            AddForeignKey("dbo.Location", "Region_LocationId", "dbo.Location", "LocationId");
        }
    }
}
