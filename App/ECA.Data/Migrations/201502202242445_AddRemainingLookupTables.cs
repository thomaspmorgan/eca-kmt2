namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemainingLookupTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActorType",
                c => new
                    {
                        ActorTypeId = c.Int(nullable: false, identity: true),
                        ActorName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ActorTypeId);
            
            CreateTable(
                "dbo.PhoneNumberType",
                c => new
                    {
                        PhoneNumberTypeId = c.Int(nullable: false, identity: true),
                        PhoneNumberTypeName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.PhoneNumberTypeId);
            
            CreateTable(
                "dbo.Method",
                c => new
                    {
                        MethodId = c.Int(nullable: false, identity: true),
                        MethodName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.MethodId);
            
            CreateTable(
                "dbo.ProjectType",
                c => new
                    {
                        ProjectTypeId = c.Int(nullable: false, identity: true),
                        ProjectTypeName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ProjectTypeId);
            
            CreateTable(
                "dbo.ProgramType",
                c => new
                    {
                        ProgramTypeId = c.Int(nullable: false, identity: true),
                        ProgramTypeName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Program_ProgramId = c.Int(),
                    })
                .PrimaryKey(t => t.ProgramTypeId)
                .ForeignKey("dbo.Program", t => t.Program_ProgramId)
                .Index(t => t.Program_ProgramId);
            
            CreateTable(
                "dbo.ItineraryStatus",
                c => new
                    {
                        ItineraryStatusId = c.Int(nullable: false, identity: true),
                        ItineraryStatusName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ItineraryStatusId);
            
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        GenderId = c.Int(nullable: false, identity: true),
                        GenderName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.GenderId);
            
            CreateTable(
                "dbo.NameType",
                c => new
                    {
                        NameTypeId = c.Int(nullable: false, identity: true),
                        NameTypeName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.NameTypeId);
            
            CreateTable(
                "dbo.SocialMediaType",
                c => new
                    {
                        SocialMediaTypeId = c.Int(nullable: false, identity: true),
                        SocialMediaTypeName = c.String(nullable: false, maxLength: 20),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SocialMediaTypeId);


            RenameColumn("dbo.Person", "Gender", "GenderId");
            RenameColumn("dbo.Actor", "ActorType", "ActorTypeId");
            RenameColumn("dbo.ItineraryStop", "ItineraryStatus", "ItineraryStatusId");
            RenameColumn("dbo.PhoneNumber", "PhoneNumberType", "PhoneNumberTypeId");
            RenameColumn("dbo.Project", "ProjectType", "ProjectTypeId");
            RenameColumn("dbo.Transportation", "Method", "MethodId");
            RenameColumn("dbo.Itinerary", "ItineraryStatus", "ItineraryStatusId");
            RenameColumn("dbo.NamePart", "NameType", "NameTypeId");
            RenameColumn("dbo.SocialMedia", "SocialMediaType", "SocialMediaTypeId");
            CreateIndex("dbo.Person", "GenderId");
            CreateIndex("dbo.Actor", "ActorTypeId");
            CreateIndex("dbo.ItineraryStop", "ItineraryStatusId");
            CreateIndex("dbo.PhoneNumber", "PhoneNumberTypeId");
            CreateIndex("dbo.Project", "ProjectTypeId");
            CreateIndex("dbo.Transportation", "MethodId");
            CreateIndex("dbo.Itinerary", "ItineraryStatusId");
            CreateIndex("dbo.NamePart", "NameTypeId");
            CreateIndex("dbo.SocialMedia", "SocialMediaTypeId");
            AddForeignKey("dbo.Actor", "ActorTypeId", "dbo.ActorType", "ActorTypeId", cascadeDelete: true);
            //AddForeignKey("dbo.PhoneNumber", "PhoneNumberTypeId", "dbo.PhoneNumberType", "PhoneNumberTypeId", cascadeDelete: true);
            AddForeignKey("dbo.Transportation", "MethodId", "dbo.Method", "MethodId", cascadeDelete: true);
            //AddForeignKey("dbo.Project", "ProjectTypeId", "dbo.ProjectType", "ProjectTypeId");
            AddForeignKey("dbo.Itinerary", "ItineraryStatusId", "dbo.ItineraryStatus", "ItineraryStatusId", cascadeDelete: true);
            //AddForeignKey("dbo.ItineraryStop", "ItineraryStatusId", "dbo.ItineraryStatus", "ItineraryStatusId", cascadeDelete: true);
            //AddForeignKey("dbo.Person", "GenderId", "dbo.Gender", "GenderId", cascadeDelete: true);
            //AddForeignKey("dbo.NamePart", "NameTypeId", "dbo.NameType", "NameTypeId", cascadeDelete: true);
            AddForeignKey("dbo.SocialMedia", "SocialMediaTypeId", "dbo.SocialMediaType", "SocialMediaTypeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialMedia", "SocialMediaTypeId", "dbo.SocialMediaType");
            DropForeignKey("dbo.NamePart", "NameTypeId", "dbo.NameType");
            DropForeignKey("dbo.Person", "GenderId", "dbo.Gender");
            DropForeignKey("dbo.ItineraryStop", "ItineraryStatusId", "dbo.ItineraryStatus");
            DropForeignKey("dbo.Itinerary", "ItineraryStatusId", "dbo.ItineraryStatus");
            DropForeignKey("dbo.ProgramType", "Program_ProgramId", "dbo.Program");
            DropForeignKey("dbo.Project", "ProjectTypeId", "dbo.ProjectType");
            DropForeignKey("dbo.Transportation", "MethodId", "dbo.Method");
            DropForeignKey("dbo.PhoneNumber", "PhoneNumberTypeId", "dbo.PhoneNumberType");
            DropForeignKey("dbo.Actor", "ActorTypeId", "dbo.ActorType");
            DropIndex("dbo.SocialMedia", new[] { "SocialMediaTypeId" });
            DropIndex("dbo.NamePart", new[] { "NameTypeId" });
            DropIndex("dbo.Itinerary", new[] { "ItineraryStatusId" });
            DropIndex("dbo.ProgramType", new[] { "Program_ProgramId" });
            DropIndex("dbo.Transportation", new[] { "MethodId" });
            DropIndex("dbo.Project", new[] { "ProjectTypeId" });
            DropIndex("dbo.PhoneNumber", new[] { "PhoneNumberTypeId" });
            DropIndex("dbo.ItineraryStop", new[] { "ItineraryStatusId" });
            DropIndex("dbo.Actor", new[] { "ActorTypeId" });
            DropIndex("dbo.Person", new[] { "GenderId" });
            RenameColumn("dbo.SocialMedia", "SocialMediaTypeId", "SocialMediaType");
            RenameColumn("dbo.NamePart", "NameTypeId", "NameType");
            RenameColumn("dbo.Itinerary", "ItineraryStatusId", "ItineraryStatus");
            RenameColumn("dbo.Transportation", "MethodId", "Method");
            RenameColumn("dbo.Project", "ProjectTypeId", "PhoneType");
            RenameColumn("dbo.PhoneNumber", "PhoneNumberTypeId", "PhoneNumberType");
            RenameColumn("dbo.ItineraryStop", "ItineraryStatusId", "ItineraryStatus");
            RenameColumn("dbo.Actor", "ActorTypeId", "ActorType");
            RenameColumn("dbo.Person", "GenderId", "Gender");
            DropTable("dbo.SocialMediaType");
            DropTable("dbo.NameType");
            DropTable("dbo.Gender");
            DropTable("dbo.ItineraryStatus");
            DropTable("dbo.ProgramType");
            DropTable("dbo.ProjectType");
            DropTable("dbo.Method");
            DropTable("dbo.PhoneNumberType");
            DropTable("dbo.ActorType");
        }
    }
}
