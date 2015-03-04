using ECA.Core.Data;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Test
{
    public class DbContextHelper
    {
        public static int DATE_PRECISION = 2000;

        public static TestEcaContext GetInMemoryContext()
        {
            return new TestEcaContext
            {
                Accommodations = new TestDbSet<Accommodation>(),
                Actors = new TestDbSet<Actor>(),
                Addresses = new TestDbSet<Address>(),
                AddressTypes = new TestDbSet<AddressType>(),
                Artifacts = new TestDbSet<Artifact>(),
                ArtifactTypes = new TestDbSet<ArtifactType>(),
                Contacts = new TestDbSet<Contact>(),
                Courses = new TestDbSet<Course>(),
                EmailAddresses = new TestDbSet<EmailAddress>(),
                Events = new TestDbSet<Event>(),
                EventTypes = new TestDbSet<EventType>(),
                ExternalIds = new TestDbSet<ExternalId>(),
                Genders = new TestDbSet<Gender>(),
                Goals = new TestDbSet<Goal>(),
                Impacts = new TestDbSet<Impact>(),
                ImpactTypes = new TestDbSet<ImpactType>(),
                InterestSpecializations = new TestDbSet<InterestSpecialization>(),
                Itineraries = new TestDbSet<Itinerary>(),
                ItineraryStops = new TestDbSet<ItineraryStop>(),
                LanguangeProficiencies = new TestDbSet<LanguageProficiency>(),
                Locations = new TestDbSet<Location>(),
                LocationTypes = new TestDbSet<LocationType>(),
                Materials = new TestDbSet<Material>(),
                Memberships = new TestDbSet<Membership>(),
                MoneyFlows = new TestDbSet<MoneyFlow>(),
                MoneyFlowSourceRecipientTypes = new TestDbSet<MoneyFlowSourceRecipientType>(),
                MoneyFlowStatuses = new TestDbSet<MoneyFlowStatus>(),
                MoneyFlowTypes = new TestDbSet<MoneyFlowType>(),
                Organizations = new TestDbSet<Organization>(),
                OrganizationTypes = new TestDbSet<OrganizationType>(),
                Participants = new TestDbSet<Participant>(),
                ParticipantStatuses = new TestDbSet<ParticipantStatus>(),
                ParticipantTypes = new TestDbSet<ParticipantType>(),
                People = new TestDbSet<Person>(),
                PhoneNumbers = new TestDbSet<PhoneNumber>(),
                ProfessionEducations = new TestDbSet<ProfessionEducation>(),
                Programs = new TestDbSet<Program>(),
                ProgramTypes = new TestDbSet<ProgramType>(),
                Projects = new TestDbSet<Project>(),
                ProjectStatuses = new TestDbSet<ProjectStatus>(),
                ProminentCategories = new TestDbSet<ProminentCategory>(),
                Publications = new TestDbSet<Publication>(),
                SocialMedias = new TestDbSet<SocialMedia>(),
                SpecialStatuses = new TestDbSet<SpecialStatus>(),
                Themes = new TestDbSet<Theme>(),
                Transportations = new TestDbSet<Transportation>()
            };
        }
    }
}
