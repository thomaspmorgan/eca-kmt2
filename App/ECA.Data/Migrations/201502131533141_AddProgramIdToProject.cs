namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProgramIdToProject : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Project", "ParentProgram_ProgramId", "dbo.Program");
            //DropIndex("dbo.Project", new[] { "ParentProgram_ProgramId" });
            //AddColumn("dbo.Project", "ParentProgram_ProgramId1", c => c.Int());
            //AlterColumn("dbo.Project", "ParentProgram_ProgramId", c => c.Int(nullable: false));
            //CreateIndex("dbo.Project", "ParentProgram_ProgramId1");
            //AddForeignKey("dbo.Project", "ParentProgram_ProgramId1", "dbo.Program", "ProgramId");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Project", "ParentProgram_ProgramId1", "dbo.Program");
            //DropIndex("dbo.Project", new[] { "ParentProgram_ProgramId1" });
            //AlterColumn("dbo.Project", "ParentProgram_ProgramId", c => c.Int());
            //DropColumn("dbo.Project", "ParentProgram_ProgramId1");
            //CreateIndex("dbo.Project", "ParentProgram_ProgramId");
            //AddForeignKey("dbo.Project", "ParentProgram_ProgramId", "dbo.Program", "ProgramId");
        }
    }
}
