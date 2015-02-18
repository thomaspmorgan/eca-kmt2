namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonMods2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Person", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop");
            DropIndex("dbo.Person", new[] { "ItineraryStop_ItineraryStopId" });
            CreateTable(
                "dbo.ItineraryStopParticipant",
                c => new
                    {
                        ItineraryStopId = c.Int(nullable: false),
                        ParticipantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItineraryStopId, t.ParticipantId })
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStopId, cascadeDelete: true)
                .ForeignKey("dbo.Participant", t => t.ParticipantId, cascadeDelete: true)
                .Index(t => t.ItineraryStopId)
                .Index(t => t.ParticipantId);
            
            DropColumn("dbo.Person", "ItineraryStop_ItineraryStopId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "ItineraryStop_ItineraryStopId", c => c.Int());
            DropForeignKey("dbo.ItineraryStopParticipant", "ParticipantId", "dbo.Participant");
            DropForeignKey("dbo.ItineraryStopParticipant", "ItineraryStopId", "dbo.ItineraryStop");
            DropIndex("dbo.ItineraryStopParticipant", new[] { "ParticipantId" });
            DropIndex("dbo.ItineraryStopParticipant", new[] { "ItineraryStopId" });
            DropTable("dbo.ItineraryStopParticipant");
            CreateIndex("dbo.Person", "ItineraryStop_ItineraryStopId");
            AddForeignKey("dbo.Person", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop", "ItineraryStopId");
        }
    }
}
