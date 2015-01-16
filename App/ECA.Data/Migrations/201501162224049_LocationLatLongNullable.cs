namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationLatLongNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Location", "Latitude", c => c.Single());
            AlterColumn("dbo.Location", "Longitude", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Location", "Longitude", c => c.Single(nullable: false));
            AlterColumn("dbo.Location", "Latitude", c => c.Single(nullable: false));
        }
    }
}
