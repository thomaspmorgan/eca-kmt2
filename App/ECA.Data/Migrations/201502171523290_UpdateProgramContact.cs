namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProgramContact : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ProjectContact");
            DropTable("dbo.ProgramContact");

            CreateTable(
                "dbo.ProjectContact",
                c => new
                {
                    ProjectId = c.Int(nullable: false),
                    ContactId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.ProjectId, t.ContactId })
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.ContactId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.ContactId);

            CreateTable(
                "dbo.ProgramContact",
                c => new
                {
                    ProgramId = c.Int(nullable: false),
                    ContactId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.ProgramId, t.ContactId })
                .ForeignKey("dbo.Contact", t => t.ContactId, cascadeDelete: true)
                .ForeignKey("dbo.Program", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ContactId)
                .Index(t => t.ProgramId);

            AddColumn("dbo.MoneyFlow", "Description", c => c.String(maxLength: 255));

        }
        
        public override void Down()
        {


            DropColumn("dbo.MoneyFlow", "Description");

            DropTable("dbo.ProjectContact");
            DropTable("dbo.ProgramContact");

            CreateTable(
                "dbo.ProjectContact",
                c => new
                {
                    Project_ProjectId = c.Int(nullable: false),
                    Contact_ContactId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Project_ProjectId, t.Contact_ContactId })
                .ForeignKey("dbo.Project", t => t.Project_ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.Contact_ContactId, cascadeDelete: true)
                .Index(t => t.Project_ProjectId)
                .Index(t => t.Contact_ContactId);


            CreateTable(
                "dbo.ProgramContact",
                c => new
                {
                    Contact_ContactId = c.Int(nullable: false),
                    Program_ProgramId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Program_ProgramId, t.Contact_ContactId })
                .ForeignKey("dbo.Contact", t => t.Contact_ContactId, cascadeDelete: true)
                .ForeignKey("dbo.Program", t => t.Program_ProgramId, cascadeDelete: true)
                .Index(t => t.Contact_ContactId)
                .Index(t => t.Program_ProgramId);
        }
    }
}
