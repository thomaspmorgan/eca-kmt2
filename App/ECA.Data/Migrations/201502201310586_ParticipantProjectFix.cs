namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParticipantProjectFix : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.ParticipantProject", name: "Participant_ParticipantId", newName: "ParticipantId");
            //RenameColumn(table: "dbo.ParticipantProject", name: "Project_ProjectId", newName: "ProjectId");
            //RenameIndex(table: "dbo.ParticipantProject", name: "IX_Participant_ParticipantId", newName: "IX_ParticipantId");
            //RenameIndex(table: "dbo.ParticipantProject", name: "IX_Project_ProjectId", newName: "IX_ProjectId");
        }
        
        public override void Down()
        {
            //RenameIndex(table: "dbo.ParticipantProject", name: "IX_ProjectId", newName: "IX_Project_ProjectId");
            //RenameIndex(table: "dbo.ParticipantProject", name: "IX_ParticipantId", newName: "IX_Participant_ParticipantId");
            //RenameColumn(table: "dbo.ParticipantProject", name: "ProjectId", newName: "Project_ProjectId");
            //RenameColumn(table: "dbo.ParticipantProject", name: "ParticipantId", newName: "Participant_ParticipantId");
        }
    }
}
