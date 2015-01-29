namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLocationAndGoals : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Location", "ProjectOfLocation_ProjectId", "dbo.Project");
            DropForeignKey("dbo.Location", "ProjectOfRegion_ProjectId", "dbo.Project");
            DropForeignKey("dbo.Location", "ProjectOfTarget_ProjectId", "dbo.Project");
            DropForeignKey("dbo.Location", "ProgramOfLocation_ProgramId", "dbo.Program");
            DropForeignKey("dbo.Location", "ProgramOfRegion_ProgramId", "dbo.Program");
            DropForeignKey("dbo.Location", "ProgramOfTarget_ProgramId", "dbo.Program");
            DropIndex("dbo.Location", new[] { "ProjectOfLocation_ProjectId" });
            DropIndex("dbo.Location", new[] { "ProjectOfRegion_ProjectId" });
            DropIndex("dbo.Location", new[] { "ProjectOfTarget_ProjectId" });
            DropIndex("dbo.Location", new[] { "ProgramOfLocation_ProgramId" });
            DropIndex("dbo.Location", new[] { "ProgramOfRegion_ProgramId" });
            DropIndex("dbo.Location", new[] { "ProgramOfTarget_ProgramId" });
            DropPrimaryKey("dbo.Material");
            CreateTable(
                "dbo.Goal",
                c => new
                    {
                        GoalId = c.Int(nullable: false, identity: true),
                        GoalName = c.String(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.GoalId);
            
            CreateTable(
                "dbo.ProgramGoal",
                c => new
                    {
                        ProgramId = c.Int(nullable: false),
                        GoalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new {  t.ProgramId, t.GoalId})
                .ForeignKey("dbo.Goal", t => t.GoalId, cascadeDelete: true)
                .ForeignKey("dbo.Program", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.GoalId)
                .Index(t => t.ProgramId);
            
            CreateTable(
                "dbo.ProjectGoal",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        GoalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.GoalId })
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Goal", t => t.GoalId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.GoalId);
            
            CreateTable(
                "dbo.ProjectLocation",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.LocationId })
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.ProjectRegion",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.LocationId })
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.ProjectTarget",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.LocationId })
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.ProgramLocation",
                c => new
                    {
                        ProgramId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProgramId, t.LocationId })
                .ForeignKey("dbo.Program", t => t.ProgramId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.ProgramRegion",
                c => new
                    {
                        ProgramId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProgramId, t.LocationId })
                .ForeignKey("dbo.Program", t => t.ProgramId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.ProgramTarget",
                c => new
                    {
                        ProgramId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProgramId, t.LocationId })
                .ForeignKey("dbo.Program", t => t.ProgramId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.LocationId);

            DropColumn("dbo.Material", "Id");
            AddColumn("dbo.Material", "MaterialId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Material", "MaterialId");
            DropColumn("dbo.Location", "ProjectOfLocation_ProjectId");
            DropColumn("dbo.Location", "ProjectOfRegion_ProjectId");
            DropColumn("dbo.Location", "ProjectOfTarget_ProjectId");
            DropColumn("dbo.Location", "ProgramOfLocation_ProgramId");
            DropColumn("dbo.Location", "ProgramOfRegion_ProgramId");
            DropColumn("dbo.Location", "ProgramOfTarget_ProgramId");

        }
        
        public override void Down()
        {
            AddColumn("dbo.Material", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Location", "ProgramOfTarget_ProgramId", c => c.Int());
            AddColumn("dbo.Location", "ProgramOfRegion_ProgramId", c => c.Int());
            AddColumn("dbo.Location", "ProgramOfLocation_ProgramId", c => c.Int());
            AddColumn("dbo.Location", "ProjectOfTarget_ProjectId", c => c.Int());
            AddColumn("dbo.Location", "ProjectOfRegion_ProjectId", c => c.Int());
            AddColumn("dbo.Location", "ProjectOfLocation_ProjectId", c => c.Int());
            DropForeignKey("dbo.ProgramRegion", "LocationId", "dbo.Location");
            DropForeignKey("dbo.ProgramRegion", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.ProgramTarget", "LocationId", "dbo.Location");
            DropForeignKey("dbo.ProgramTarget", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.ProgramLocation", "LocationId", "dbo.Location");
            DropForeignKey("dbo.ProgramLocation", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.ProjectRegion", "LocationId", "dbo.Location");
            DropForeignKey("dbo.ProjectRegion", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProjectTarget", "LocationId", "dbo.Location");
            DropForeignKey("dbo.ProjectTarget", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProjectLocation", "LocationId", "dbo.Location");
            DropForeignKey("dbo.ProjectLocation", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProjectGoal", "GoalId", "dbo.Goal");
            DropForeignKey("dbo.ProjectGoal", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProgramGoal", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.ProgramGoal", "GoalId", "dbo.Goal");
            DropIndex("dbo.ProgramRegion", new[] { "LocationId" });
            DropIndex("dbo.ProgramRegion", new[] { "ProgramId" });
            DropIndex("dbo.ProgramTarget", new[] { "LocationId" });
            DropIndex("dbo.ProgramTarget", new[] { "ProgramId" });
            DropIndex("dbo.ProgramLocation", new[] { "LocationId" });
            DropIndex("dbo.ProgramLocation", new[] { "ProgramId" });
            DropIndex("dbo.ProjectRegion", new[] { "LocationId" });
            DropIndex("dbo.ProjectRegion", new[] { "ProjectId" });
            DropIndex("dbo.ProjectTarget", new[] { "LocationId" });
            DropIndex("dbo.ProjectTarget", new[] { "ProjectId" });
            DropIndex("dbo.ProjectLocation", new[] { "LocationId" });
            DropIndex("dbo.ProjectLocation", new[] { "ProjectId" });
            DropIndex("dbo.ProjectGoal", new[] { "GoalId" });
            DropIndex("dbo.ProjectGoal", new[] { "ProjectId" });
            DropIndex("dbo.ProgramGoal", new[] { "ProgramId" });
            DropIndex("dbo.ProgramGoal", new[] { "GoalId" });
            DropPrimaryKey("dbo.Material");
            DropColumn("dbo.Material", "MaterialId");
            DropTable("dbo.ProgramRegion");
            DropTable("dbo.ProgramTarget");
            DropTable("dbo.ProgramLocation");
            DropTable("dbo.ProjectRegion");
            DropTable("dbo.ProjectTarget");
            DropTable("dbo.ProjectLocation");
            DropTable("dbo.ProjectGoal");
            DropTable("dbo.ProgramGoal");
            DropTable("dbo.Goal");
            AddPrimaryKey("dbo.Material", "Id");
            CreateIndex("dbo.Location", "ProgramOfTarget_ProgramId");
            CreateIndex("dbo.Location", "ProgramOfRegion_ProgramId");
            CreateIndex("dbo.Location", "ProgramOfLocation_ProgramId");
            CreateIndex("dbo.Location", "ProjectOfTarget_ProjectId");
            CreateIndex("dbo.Location", "ProjectOfRegion_ProjectId");
            CreateIndex("dbo.Location", "ProjectOfLocation_ProjectId");
            AddForeignKey("dbo.Location", "ProgramOfTarget_ProgramId", "dbo.Program", "ProgramId");
            AddForeignKey("dbo.Location", "ProgramOfRegion_ProgramId", "dbo.Program", "ProgramId");
            AddForeignKey("dbo.Location", "ProgramOfLocation_ProgramId", "dbo.Program", "ProgramId");
            AddForeignKey("dbo.Location", "ProjectOfTarget_ProjectId", "dbo.Project", "ProjectId");
            AddForeignKey("dbo.Location", "ProjectOfRegion_ProjectId", "dbo.Project", "ProjectId");
            AddForeignKey("dbo.Location", "ProjectOfLocation_ProjectId", "dbo.Project", "ProjectId");
        }
    }
}
