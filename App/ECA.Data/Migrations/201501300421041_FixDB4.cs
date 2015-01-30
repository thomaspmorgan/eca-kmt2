namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDB4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "Region_LocationId", c => c.Int());
            CreateIndex("dbo.Location", "Region_LocationId");
            AddForeignKey("dbo.Location", "Region_LocationId", "dbo.Location", "LocationId");
            AddColumn("dbo.Location", "Country_LocationId", c => c.Int());
            CreateIndex("dbo.Location", "Country_LocationId");
            AddForeignKey("dbo.Location", "Country_LocationId", "dbo.Location", "LocationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Location", "Region_LocationId", "dbo.Location");
            DropIndex("dbo.Location", new[] { "Region_LocationId" });
            DropColumn("dbo.Location", "Region_LocationId");
            DropForeignKey("dbo.Location", "Country_LocationId", "dbo.Location");
            DropIndex("dbo.Location", new[] { "Country_LocationId" });
            DropColumn("dbo.Location", "Country_LocationId");
        }
    }
}
