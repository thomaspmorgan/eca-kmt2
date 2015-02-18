namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParticipantImpact : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Person", "ImpactId", "dbo.Impact");
            DropIndex("dbo.Person", new[] { "ImpactId" });
            CreateTable(
                "dbo.ImpactPerson",
                c => new
                    {
                        ImpactId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ImpactId, t.PersonId })
                .ForeignKey("dbo.Impact", t => t.ImpactId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.ImpactId)
                .Index(t => t.PersonId);
            
            DropColumn("dbo.Person", "ImpactId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "ImpactId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ImpactPerson", "PersonId", "dbo.Person");
            DropForeignKey("dbo.ImpactPerson", "ImpactId", "dbo.Impact");
            DropIndex("dbo.ImpactPerson", new[] { "PersonId" });
            DropIndex("dbo.ImpactPerson", new[] { "ImpactId" });
            DropTable("dbo.ImpactPerson");
            CreateIndex("dbo.Person", "ImpactId");
            AddForeignKey("dbo.Person", "ImpactId", "dbo.Impact", "ImpactId", cascadeDelete: true);
        }
    }
}
