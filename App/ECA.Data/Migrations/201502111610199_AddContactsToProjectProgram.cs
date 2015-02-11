namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactsToProjectProgram : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contact", "OrganizationId", "dbo.Organization");
            DropIndex("dbo.Contact", new[] { "OrganizationId" });
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
                "dbo.OrganizationContact",
                c => new
                    {
                        Organization_OrganizationId = c.Int(nullable: false),
                        Contact_ContactId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_OrganizationId, t.Contact_ContactId })
                .ForeignKey("dbo.Organization", t => t.Organization_OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.Contact_ContactId, cascadeDelete: true)
                .Index(t => t.Organization_OrganizationId)
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
            
            DropColumn("dbo.Contact", "OrganizationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contact", "OrganizationId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ContactProgram", "Program_ProgramId", "dbo.Program");
            DropForeignKey("dbo.ContactProgram", "Contact_ContactId", "dbo.Contact");
            DropForeignKey("dbo.OrganizationContact", "Contact_ContactId", "dbo.Contact");
            DropForeignKey("dbo.OrganizationContact", "Organization_OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.ProjectContact", "Contact_ContactId", "dbo.Contact");
            DropForeignKey("dbo.ProjectContact", "Project_ProjectId", "dbo.Project");
            DropIndex("dbo.ContactProgram", new[] { "Program_ProgramId" });
            DropIndex("dbo.ContactProgram", new[] { "Contact_ContactId" });
            DropIndex("dbo.OrganizationContact", new[] { "Contact_ContactId" });
            DropIndex("dbo.OrganizationContact", new[] { "Organization_OrganizationId" });
            DropIndex("dbo.ProjectContact", new[] { "Contact_ContactId" });
            DropIndex("dbo.ProjectContact", new[] { "Project_ProjectId" });
            DropTable("dbo.ContactProgram");
            DropTable("dbo.OrganizationContact");
            DropTable("dbo.ProjectContact");
            CreateIndex("dbo.Contact", "OrganizationId");
            AddForeignKey("dbo.Contact", "OrganizationId", "dbo.Organization", "OrganizationId", cascadeDelete: true);
        }
    }
}
