namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonMods : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Location", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Person", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Person", "ParticipantOrigination_LocationId", "dbo.Location");
            DropForeignKey("dbo.Person", "PersonId", "dbo.Location");
            DropForeignKey("dbo.Address", "PersonId", "dbo.Person");
            DropForeignKey("dbo.CitizenCountry", "PersonId", "dbo.Person");
            DropForeignKey("dbo.ProfessionEducation", "PersonOfEducation_PersonId", "dbo.Person");
            DropForeignKey("dbo.ProfessionEducation", "PersonOfProfession_PersonId", "dbo.Person");
            DropForeignKey("dbo.EmailAddress", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ImpactPerson", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Participant", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Publication", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Actor", "PersonId", "dbo.Person");
            DropForeignKey("dbo.EventPerson", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ExternalId", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.PersonFamily", "PersonId", "dbo.Person");
            DropForeignKey("dbo.PersonFamily", "RelatedPersonId", "dbo.Person");
            DropForeignKey("dbo.InterestSpecialization", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.LanguageProficiencyPerson", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Membership", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.NamePart", "PersonId", "dbo.Person");
            DropForeignKey("dbo.PhoneNumber", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ProminentCategory", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.SocialMedia", "PersonId", "dbo.Person");
            DropForeignKey("dbo.SpecialStatus", "Person_PersonId", "dbo.Person");
            DropIndex("dbo.Location", new[] { "Person_PersonId" });
            DropIndex("dbo.Person", new[] { "PersonId" });
            DropIndex("dbo.Person", new[] { "Person_PersonId" });
            DropIndex("dbo.Person", new[] { "ParticipantOrigination_LocationId" });
            DropPrimaryKey("dbo.Person");
            CreateTable(
                "dbo.CitizenCountry",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.LocationId })
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.PersonFamily",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        RelatedPersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.RelatedPersonId })
                .ForeignKey("dbo.Person", t => t.PersonId)
                .ForeignKey("dbo.Person", t => t.RelatedPersonId)
                .Index(t => t.PersonId)
                .Index(t => t.RelatedPersonId);
            
            AddColumn("dbo.Person", "MedicalConditions", c => c.String());
            AddColumn("dbo.Person", "Awards", c => c.String());
            AlterColumn("dbo.Person", "PersonId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Person", "PersonId");
            AddForeignKey("dbo.Address", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ProfessionEducation", "PersonOfEducation_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ProfessionEducation", "PersonOfProfession_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.EmailAddress", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ImpactPerson", "PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.Participant", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.Publication", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.Actor", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.EventPerson", "Person_PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.ExternalId", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.InterestSpecialization", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.LanguageProficiencyPerson", "Person_PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.Membership", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.NamePart", "PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.PhoneNumber", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ProminentCategory", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.SocialMedia", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.SpecialStatus", "Person_PersonId", "dbo.Person", "PersonId");
            DropColumn("dbo.Location", "Person_PersonId");
            DropColumn("dbo.Person", "Person_PersonId");
            DropColumn("dbo.Person", "ParticipantOrigination_LocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "ParticipantOrigination_LocationId", c => c.Int());
            AddColumn("dbo.Person", "Person_PersonId", c => c.Int());
            AddColumn("dbo.Location", "Person_PersonId", c => c.Int());
            DropForeignKey("dbo.SpecialStatus", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.SocialMedia", "PersonId", "dbo.Person");
            DropForeignKey("dbo.ProminentCategory", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.PhoneNumber", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.NamePart", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Membership", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.LanguageProficiencyPerson", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.InterestSpecialization", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ExternalId", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.EventPerson", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Actor", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Publication", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Participant", "PersonId", "dbo.Person");
            DropForeignKey("dbo.ImpactPerson", "PersonId", "dbo.Person");
            DropForeignKey("dbo.EmailAddress", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ProfessionEducation", "PersonOfProfession_PersonId", "dbo.Person");
            DropForeignKey("dbo.ProfessionEducation", "PersonOfEducation_PersonId", "dbo.Person");
            DropForeignKey("dbo.Address", "PersonId", "dbo.Person");
            DropForeignKey("dbo.PersonFamily", "RelatedPersonId", "dbo.Person");
            DropForeignKey("dbo.PersonFamily", "PersonId", "dbo.Person");
            DropForeignKey("dbo.CitizenCountry", "LocationId", "dbo.Location");
            DropForeignKey("dbo.CitizenCountry", "PersonId", "dbo.Person");
            DropIndex("dbo.PersonFamily", new[] { "RelatedPersonId" });
            DropIndex("dbo.PersonFamily", new[] { "PersonId" });
            DropIndex("dbo.CitizenCountry", new[] { "LocationId" });
            DropIndex("dbo.CitizenCountry", new[] { "PersonId" });
            DropPrimaryKey("dbo.Person");
            AlterColumn("dbo.Person", "PersonId", c => c.Int(nullable: false));
            DropColumn("dbo.Person", "Awards");
            DropColumn("dbo.Person", "MedicalConditions");
            DropTable("dbo.PersonFamily");
            DropTable("dbo.CitizenCountry");
            AddPrimaryKey("dbo.Person", "PersonId");
            CreateIndex("dbo.Person", "ParticipantOrigination_LocationId");
            CreateIndex("dbo.Person", "Person_PersonId");
            CreateIndex("dbo.Person", "PersonId");
            CreateIndex("dbo.Location", "Person_PersonId");
            AddForeignKey("dbo.SpecialStatus", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.SocialMedia", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ProminentCategory", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.PhoneNumber", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.NamePart", "PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.Membership", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.LanguageProficiencyPerson", "Person_PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.InterestSpecialization", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.PersonFamily", "RelatedPersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.PersonFamily", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ExternalId", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.EventPerson", "Person_PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.Actor", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.Publication", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.Participant", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ImpactPerson", "PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.EmailAddress", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ProfessionEducation", "PersonOfProfession_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ProfessionEducation", "PersonOfEducation_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.CitizenCountry", "PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.Address", "PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.Person", "PersonId", "dbo.Location", "LocationId");
            AddForeignKey("dbo.Person", "ParticipantOrigination_LocationId", "dbo.Location", "LocationId");
            AddForeignKey("dbo.Person", "Person_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.Location", "Person_PersonId", "dbo.Person", "PersonId");
        }
    }
}
