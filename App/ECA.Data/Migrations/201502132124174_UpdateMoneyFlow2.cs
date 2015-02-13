namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMoneyFlow2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.MoneyFlow", "Program_ProgramId", "dbo.Program");
            DropForeignKey("dbo.MoneyFlow", "Project_ProjectId", "dbo.Project");
            DropIndex("dbo.MoneyFlow", new[] { "ItineraryStop_ItineraryStopId" });
            DropIndex("dbo.MoneyFlow", new[] { "Program_ProgramId" });
            DropIndex("dbo.MoneyFlow", new[] { "Project_ProjectId" });
            RenameColumn(table: "dbo.MoneyFlow", name: "MoneyFlowStatus_MoneyFlowStatusId", newName: "MoneyFlowStatusId");
            RenameColumn(table: "dbo.MoneyFlow", name: "MoneyFlowType_MoneyFlowTypeId", newName: "MoneyFlowTypeId");
            RenameIndex(table: "dbo.MoneyFlow", name: "IX_MoneyFlowType_MoneyFlowTypeId", newName: "IX_MoneyFlowTypeId");
            RenameIndex(table: "dbo.MoneyFlow", name: "IX_MoneyFlowStatus_MoneyFlowStatusId", newName: "IX_MoneyFlowStatusId");
            DropColumn("dbo.MoneyFlow", "ProgramId");
            DropColumn("dbo.MoneyFlow", "ProjectId");
            DropColumn("dbo.MoneyFlow", "ItineraryStopId");
            DropColumn("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId");
            DropColumn("dbo.MoneyFlow", "Program_ProgramId");
            DropColumn("dbo.MoneyFlow", "Project_ProjectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MoneyFlow", "Project_ProjectId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "Program_ProgramId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "ItineraryStopId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "ProjectId", c => c.Int());
            AddColumn("dbo.MoneyFlow", "ProgramId", c => c.Int());
            RenameIndex(table: "dbo.MoneyFlow", name: "IX_MoneyFlowStatusId", newName: "IX_MoneyFlowStatus_MoneyFlowStatusId");
            RenameIndex(table: "dbo.MoneyFlow", name: "IX_MoneyFlowTypeId", newName: "IX_MoneyFlowType_MoneyFlowTypeId");
            RenameColumn(table: "dbo.MoneyFlow", name: "MoneyFlowTypeId", newName: "MoneyFlowType_MoneyFlowTypeId");
            RenameColumn(table: "dbo.MoneyFlow", name: "MoneyFlowStatusId", newName: "MoneyFlowStatus_MoneyFlowStatusId");
            CreateIndex("dbo.MoneyFlow", "Project_ProjectId");
            CreateIndex("dbo.MoneyFlow", "Program_ProgramId");
            CreateIndex("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId");
            AddForeignKey("dbo.MoneyFlow", "Project_ProjectId", "dbo.Project", "ProjectId");
            AddForeignKey("dbo.MoneyFlow", "Program_ProgramId", "dbo.Program", "ProgramId");
            AddForeignKey("dbo.MoneyFlow", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop", "ItineraryStopId");
        }
    }
}
