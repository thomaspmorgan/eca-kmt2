namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParticipant : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ParticipantType", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ParticipantStatus", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ParticipantStatus", "Project_ProjectId", "dbo.Project");
            DropIndex("dbo.ParticipantType", new[] { "Person_PersonId" });
            DropIndex("dbo.ParticipantStatus", new[] { "Person_PersonId" });
            DropIndex("dbo.ParticipantStatus", new[] { "Project_ProjectId" });
            CreateTable(
                "dbo.Participant",
                c => new
                    {
                        ParticipantId = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(),
                        PersonId = c.Int(),
                        ParticipantTypeId = c.Int(nullable: false),
                        IsRecipient = c.Boolean(nullable: false, defaultValue: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ParticipantId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .ForeignKey("dbo.ParticipantType", t => t.ParticipantTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId)
                .Index(t => t.OrganizationId)
                .Index(t => t.PersonId)
                .Index(t => t.ParticipantTypeId);
            
            CreateTable(
                "dbo.ProjectStatus",
                c => new
                    {
                        ProjectStatusId = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ProjectStatusId);
            
            CreateTable(
                "dbo.ParticipantProject",
                c => new
                    {
                        ParticipantId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParticipantId, t.ProjectId })
                .ForeignKey("dbo.Participant", t => t.ParticipantId, cascadeDelete: true)
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ParticipantId)
                .Index(t => t.ProjectId);
            
            AddColumn("dbo.Project", "ProjectStatusId", c => c.Int(nullable: false));
            AddColumn("dbo.ParticipantStatus", "StatusDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.ParticipantStatus", "ParticipantId", c => c.Int(nullable: false));
            CreateIndex("dbo.Project", "ProjectStatusId");
            CreateIndex("dbo.ParticipantStatus", "ParticipantId");
            //AddForeignKey("dbo.Project", "ProjectStatusId", "dbo.ProjectStatus", "ProjectStatusId", cascadeDelete: true);
            AddForeignKey("dbo.ParticipantStatus", "ParticipantId", "dbo.Participant", "ParticipantId", cascadeDelete: true);
            DropColumn("dbo.ParticipantType", "Person_PersonId");
            DropColumn("dbo.Project", "Status");
            DropColumn("dbo.ParticipantStatus", "Person_PersonId");
            DropColumn("dbo.ParticipantStatus", "Project_ProjectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParticipantStatus", "Project_ProjectId", c => c.Int());
            AddColumn("dbo.ParticipantStatus", "Person_PersonId", c => c.Int());
            AddColumn("dbo.ParticipantStatus", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Project", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.ParticipantType", "Person_PersonId", c => c.Int());
            DropForeignKey("dbo.ParticipantStatus", "Status_ParticipantStatusId", "dbo.ParticipantStatus");
            DropForeignKey("dbo.ParticipantStatus", "Participant_ParticipantId", "dbo.Participant");
            DropForeignKey("dbo.Project", "Status_ProjectStatusId", "dbo.ProjectStatus");
            DropForeignKey("dbo.ParticipantProject", "Project_ProjectId", "dbo.Project");
            DropForeignKey("dbo.ParticipantProject", "Participant_ParticipantId", "dbo.Participant");
            DropForeignKey("dbo.Participant", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Participant", "ParticipantTypeId", "dbo.ParticipantType");
            DropForeignKey("dbo.Participant", "OrganizationId", "dbo.Organization");
            DropIndex("dbo.ParticipantProject", new[] { "Project_ProjectId" });
            DropIndex("dbo.ParticipantProject", new[] { "Participant_ParticipantId" });
            DropIndex("dbo.ParticipantStatus", new[] { "Status_ParticipantStatusId" });
            DropIndex("dbo.ParticipantStatus", new[] { "Participant_ParticipantId" });
            DropIndex("dbo.Participant", new[] { "ParticipantTypeId" });
            DropIndex("dbo.Participant", new[] { "PersonId" });
            DropIndex("dbo.Participant", new[] { "OrganizationId" });
            DropIndex("dbo.Project", new[] { "Status_ProjectStatusId" });
            DropColumn("dbo.ParticipantStatus", "Status_ParticipantStatusId");
            DropColumn("dbo.ParticipantStatus", "Participant_ParticipantId");
            DropColumn("dbo.ParticipantStatus", "StatusDate");
            DropColumn("dbo.Project", "Status_ProjectStatusId");
            DropTable("dbo.ParticipantProject");
            DropTable("dbo.ProjectStatus");
            DropTable("dbo.Participant");
            CreateIndex("dbo.ParticipantStatus", "Project_ProjectId");
            CreateIndex("dbo.ParticipantStatus", "Person_PersonId");
            CreateIndex("dbo.ParticipantType", "Person_PersonId");
            AddForeignKey("dbo.ParticipantStatus", "Project_ProjectId", "dbo.Project", "ProjectId");
            AddForeignKey("dbo.ParticipantStatus", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ParticipantType", "Person_PersonId", "dbo.Person", "PersonId");
        }
    }
}
