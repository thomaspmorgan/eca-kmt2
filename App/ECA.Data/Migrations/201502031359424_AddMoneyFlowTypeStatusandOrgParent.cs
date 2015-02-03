namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoneyFlowTypeStatusandOrgParent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoneyFlowStatus",
                c => new
                    {
                        MoneyFlowStatusId = c.Int(nullable: false, identity: true),
                        MoneyFlowStatusName = c.String(nullable: false, maxLength: 80),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.MoneyFlowStatusId);
            
            CreateTable(
                "dbo.MoneyFlowType",
                c => new
                    {
                        MoneyFlowTypeId = c.Int(nullable: false, identity: true),
                        MoneyFlowTypeName = c.String(nullable: false, maxLength: 80),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.MoneyFlowTypeId);
            
            AddColumn("dbo.MoneyFlow", "MoneyFlowStatus_MoneyFlowStatusId", c => c.Int(nullable: false));
            AddColumn("dbo.MoneyFlow", "MoneyFlowType_MoneyFlowTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Organization", "ParentOrganization_OrganizationId", c => c.Int(nullable: true));
            CreateIndex("dbo.MoneyFlow", "MoneyFlowStatus_MoneyFlowStatusId");
            CreateIndex("dbo.MoneyFlow", "MoneyFlowType_MoneyFlowTypeId");
            CreateIndex("dbo.Organization", "ParentOrganization_OrganizationId");
            AddForeignKey("dbo.Organization", "ParentOrganization_OrganizationId", "dbo.Organization", "OrganizationId");
            AddForeignKey("dbo.MoneyFlow", "MoneyFlowStatus_MoneyFlowStatusId", "dbo.MoneyFlowStatus", "MoneyFlowStatusId");
            AddForeignKey("dbo.MoneyFlow", "MoneyFlowType_MoneyFlowTypeId", "dbo.MoneyFlowType", "MoneyFlowTypeId");
            DropColumn("dbo.MoneyFlow", "MoneyFlowType");
            DropColumn("dbo.MoneyFlow", "MoneyFlowStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MoneyFlow", "MoneyFlowStatus", c => c.Int(nullable: true));
            AddColumn("dbo.MoneyFlow", "MoneyFlowType", c => c.Int(nullable: true));
            DropForeignKey("dbo.MoneyFlow", "MoneyFlowType_MoneyFlowTypeId", "dbo.MoneyFlowType");
            DropForeignKey("dbo.MoneyFlow", "MoneyFlowStatus_MoneyFlowStatusId", "dbo.MoneyFlowStatus");
            DropForeignKey("dbo.Organization", "ParentOrganization_OrganizationId", "dbo.Organization");
            DropIndex("dbo.Organization", new[] { "ParentOrganization_OrganizationId" });
            DropIndex("dbo.MoneyFlow", new[] { "MoneyFlowType_MoneyFlowTypeId" });
            DropIndex("dbo.MoneyFlow", new[] { "MoneyFlowStatus_MoneyFlowStatusId" });
            DropColumn("dbo.Organization", "ParentOrganization_OrganizationId");
            DropColumn("dbo.MoneyFlow", "MoneyFlowType_MoneyFlowTypeId");
            DropColumn("dbo.MoneyFlow", "MoneyFlowStatus_MoneyFlowStatusId");
            DropTable("dbo.MoneyFlowType");
            DropTable("dbo.MoneyFlowStatus");
        }
    }
}
