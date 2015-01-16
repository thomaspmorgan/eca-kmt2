namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
           
            CreateTable(
                "dbo.Theme",
                c => new
                    {
                        ThemeId = c.Int(nullable: false, identity: true),
                        ThemeName = c.String(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ThemeId);
            
           
            CreateTable(
                "dbo.ProjectTheme",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        ThemeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.ThemeId })
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Theme", t => t.ThemeId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.ThemeId);
            
            CreateTable(
                "dbo.ProgramTheme",
                c => new
                    {
                        ProgramId = c.Int(nullable: false),
                        ThemeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProgramId, t.ThemeId })
                .ForeignKey("dbo.Program", t => t.ProgramId, cascadeDelete: true)
                .ForeignKey("dbo.Theme", t => t.ThemeId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ThemeId);
            
        }
        
        public override void Down()
        {

            DropForeignKey("dbo.ProgramTheme", "ThemeId", "dbo.Theme");
            DropForeignKey("dbo.ProgramTheme", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.ProjectTheme", "ThemeId", "dbo.Theme");
            DropForeignKey("dbo.ProjectTheme", "ProjectId", "dbo.Project");
            DropIndex("dbo.ProgramTheme", new[] { "ThemeId" });
            DropIndex("dbo.ProgramTheme", new[] { "ProgramId" });
            DropIndex("dbo.ProjectTheme", new[] { "ThemeId" });
            DropIndex("dbo.ProjectTheme", new[] { "ProjectId" });
            DropTable("dbo.ProgramTheme");
            DropTable("dbo.ProjectTheme");
         }
    }
}
