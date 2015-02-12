namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixProjectStatusIdField : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.Project", name: "Status_ProjectStatusId", newName: "ProjectStatusId");
            //RenameIndex(table: "dbo.Project", name: "IX_Status_ProjectStatusId", newName: "IX_ProjectStatusId");
        }
        
        public override void Down()
        {
            //RenameIndex(table: "dbo.Project", name: "IX_ProjectStatusId", newName: "IX_Status_ProjectStatusId");
            //RenameColumn(table: "dbo.Project", name: "ProjectStatusId", newName: "Status_ProjectStatusId");
        }
    }
}
