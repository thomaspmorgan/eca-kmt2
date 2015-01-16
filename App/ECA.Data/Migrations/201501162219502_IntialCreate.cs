namespace ECA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accommodation",
                c => new
                    {
                        AccommodationId = c.Int(nullable: false, identity: true),
                        CheckIn = c.DateTimeOffset(nullable: false, precision: 7),
                        CheckOut = c.DateTimeOffset(nullable: false, precision: 7),
                        RecordLocator = c.String(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Host_OrganizationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccommodationId)
                .ForeignKey("dbo.Organization", t => t.Host_OrganizationId, cascadeDelete: true)
                .Index(t => t.Host_OrganizationId);
            
            CreateTable(
                "dbo.MoneyFlow",
                c => new
                    {
                        MoneyFlowId = c.Int(nullable: false, identity: true),
                        MoneyFlowType = c.Int(nullable: false),
                        Value = c.Single(nullable: false),
                        MoneyFlowStatus = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        ItineraryStopId = c.Int(nullable: false),
                        AccommodationId = c.Int(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Parent_MoneyFlowId = c.Int(),
                        Recipient_OrganizationId = c.Int(nullable: false),
                        Source_OrganizationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MoneyFlowId)
                .ForeignKey("dbo.Accommodation", t => t.AccommodationId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Program", t => t.ProgramId, cascadeDelete: true)
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStopId, cascadeDelete: true)
                .ForeignKey("dbo.MoneyFlow", t => t.Parent_MoneyFlowId)
                .ForeignKey("dbo.Organization", t => t.Recipient_OrganizationId)
                .ForeignKey("dbo.Organization", t => t.Source_OrganizationId)
                .Index(t => t.ProgramId)
                .Index(t => t.ProjectId)
                .Index(t => t.PersonId)
                .Index(t => t.ItineraryStopId)
                .Index(t => t.AccommodationId)
                .Index(t => t.Parent_MoneyFlowId)
                .Index(t => t.Recipient_OrganizationId)
                .Index(t => t.Source_OrganizationId);
            
            CreateTable(
                "dbo.ItineraryStop",
                c => new
                    {
                        ItineraryStopId = c.Int(nullable: false, identity: true),
                        ItineraryStatus = c.Int(nullable: false),
                        DateArrive = c.DateTimeOffset(nullable: false, precision: 7),
                        DateLeave = c.DateTimeOffset(nullable: false, precision: 7),
                        ItineraryId = c.Int(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Destination_LocationId = c.Int(),
                        Origin_LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.ItineraryStopId)
                .ForeignKey("dbo.Location", t => t.Destination_LocationId)
                .ForeignKey("dbo.Itinerary", t => t.ItineraryId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.Origin_LocationId)
                .Index(t => t.ItineraryId)
                .Index(t => t.Destination_LocationId)
                .Index(t => t.Origin_LocationId);
            
            CreateTable(
                "dbo.Actor",
                c => new
                    {
                        ActorId = c.Int(nullable: false, identity: true),
                        ActorType = c.Int(nullable: false),
                        ActorName = c.String(nullable: false),
                        Status = c.String(),
                        Action = c.String(),
                        PersonId = c.Int(),
                        OrganizationId = c.Int(),
                        EventId = c.Int(),
                        ItineraryStopId = c.Int(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ActorId)
                .ForeignKey("dbo.Event", t => t.EventId)
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStopId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .ForeignKey("dbo.Person", t => t.PersonId)
                .Index(t => t.PersonId)
                .Index(t => t.OrganizationId)
                .Index(t => t.EventId)
                .Index(t => t.ItineraryStopId);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        EventTypeId = c.Int(nullable: false),
                        EventDate = c.DateTimeOffset(nullable: false, precision: 7),
                        LocationId = c.Int(nullable: false),
                        Description = c.String(),
                        TargetAudience = c.String(),
                        EsimatedAudienceSize = c.Int(nullable: false),
                        EsimatedNumberOfAlumni = c.Int(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.EventType", t => t.EventTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.EventTypeId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Artifact",
                c => new
                    {
                        ArtifactId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Path = c.String(),
                        ArtifactTypeId = c.Int(nullable: false),
                        Data = c.Binary(),
                        EventId = c.Int(),
                        ProjectId = c.Int(),
                        ProgramId = c.Int(),
                        PublicationId = c.Int(),
                        ItineraryStopId = c.Int(),
                        ImpactId = c.Int(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ArtifactId)
                .ForeignKey("dbo.ArtifactType", t => t.ArtifactTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Event", t => t.EventId)
                .ForeignKey("dbo.Impact", t => t.ImpactId)
                .ForeignKey("dbo.Program", t => t.ProgramId)
                .ForeignKey("dbo.Publication", t => t.PublicationId)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStopId)
                .Index(t => t.ArtifactTypeId)
                .Index(t => t.EventId)
                .Index(t => t.ProjectId)
                .Index(t => t.ProgramId)
                .Index(t => t.PublicationId)
                .Index(t => t.ItineraryStopId)
                .Index(t => t.ImpactId);
            
            CreateTable(
                "dbo.ArtifactType",
                c => new
                    {
                        ArtifactTypeId = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ArtifactTypeId);
            
            CreateTable(
                "dbo.Impact",
                c => new
                    {
                        ImpactId = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(),
                        ProjectId = c.Int(),
                        Description = c.String(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ImpactId)
                .ForeignKey("dbo.Program", t => t.ProgramId)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .Index(t => t.ProgramId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ImpactType",
                c => new
                    {
                        ImpactTypeId = c.Int(nullable: false, identity: true),
                        Impact = c.String(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Impact_ImpactId = c.Int(),
                    })
                .PrimaryKey(t => t.ImpactTypeId)
                .ForeignKey("dbo.Impact", t => t.Impact_ImpactId)
                .Index(t => t.Impact_ImpactId);
            
            CreateTable(
                "dbo.Program",
                c => new
                    {
                        ProgramId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        ParentProgram_ProgramId = c.Int(),
                        Owner_OrganizationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProgramId)
                .ForeignKey("dbo.Program", t => t.ParentProgram_ProgramId)
                .ForeignKey("dbo.Organization", t => t.Owner_OrganizationId)
                .Index(t => t.ParentProgram_ProgramId)
                .Index(t => t.Owner_OrganizationId);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        LocationTypeId = c.Int(nullable: false),
                        Latitude = c.Single(nullable: false),
                        Longitude = c.Single(nullable: false),
                        Street1 = c.String(),
                        Street2 = c.String(),
                        Street3 = c.String(),
                        City = c.String(),
                        Division = c.String(),
                        PostalCode = c.String(),
                        CountryName = c.String(),
                        CountryIso = c.String(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Person_PersonId = c.Int(),
                        ProjectOfLocation_ProjectId = c.Int(),
                        ProjectOfRegion_ProjectId = c.Int(),
                        ProjectOfTarget_ProjectId = c.Int(),
                        Region_LocationId = c.Int(),
                        ProgramOfLocation_ProgramId = c.Int(),
                        ProgramOfRegion_ProgramId = c.Int(),
                        ProgramOfTarget_ProgramId = c.Int(),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .ForeignKey("dbo.LocationType", t => t.LocationTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Project", t => t.ProjectOfLocation_ProjectId)
                .ForeignKey("dbo.Project", t => t.ProjectOfRegion_ProjectId)
                .ForeignKey("dbo.Project", t => t.ProjectOfTarget_ProjectId)
                .ForeignKey("dbo.Location", t => t.Region_LocationId)
                .ForeignKey("dbo.Program", t => t.ProgramOfLocation_ProgramId)
                .ForeignKey("dbo.Program", t => t.ProgramOfRegion_ProgramId)
                .ForeignKey("dbo.Program", t => t.ProgramOfTarget_ProgramId)
                .Index(t => t.LocationTypeId)
                .Index(t => t.Person_PersonId)
                .Index(t => t.ProjectOfLocation_ProjectId)
                .Index(t => t.ProjectOfRegion_ProjectId)
                .Index(t => t.ProjectOfTarget_ProjectId)
                .Index(t => t.Region_LocationId)
                .Index(t => t.ProgramOfLocation_ProgramId)
                .Index(t => t.ProgramOfRegion_ProgramId)
                .Index(t => t.ProgramOfTarget_ProgramId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        DateOfBirth = c.DateTimeOffset(nullable: false, precision: 7),
                        Ethnicity = c.String(),
                        PermissionToContact = c.Boolean(nullable: false),
                        EvaluationRetention = c.String(),
                        ImpactId = c.Int(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Person_PersonId = c.Int(),
                        ParticipantOrigination_LocationId = c.Int(),
                        Location_LocationId = c.Int(),
                        ItineraryStop_ItineraryStopId = c.Int(),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .ForeignKey("dbo.Impact", t => t.ImpactId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.ParticipantOrigination_LocationId)
                .ForeignKey("dbo.Location", t => t.PersonId)
                .ForeignKey("dbo.Location", t => t.Location_LocationId)
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStop_ItineraryStopId)
                .Index(t => t.PersonId)
                .Index(t => t.ImpactId)
                .Index(t => t.Person_PersonId)
                .Index(t => t.ParticipantOrigination_LocationId)
                .Index(t => t.Location_LocationId)
                .Index(t => t.ItineraryStop_ItineraryStopId);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        AddressTypeId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        DisplayName = c.String(nullable: false),
                        PersonId = c.Int(),
                        OrganizationId = c.Int(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.AddressType", t => t.AddressTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .ForeignKey("dbo.Person", t => t.PersonId)
                .Index(t => t.AddressTypeId)
                .Index(t => t.LocationId)
                .Index(t => t.PersonId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.AddressType",
                c => new
                    {
                        AddressTypeId = c.Int(nullable: false, identity: true),
                        AddressName = c.String(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.AddressTypeId);
            
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        OrganizationId = c.Int(nullable: false, identity: true),
                        OrganizationTypeId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        Status = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Website = c.String(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.OrganizationId)
                .ForeignKey("dbo.OrganizationType", t => t.OrganizationTypeId, cascadeDelete: true)
                .Index(t => t.OrganizationTypeId);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Position = c.String(),
                        OrganizationId = c.Int(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ContactId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.EmailAddress",
                c => new
                    {
                        EmailAddressId = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Contact_ContactId = c.Int(),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.EmailAddressId)
                .ForeignKey("dbo.Contact", t => t.Contact_ContactId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .Index(t => t.Contact_ContactId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.PhoneNumber",
                c => new
                    {
                        PhoneNumberId = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        PhoneNumberType = c.Int(nullable: false),
                        Contact_ContactId = c.Int(),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.PhoneNumberId)
                .ForeignKey("dbo.Contact", t => t.Contact_ContactId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .Index(t => t.Contact_ContactId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.OrganizationType",
                c => new
                    {
                        OrganizationTypeId = c.Int(nullable: false, identity: true),
                        OrganizationTypeName = c.String(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.OrganizationTypeId);
            
            CreateTable(
                "dbo.SocialMedia",
                c => new
                    {
                        SocialMediaId = c.Int(nullable: false, identity: true),
                        SocialMediaType = c.Int(nullable: false),
                        SocialMediaValue = c.String(),
                        OrganizationId = c.Int(),
                        PersonId = c.Int(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SocialMediaId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .ForeignKey("dbo.Person", t => t.PersonId)
                .Index(t => t.OrganizationId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.ProfessionEducation",
                c => new
                    {
                        ProfessionEducationId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Role = c.String(),
                        OrganizationId = c.Int(nullable: false),
                        DateFrom = c.DateTimeOffset(nullable: false, precision: 7),
                        DateTo = c.DateTimeOffset(precision: 7),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        PersonOfEducation_PersonId = c.Int(),
                        PersonOfProfession_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.ProfessionEducationId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonOfEducation_PersonId)
                .ForeignKey("dbo.Person", t => t.PersonOfProfession_PersonId)
                .Index(t => t.OrganizationId)
                .Index(t => t.PersonOfEducation_PersonId)
                .Index(t => t.PersonOfProfession_PersonId);
            
            CreateTable(
                "dbo.ExternalId",
                c => new
                    {
                        ExternalIdId = c.Int(nullable: false, identity: true),
                        ExternalIdValue = c.String(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.ExternalIdId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.InterestSpecialization",
                c => new
                    {
                        InterestSpecializationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.InterestSpecializationId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.LanguageProficiency",
                c => new
                    {
                        LanguageProficiencyId = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.LanguageProficiencyId);
            
            CreateTable(
                "dbo.Membership",
                c => new
                    {
                        MembershipId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.MembershipId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.NamePart",
                c => new
                    {
                        NamePartId = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        NameType = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NamePartId)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.ParticipantType",
                c => new
                    {
                        ParticipantTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.ParticipantTypeId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.ProminentCategory",
                c => new
                    {
                        ProminentCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.ProminentCategoryId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.Publication",
                c => new
                    {
                        PublicationId = c.Int(nullable: false, identity: true),
                        PublicationName = c.String(nullable: false),
                        Work = c.String(nullable: false),
                        PublicationDate = c.DateTimeOffset(nullable: false, precision: 7),
                        PersonId = c.Int(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.PublicationId)
                .ForeignKey("dbo.Person", t => t.PersonId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.SpecialStatus",
                c => new
                    {
                        SpecialStatusId = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.SpecialStatusId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.LocationType",
                c => new
                    {
                        LocationTypeId = c.Int(nullable: false, identity: true),
                        LocationTypeName = c.String(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.LocationTypeId);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ProjectType = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        FocusArea = c.String(),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        Language = c.String(),
                        AudienceReach = c.Int(nullable: false),
                        EventId = c.Int(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        NominationSource_OrganizationId = c.Int(),
                        ParentProgram_ProgramId = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Event", t => t.EventId)
                .ForeignKey("dbo.Organization", t => t.NominationSource_OrganizationId)
                .ForeignKey("dbo.Program", t => t.ParentProgram_ProgramId)
                .Index(t => t.EventId)
                .Index(t => t.NominationSource_OrganizationId)
                .Index(t => t.ParentProgram_ProgramId);
            
            CreateTable(
                "dbo.ParticipantStatus",
                c => new
                    {
                        ParticipantStatusId = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Person_PersonId = c.Int(),
                        Project_ProjectId = c.Int(),
                        Itinerary_ItineraryId = c.Int(),
                        ItineraryStop_ItineraryStopId = c.Int(),
                    })
                .PrimaryKey(t => t.ParticipantStatusId)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .ForeignKey("dbo.Project", t => t.Project_ProjectId)
                .ForeignKey("dbo.Itinerary", t => t.Itinerary_ItineraryId)
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStop_ItineraryStopId)
                .Index(t => t.Person_PersonId)
                .Index(t => t.Project_ProjectId)
                .Index(t => t.Itinerary_ItineraryId)
                .Index(t => t.ItineraryStop_ItineraryStopId);
            
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
                "dbo.EventType",
                c => new
                    {
                        EventTypeId = c.Int(nullable: false, identity: true),
                        EventTypeName = c.String(nullable: false),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.EventTypeId);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ItineraryStop_ItineraryStopId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStop_ItineraryStopId)
                .Index(t => t.ItineraryStop_ItineraryStopId);
            
            CreateTable(
                "dbo.Itinerary",
                c => new
                    {
                        ItineraryId = c.Int(nullable: false, identity: true),
                        ItineraryStatus = c.Int(nullable: false),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ItineraryId);
            
            CreateTable(
                "dbo.Material",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ItineraryStop_ItineraryStopId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStop_ItineraryStopId)
                .Index(t => t.ItineraryStop_ItineraryStopId);
            
            CreateTable(
                "dbo.Transportation",
                c => new
                    {
                        TransportationId = c.Int(nullable: false, identity: true),
                        Method = c.Int(nullable: false),
                        CarriageId = c.String(),
                        RecordLocator = c.String(),
                        ItineraryStopId = c.Int(),
                        History_CreatedBy = c.Int(nullable: false),
                        History_CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        History_RevisedBy = c.Int(nullable: false),
                        History_RevisedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Carrier_OrganizationId = c.Int(),
                    })
                .PrimaryKey(t => t.TransportationId)
                .ForeignKey("dbo.Organization", t => t.Carrier_OrganizationId)
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStopId)
                .Index(t => t.ItineraryStopId)
                .Index(t => t.Carrier_OrganizationId);
            
            CreateTable(
                "dbo.ItineraryStopAccommodation",
                c => new
                    {
                        ItineraryStop_ItineraryStopId = c.Int(nullable: false),
                        Accommodation_AccommodationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItineraryStop_ItineraryStopId, t.Accommodation_AccommodationId })
                .ForeignKey("dbo.ItineraryStop", t => t.ItineraryStop_ItineraryStopId, cascadeDelete: true)
                .ForeignKey("dbo.Accommodation", t => t.Accommodation_AccommodationId, cascadeDelete: true)
                .Index(t => t.ItineraryStop_ItineraryStopId)
                .Index(t => t.Accommodation_AccommodationId);
            
            CreateTable(
                "dbo.PersonEvent",
                c => new
                    {
                        Person_PersonId = c.Int(nullable: false),
                        Event_EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_PersonId, t.Event_EventId })
                .ForeignKey("dbo.Person", t => t.Person_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Event", t => t.Event_EventId, cascadeDelete: true)
                .Index(t => t.Person_PersonId)
                .Index(t => t.Event_EventId);
            
            CreateTable(
                "dbo.LanguageProficiencyPerson",
                c => new
                    {
                        LanguageProficiency_LanguageProficiencyId = c.Int(nullable: false),
                        Person_PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LanguageProficiency_LanguageProficiencyId, t.Person_PersonId })
                .ForeignKey("dbo.LanguageProficiency", t => t.LanguageProficiency_LanguageProficiencyId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.Person_PersonId, cascadeDelete: true)
                .Index(t => t.LanguageProficiency_LanguageProficiencyId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.RelatedProjects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        RelatedProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.RelatedProjectId })
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .ForeignKey("dbo.Project", t => t.RelatedProjectId)
                .Index(t => t.ProjectId)
                .Index(t => t.RelatedProjectId);
            
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
            DropForeignKey("dbo.Accommodation", "Host_OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.MoneyFlow", "Source_OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.MoneyFlow", "Recipient_OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.MoneyFlow", "Parent_MoneyFlowId", "dbo.MoneyFlow");
            DropForeignKey("dbo.Transportation", "ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.Transportation", "Carrier_OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.ParticipantStatus", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.Person", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.ItineraryStop", "Origin_LocationId", "dbo.Location");
            DropForeignKey("dbo.MoneyFlow", "ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.Material", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.ItineraryStop", "ItineraryId", "dbo.Itinerary");
            DropForeignKey("dbo.ParticipantStatus", "Itinerary_ItineraryId", "dbo.Itinerary");
            DropForeignKey("dbo.ItineraryStop", "Destination_LocationId", "dbo.Location");
            DropForeignKey("dbo.Course", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.Actor", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Actor", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Actor", "ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.Event", "LocationId", "dbo.Location");
            DropForeignKey("dbo.Event", "EventTypeId", "dbo.EventType");
            DropForeignKey("dbo.Artifact", "ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.Impact", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Impact", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.ProgramTheme", "ThemeId", "dbo.Theme");
            DropForeignKey("dbo.ProgramTheme", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.Location", "ProgramOfTarget_ProgramId", "dbo.Program");
            DropForeignKey("dbo.Location", "ProgramOfRegion_ProgramId", "dbo.Program");
            DropForeignKey("dbo.Program", "Owner_OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.MoneyFlow", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.Location", "ProgramOfLocation_ProgramId", "dbo.Program");
            DropForeignKey("dbo.Location", "Region_LocationId", "dbo.Location");
            DropForeignKey("dbo.ProjectTheme", "ThemeId", "dbo.Theme");
            DropForeignKey("dbo.ProjectTheme", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Location", "ProjectOfTarget_ProjectId", "dbo.Project");
            DropForeignKey("dbo.RelatedProjects", "RelatedProjectId", "dbo.Project");
            DropForeignKey("dbo.RelatedProjects", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Location", "ProjectOfRegion_ProjectId", "dbo.Project");
            DropForeignKey("dbo.ParticipantStatus", "Project_ProjectId", "dbo.Project");
            DropForeignKey("dbo.ParticipantStatus", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Project", "ParentProgram_ProgramId", "dbo.Program");
            DropForeignKey("dbo.Project", "NominationSource_OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.MoneyFlow", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Location", "ProjectOfLocation_ProjectId", "dbo.Project");
            DropForeignKey("dbo.Project", "EventId", "dbo.Event");
            DropForeignKey("dbo.Artifact", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Location", "LocationTypeId", "dbo.LocationType");
            DropForeignKey("dbo.Person", "Location_LocationId", "dbo.Location");
            DropForeignKey("dbo.SpecialStatus", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Publication", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Artifact", "PublicationId", "dbo.Publication");
            DropForeignKey("dbo.ProminentCategory", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Person", "PersonId", "dbo.Location");
            DropForeignKey("dbo.PhoneNumber", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ParticipantType", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Person", "ParticipantOrigination_LocationId", "dbo.Location");
            DropForeignKey("dbo.NamePart", "PersonId", "dbo.Person");
            DropForeignKey("dbo.MoneyFlow", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Membership", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.LanguageProficiencyPerson", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.LanguageProficiencyPerson", "LanguageProficiency_LanguageProficiencyId", "dbo.LanguageProficiency");
            DropForeignKey("dbo.InterestSpecialization", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Person", "ImpactId", "dbo.Impact");
            DropForeignKey("dbo.Person", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ExternalId", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.PersonEvent", "Event_EventId", "dbo.Event");
            DropForeignKey("dbo.PersonEvent", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.EmailAddress", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.ProfessionEducation", "PersonOfProfession_PersonId", "dbo.Person");
            DropForeignKey("dbo.ProfessionEducation", "PersonOfEducation_PersonId", "dbo.Person");
            DropForeignKey("dbo.ProfessionEducation", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Location", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Address", "PersonId", "dbo.Person");
            DropForeignKey("dbo.SocialMedia", "PersonId", "dbo.Person");
            DropForeignKey("dbo.SocialMedia", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Organization", "OrganizationTypeId", "dbo.OrganizationType");
            DropForeignKey("dbo.PhoneNumber", "Contact_ContactId", "dbo.Contact");
            DropForeignKey("dbo.Contact", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.EmailAddress", "Contact_ContactId", "dbo.Contact");
            DropForeignKey("dbo.Address", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Address", "LocationId", "dbo.Location");
            DropForeignKey("dbo.Address", "AddressTypeId", "dbo.AddressType");
            DropForeignKey("dbo.Program", "ParentProgram_ProgramId", "dbo.Program");
            DropForeignKey("dbo.Artifact", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.ImpactType", "Impact_ImpactId", "dbo.Impact");
            DropForeignKey("dbo.Artifact", "ImpactId", "dbo.Impact");
            DropForeignKey("dbo.Artifact", "EventId", "dbo.Event");
            DropForeignKey("dbo.Artifact", "ArtifactTypeId", "dbo.ArtifactType");
            DropForeignKey("dbo.Actor", "EventId", "dbo.Event");
            DropForeignKey("dbo.ItineraryStopAccommodation", "Accommodation_AccommodationId", "dbo.Accommodation");
            DropForeignKey("dbo.ItineraryStopAccommodation", "ItineraryStop_ItineraryStopId", "dbo.ItineraryStop");
            DropForeignKey("dbo.MoneyFlow", "AccommodationId", "dbo.Accommodation");
            DropIndex("dbo.ProgramTheme", new[] { "ThemeId" });
            DropIndex("dbo.ProgramTheme", new[] { "ProgramId" });
            DropIndex("dbo.ProjectTheme", new[] { "ThemeId" });
            DropIndex("dbo.ProjectTheme", new[] { "ProjectId" });
            DropIndex("dbo.RelatedProjects", new[] { "RelatedProjectId" });
            DropIndex("dbo.RelatedProjects", new[] { "ProjectId" });
            DropIndex("dbo.LanguageProficiencyPerson", new[] { "Person_PersonId" });
            DropIndex("dbo.LanguageProficiencyPerson", new[] { "LanguageProficiency_LanguageProficiencyId" });
            DropIndex("dbo.PersonEvent", new[] { "Event_EventId" });
            DropIndex("dbo.PersonEvent", new[] { "Person_PersonId" });
            DropIndex("dbo.ItineraryStopAccommodation", new[] { "Accommodation_AccommodationId" });
            DropIndex("dbo.ItineraryStopAccommodation", new[] { "ItineraryStop_ItineraryStopId" });
            DropIndex("dbo.Transportation", new[] { "Carrier_OrganizationId" });
            DropIndex("dbo.Transportation", new[] { "ItineraryStopId" });
            DropIndex("dbo.Material", new[] { "ItineraryStop_ItineraryStopId" });
            DropIndex("dbo.Course", new[] { "ItineraryStop_ItineraryStopId" });
            DropIndex("dbo.ParticipantStatus", new[] { "ItineraryStop_ItineraryStopId" });
            DropIndex("dbo.ParticipantStatus", new[] { "Itinerary_ItineraryId" });
            DropIndex("dbo.ParticipantStatus", new[] { "Project_ProjectId" });
            DropIndex("dbo.ParticipantStatus", new[] { "Person_PersonId" });
            DropIndex("dbo.Project", new[] { "ParentProgram_ProgramId" });
            DropIndex("dbo.Project", new[] { "NominationSource_OrganizationId" });
            DropIndex("dbo.Project", new[] { "EventId" });
            DropIndex("dbo.SpecialStatus", new[] { "Person_PersonId" });
            DropIndex("dbo.Publication", new[] { "PersonId" });
            DropIndex("dbo.ProminentCategory", new[] { "Person_PersonId" });
            DropIndex("dbo.ParticipantType", new[] { "Person_PersonId" });
            DropIndex("dbo.NamePart", new[] { "PersonId" });
            DropIndex("dbo.Membership", new[] { "Person_PersonId" });
            DropIndex("dbo.InterestSpecialization", new[] { "Person_PersonId" });
            DropIndex("dbo.ExternalId", new[] { "Person_PersonId" });
            DropIndex("dbo.ProfessionEducation", new[] { "PersonOfProfession_PersonId" });
            DropIndex("dbo.ProfessionEducation", new[] { "PersonOfEducation_PersonId" });
            DropIndex("dbo.ProfessionEducation", new[] { "OrganizationId" });
            DropIndex("dbo.SocialMedia", new[] { "PersonId" });
            DropIndex("dbo.SocialMedia", new[] { "OrganizationId" });
            DropIndex("dbo.PhoneNumber", new[] { "Person_PersonId" });
            DropIndex("dbo.PhoneNumber", new[] { "Contact_ContactId" });
            DropIndex("dbo.EmailAddress", new[] { "Person_PersonId" });
            DropIndex("dbo.EmailAddress", new[] { "Contact_ContactId" });
            DropIndex("dbo.Contact", new[] { "OrganizationId" });
            DropIndex("dbo.Organization", new[] { "OrganizationTypeId" });
            DropIndex("dbo.Address", new[] { "OrganizationId" });
            DropIndex("dbo.Address", new[] { "PersonId" });
            DropIndex("dbo.Address", new[] { "LocationId" });
            DropIndex("dbo.Address", new[] { "AddressTypeId" });
            DropIndex("dbo.Person", new[] { "ItineraryStop_ItineraryStopId" });
            DropIndex("dbo.Person", new[] { "Location_LocationId" });
            DropIndex("dbo.Person", new[] { "ParticipantOrigination_LocationId" });
            DropIndex("dbo.Person", new[] { "Person_PersonId" });
            DropIndex("dbo.Person", new[] { "ImpactId" });
            DropIndex("dbo.Person", new[] { "PersonId" });
            DropIndex("dbo.Location", new[] { "ProgramOfTarget_ProgramId" });
            DropIndex("dbo.Location", new[] { "ProgramOfRegion_ProgramId" });
            DropIndex("dbo.Location", new[] { "ProgramOfLocation_ProgramId" });
            DropIndex("dbo.Location", new[] { "Region_LocationId" });
            DropIndex("dbo.Location", new[] { "ProjectOfTarget_ProjectId" });
            DropIndex("dbo.Location", new[] { "ProjectOfRegion_ProjectId" });
            DropIndex("dbo.Location", new[] { "ProjectOfLocation_ProjectId" });
            DropIndex("dbo.Location", new[] { "Person_PersonId" });
            DropIndex("dbo.Location", new[] { "LocationTypeId" });
            DropIndex("dbo.Program", new[] { "Owner_OrganizationId" });
            DropIndex("dbo.Program", new[] { "ParentProgram_ProgramId" });
            DropIndex("dbo.ImpactType", new[] { "Impact_ImpactId" });
            DropIndex("dbo.Impact", new[] { "ProjectId" });
            DropIndex("dbo.Impact", new[] { "ProgramId" });
            DropIndex("dbo.Artifact", new[] { "ImpactId" });
            DropIndex("dbo.Artifact", new[] { "ItineraryStopId" });
            DropIndex("dbo.Artifact", new[] { "PublicationId" });
            DropIndex("dbo.Artifact", new[] { "ProgramId" });
            DropIndex("dbo.Artifact", new[] { "ProjectId" });
            DropIndex("dbo.Artifact", new[] { "EventId" });
            DropIndex("dbo.Artifact", new[] { "ArtifactTypeId" });
            DropIndex("dbo.Event", new[] { "LocationId" });
            DropIndex("dbo.Event", new[] { "EventTypeId" });
            DropIndex("dbo.Actor", new[] { "ItineraryStopId" });
            DropIndex("dbo.Actor", new[] { "EventId" });
            DropIndex("dbo.Actor", new[] { "OrganizationId" });
            DropIndex("dbo.Actor", new[] { "PersonId" });
            DropIndex("dbo.ItineraryStop", new[] { "Origin_LocationId" });
            DropIndex("dbo.ItineraryStop", new[] { "Destination_LocationId" });
            DropIndex("dbo.ItineraryStop", new[] { "ItineraryId" });
            DropIndex("dbo.MoneyFlow", new[] { "Source_OrganizationId" });
            DropIndex("dbo.MoneyFlow", new[] { "Recipient_OrganizationId" });
            DropIndex("dbo.MoneyFlow", new[] { "Parent_MoneyFlowId" });
            DropIndex("dbo.MoneyFlow", new[] { "AccommodationId" });
            DropIndex("dbo.MoneyFlow", new[] { "ItineraryStopId" });
            DropIndex("dbo.MoneyFlow", new[] { "PersonId" });
            DropIndex("dbo.MoneyFlow", new[] { "ProjectId" });
            DropIndex("dbo.MoneyFlow", new[] { "ProgramId" });
            DropIndex("dbo.Accommodation", new[] { "Host_OrganizationId" });
            DropTable("dbo.ProgramTheme");
            DropTable("dbo.ProjectTheme");
            DropTable("dbo.RelatedProjects");
            DropTable("dbo.LanguageProficiencyPerson");
            DropTable("dbo.PersonEvent");
            DropTable("dbo.ItineraryStopAccommodation");
            DropTable("dbo.Transportation");
            DropTable("dbo.Material");
            DropTable("dbo.Itinerary");
            DropTable("dbo.Course");
            DropTable("dbo.EventType");
            DropTable("dbo.Theme");
            DropTable("dbo.ParticipantStatus");
            DropTable("dbo.Project");
            DropTable("dbo.LocationType");
            DropTable("dbo.SpecialStatus");
            DropTable("dbo.Publication");
            DropTable("dbo.ProminentCategory");
            DropTable("dbo.ParticipantType");
            DropTable("dbo.NamePart");
            DropTable("dbo.Membership");
            DropTable("dbo.LanguageProficiency");
            DropTable("dbo.InterestSpecialization");
            DropTable("dbo.ExternalId");
            DropTable("dbo.ProfessionEducation");
            DropTable("dbo.SocialMedia");
            DropTable("dbo.OrganizationType");
            DropTable("dbo.PhoneNumber");
            DropTable("dbo.EmailAddress");
            DropTable("dbo.Contact");
            DropTable("dbo.Organization");
            DropTable("dbo.AddressType");
            DropTable("dbo.Address");
            DropTable("dbo.Person");
            DropTable("dbo.Location");
            DropTable("dbo.Program");
            DropTable("dbo.ImpactType");
            DropTable("dbo.Impact");
            DropTable("dbo.ArtifactType");
            DropTable("dbo.Artifact");
            DropTable("dbo.Event");
            DropTable("dbo.Actor");
            DropTable("dbo.ItineraryStop");
            DropTable("dbo.MoneyFlow");
            DropTable("dbo.Accommodation");
        }
    }
}
