namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameLocationCountryToLocation : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Location", "CountryName", "LocationName");
            RenameColumn("dbo.Location", "CountryIso", "LocationIso");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Location", "LocationName", "CountryName");
            RenameColumn("dbo.Location", "LocationIso", "CountryIso");
        }
    }
}
