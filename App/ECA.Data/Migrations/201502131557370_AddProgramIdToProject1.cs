namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProgramIdToProject1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Project", "ParentProgram_ProgramId", "dbo.Program");
            DropIndex("dbo.Project", new[] { "ParentProgram_ProgramId" });
            RenameColumn(table: "dbo.Project", name: "ParentProgram_ProgramId", newName: "ProgramId");
            AlterColumn("dbo.Project", "ProgramId", c => c.Int(nullable: false));
            CreateIndex("dbo.Project", "ProgramId");
            AddForeignKey("dbo.Project", "ProgramId", "dbo.Program", "ProgramId", cascadeDelete: false);
            //DropColumn("dbo.Project", "ParentProgram_ProgramId");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Project", "ParentProgram_ProgramId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Project", "ProgramId", "dbo.Program");
            DropIndex("dbo.Project", new[] { "ProgramId" });
            AlterColumn("dbo.Project", "ProgramId", c => c.Int());
            RenameColumn(table: "dbo.Project", name: "ProgramId", newName: "ParentProgram_ProgramId");
            CreateIndex("dbo.Project", "ParentProgram_ProgramId");
            AddForeignKey("dbo.Project", "ParentProgram_ProgramId", "dbo.Program", "ProgramId");
        }
    }
}
