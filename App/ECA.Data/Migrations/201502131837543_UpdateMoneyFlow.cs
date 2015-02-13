namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMoneyFlow : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PersonEvent", newName: "EventPerson");
            DropForeignKey("dbo.MoneyFlow", "PersonId", "dbo.Person");
            DropForeignKey("dbo.MoneyFlow", "ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.MoneyFlow", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.MoneyFlow", "ProjectId", "dbo.Project");
            DropIndex("dbo.MoneyFlow", new[] { "ProgramId" });
            DropIndex("dbo.MoneyFlow", new[] { "ProjectId" });
            DropIndex("dbo.MoneyFlow", new[] { "PersonId" });
            DropIndex("dbo.MoneyFlow", new[] { "ItineraryStopId" });
            DropIndex("dbo.MoneyFlow", new[] { "Recipient_OrganizationId" });
            DropIndex("dbo.MoneyFlow", new[] { "Source_OrganizationId" });
            RenameColumn(table: "dbo.OrganizationContact", name: "Organization_OrganizationId", newName: "OrganizationId");
            RenameColumn(table: "dbo.OrganizationContact", name: "Contact_ContactId", newName: "ContactId");
            RenameColumn(table: "dbo.MoneyFlow", name: "Recipient_OrganizationId", newName: "RecipientOrganizationId");
            RenameColumn(table: "dbo.MoneyFlow", name: "Source_OrganizationId", newName: "SourceOrganizationId");
            RenameColumn(table: "dbo.MoneyFlow", name: "AccommodationId", newName: "RecipientAccommodationId");
            RenameIndex(table: "dbo.MoneyFlow", name: "IX_AccommodationId", newName: "IX_RecipientAccommodationId");
            RenameIndex(table: "dbo.OrganizationContact", name: "IX_Organization_OrganizationId", newName: "IX_OrganizationId");
            RenameIndex(table: "dbo.OrganizationContact", name: "IX_Contact_ContactId", newName: "IX_ContactId");
            DropPrimaryKey("dbo.EventPerson");
            CreateTable(
                "dbo.MoneyFlowSourceRecipientType",
                c => new
                    {
                        MoneyFlowSourceRecipientTypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.MoneyFlowSourceRecipientTypeId);
            
            AddColumn("dbo.MoneyFlow", "SourceTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.MoneyFlow", "RecipientTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.MoneyFlow", "SourceProgramId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "RecipientProgramId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "SourceProjectId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "RecipientProjectId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "SourceParticipantId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "RecipientParticipantId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "SourceItineraryStopId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "RecipientItineraryStopId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "RecipientTransportationId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "Program_ProgramId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "Project_ProjectId", c => c.Int());
            AlterColumn("dbo.MoneyFlow", "RecipientOrganizationId", c => c.Int());
            AlterColumn("dbo.MoneyFlow", "SourceOrganizationId", c => c.Int());
            AddPrimaryKey("dbo.EventPerson", new[] { "Event_EventId", "Person_PersonId" });
            CreateIndex("dbo.MoneyFlow", "SourceOrganizationId");
            CreateIndex("dbo.MoneyFlow", "RecipientOrganizationId");
            CreateIndex("dbo.MoneyFlow", "SourceTypeId");
            CreateIndex("dbo.MoneyFlow", "RecipientTypeId");
            CreateIndex("dbo.MoneyFlow", "SourceProgramId");
            CreateIndex("dbo.MoneyFlow", "RecipientProgramId");
            CreateIndex("dbo.MoneyFlow", "SourceProjectId");
            CreateIndex("dbo.MoneyFlow", "RecipientProjectId");
            CreateIndex("dbo.MoneyFlow", "SourceParticipantId");
            CreateIndex("dbo.MoneyFlow", "RecipientParticipantId");
            CreateIndex("dbo.MoneyFlow", "SourceItineraryStopId");
            CreateIndex("dbo.MoneyFlow", "RecipientItineraryStopId");
            CreateIndex("dbo.MoneyFlow", "RecipientTransportationId");
            CreateIndex("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId");
            CreateIndex("dbo.MoneyFlow", "Program_ProgramId");
            CreateIndex("dbo.MoneyFlow", "Project_ProjectId");
            AddForeignKey("dbo.MoneyFlow", "RecipientItineraryStopId", "dbo.ItineraryStop", "ItineraryStopId");
            AddForeignKey("dbo.MoneyFlow", "RecipientParticipantId", "dbo.Participant", "ParticipantId");
            AddForeignKey("dbo.MoneyFlow", "RecipientProgramId", "dbo.Program", "ProgramId");
            AddForeignKey("dbo.MoneyFlow", "RecipientProjectId", "dbo.Project", "ProjectId");
            AddForeignKey("dbo.MoneyFlow", "RecipientTransportationId", "dbo.Transportation", "TransportationId");
            AddForeignKey("dbo.MoneyFlow", "RecipientTypeId", "dbo.MoneyFlowSourceRecipientType", "MoneyFlowSourceRecipientTypeId", cascadeDelete: false);
            AddForeignKey("dbo.MoneyFlow", "SourceItineraryStopId", "dbo.ItineraryStop", "ItineraryStopId");
            AddForeignKey("dbo.MoneyFlow", "SourceParticipantId", "dbo.Participant", "ParticipantId");
            AddForeignKey("dbo.MoneyFlow", "SourceProgramId", "dbo.Program", "ProgramId");
            AddForeignKey("dbo.MoneyFlow", "SourceProjectId", "dbo.Project", "ProjectId");
            AddForeignKey("dbo.MoneyFlow", "SourceTypeId", "dbo.MoneyFlowSourceRecipientType", "MoneyFlowSourceRecipientTypeId", cascadeDelete: false);
            AddForeignKey("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop", "ItineraryStopId");
            AddForeignKey("dbo.MoneyFlow", "Program_ProgramId", "dbo.Program", "ProgramId");
            AddForeignKey("dbo.MoneyFlow", "Project_ProjectId", "dbo.Project", "ProjectId");
            DropColumn("dbo.MoneyFlow", "PersonId");
            DropColumn("dbo.Participant", "IsRecipient");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Participant", "IsRecipient", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyFlow", "PersonId", c => c.Int());
            DropForeignKey("dbo.MoneyFlow", "Project_ProjectId", "dbo.Project");
            DropForeignKey("dbo.MoneyFlow", "Program_ProgramId", "dbo.Program");
            DropForeignKey("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.MoneyFlow", "SourceTypeId", "dbo.MoneyFlowSourceRecipientType");
            DropForeignKey("dbo.MoneyFlow", "SourceProjectId", "dbo.Project");
            DropForeignKey("dbo.MoneyFlow", "SourceProgramId", "dbo.Program");
            DropForeignKey("dbo.MoneyFlow", "SourceParticipantId", "dbo.Participant");
            DropForeignKey("dbo.MoneyFlow", "SourceItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.MoneyFlow", "RecipientTypeId", "dbo.MoneyFlowSourceRecipientType");
            DropForeignKey("dbo.MoneyFlow", "RecipientTransportationId", "dbo.Transportation");
            DropForeignKey("dbo.MoneyFlow", "RecipientProjectId", "dbo.Project");
            DropForeignKey("dbo.MoneyFlow", "RecipientProgramId", "dbo.Program");
            DropForeignKey("dbo.MoneyFlow", "RecipientParticipantId", "dbo.Participant");
            DropForeignKey("dbo.MoneyFlow", "RecipientItineraryStopId", "dbo.ItineraryStop");
            DropIndex("dbo.MoneyFlow", new[] { "Project_ProjectId" });
            DropIndex("dbo.MoneyFlow", new[] { "Program_ProgramId" });
            DropIndex("dbo.MoneyFlow", new[] { "ItineraryStop_ItineraryStopId" });
            DropIndex("dbo.MoneyFlow", new[] { "RecipientTransportationId" });
            DropIndex("dbo.MoneyFlow", new[] { "RecipientItineraryStopId" });
            DropIndex("dbo.MoneyFlow", new[] { "SourceItineraryStopId" });
            DropIndex("dbo.MoneyFlow", new[] { "RecipientParticipantId" });
            DropIndex("dbo.MoneyFlow", new[] { "SourceParticipantId" });
            DropIndex("dbo.MoneyFlow", new[] { "RecipientProjectId" });
            DropIndex("dbo.MoneyFlow", new[] { "SourceProjectId" });
            DropIndex("dbo.MoneyFlow", new[] { "RecipientProgramId" });
            DropIndex("dbo.MoneyFlow", new[] { "SourceProgramId" });
            DropIndex("dbo.MoneyFlow", new[] { "RecipientTypeId" });
            DropIndex("dbo.MoneyFlow", new[] { "SourceTypeId" });
            DropIndex("dbo.MoneyFlow", new[] { "RecipientOrganizationId" });
            DropIndex("dbo.MoneyFlow", new[] { "SourceOrganizationId" });
            DropPrimaryKey("dbo.EventPerson");
            AlterColumn("dbo.MoneyFlow", "SourceOrganizationId", c => c.Int(nullable: false));
            AlterColumn("dbo.MoneyFlow", "RecipientOrganizationId", c => c.Int(nullable: false));
            DropColumn("dbo.MoneyFlow", "Project_ProjectId");
            DropColumn("dbo.MoneyFlow", "Program_ProgramId");
            DropColumn("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId");
            DropColumn("dbo.MoneyFlow", "RecipientTransportationId");
            DropColumn("dbo.MoneyFlow", "RecipientItineraryStopId");
            DropColumn("dbo.MoneyFlow", "SourceItineraryStopId");
            DropColumn("dbo.MoneyFlow", "RecipientParticipantId");
            DropColumn("dbo.MoneyFlow", "SourceParticipantId");
            DropColumn("dbo.MoneyFlow", "RecipientProjectId");
            DropColumn("dbo.MoneyFlow", "SourceProjectId");
            DropColumn("dbo.MoneyFlow", "RecipientProgramId");
            DropColumn("dbo.MoneyFlow", "SourceProgramId");
            DropColumn("dbo.MoneyFlow", "RecipientTypeId");
            DropColumn("dbo.MoneyFlow", "SourceTypeId");
            DropTable("dbo.MoneyFlowSourceRecipientType");
            AddPrimaryKey("dbo.EventPerson", new[] { "Person_PersonId", "Event_EventId" });
            RenameIndex(table: "dbo.OrganizationContact", name: "IX_ContactId", newName: "IX_Contact_ContactId");
            RenameIndex(table: "dbo.OrganizationContact", name: "IX_OrganizationId", newName: "IX_Organization_OrganizationId");
            RenameIndex(table: "dbo.MoneyFlow", name: "IX_RecipientAccommodationId", newName: "IX_AccommodationId");
            RenameColumn(table: "dbo.MoneyFlow", name: "RecipientAccommodationId", newName: "AccommodationId");
            RenameColumn(table: "dbo.MoneyFlow", name: "SourceOrganizationId", newName: "Source_OrganizationId");
            RenameColumn(table: "dbo.MoneyFlow", name: "RecipientOrganizationId", newName: "Recipient_OrganizationId");
            RenameColumn(table: "dbo.OrganizationContact", name: "ContactId", newName: "Contact_ContactId");
            RenameColumn(table: "dbo.OrganizationContact", name: "OrganizationId", newName: "Organization_OrganizationId");
            CreateIndex("dbo.MoneyFlow", "Source_OrganizationId");
            CreateIndex("dbo.MoneyFlow", "Recipient_OrganizationId");
            CreateIndex("dbo.MoneyFlow", "ItineraryStopId");
            CreateIndex("dbo.MoneyFlow", "PersonId");
            CreateIndex("dbo.MoneyFlow", "ProjectId");
            CreateIndex("dbo.MoneyFlow", "ProgramId");
            AddForeignKey("dbo.MoneyFlow", "ProjectId", "dbo.Project", "ProjectId");
            AddForeignKey("dbo.MoneyFlow", "ProgramId", "dbo.Program", "ProgramId");
            AddForeignKey("dbo.MoneyFlow", "ItineraryStopId", "dbo.ItineraryStop", "ItineraryStopId");
            AddForeignKey("dbo.MoneyFlow", "PersonId", "dbo.Person", "PersonId");
            RenameTable(name: "dbo.EventPerson", newName: "PersonEvent");
        }
    }
}
