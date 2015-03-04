






namespace ECA.Data.Test
{
	#region EcaContext
	public class InMemoryEcaContext : ECA.Data.EcaContext
	{
		public InMemoryEcaContext()
		{
			this.Accommodations = new ECA.Core.Data.TestDbSet<ECA.Data.Accommodation>();
			this.Actors = new ECA.Core.Data.TestDbSet<ECA.Data.Actor>();
			this.Addresses = new ECA.Core.Data.TestDbSet<ECA.Data.Address>();
			this.AddressTypes = new ECA.Core.Data.TestDbSet<ECA.Data.AddressType>();
			this.Artifacts = new ECA.Core.Data.TestDbSet<ECA.Data.Artifact>();
			this.ArtifactTypes = new ECA.Core.Data.TestDbSet<ECA.Data.ArtifactType>();
			this.Contacts = new ECA.Core.Data.TestDbSet<ECA.Data.Contact>();
			this.Courses = new ECA.Core.Data.TestDbSet<ECA.Data.Course>();
			this.EmailAddresses = new ECA.Core.Data.TestDbSet<ECA.Data.EmailAddress>();
			this.Events = new ECA.Core.Data.TestDbSet<ECA.Data.Event>();
			this.EventTypes = new ECA.Core.Data.TestDbSet<ECA.Data.EventType>();
			this.ExternalIds = new ECA.Core.Data.TestDbSet<ECA.Data.ExternalId>();
			this.Foci = new ECA.Core.Data.TestDbSet<ECA.Data.Focus>();
			this.Genders = new ECA.Core.Data.TestDbSet<ECA.Data.Gender>();
			this.Goals = new ECA.Core.Data.TestDbSet<ECA.Data.Goal>();
			this.Impacts = new ECA.Core.Data.TestDbSet<ECA.Data.Impact>();
			this.ImpactTypes = new ECA.Core.Data.TestDbSet<ECA.Data.ImpactType>();
			this.InterestSpecializations = new ECA.Core.Data.TestDbSet<ECA.Data.InterestSpecialization>();
			this.Itineraries = new ECA.Core.Data.TestDbSet<ECA.Data.Itinerary>();
			this.ItineraryStops = new ECA.Core.Data.TestDbSet<ECA.Data.ItineraryStop>();
			this.LanguangeProficiencies = new ECA.Core.Data.TestDbSet<ECA.Data.LanguageProficiency>();
			this.Locations = new ECA.Core.Data.TestDbSet<ECA.Data.Location>();
			this.LocationTypes = new ECA.Core.Data.TestDbSet<ECA.Data.LocationType>();
			this.Materials = new ECA.Core.Data.TestDbSet<ECA.Data.Material>();
			this.Memberships = new ECA.Core.Data.TestDbSet<ECA.Data.Membership>();
			this.MoneyFlows = new ECA.Core.Data.TestDbSet<ECA.Data.MoneyFlow>();
			this.MoneyFlowSourceRecipientTypes = new ECA.Core.Data.TestDbSet<ECA.Data.MoneyFlowSourceRecipientType>();
			this.MoneyFlowStatuses = new ECA.Core.Data.TestDbSet<ECA.Data.MoneyFlowStatus>();
			this.MoneyFlowTypes = new ECA.Core.Data.TestDbSet<ECA.Data.MoneyFlowType>();
			this.Organizations = new ECA.Core.Data.TestDbSet<ECA.Data.Organization>();
			this.OrganizationTypes = new ECA.Core.Data.TestDbSet<ECA.Data.OrganizationType>();
			this.Participants = new ECA.Core.Data.TestDbSet<ECA.Data.Participant>();
			this.ParticipantStatuses = new ECA.Core.Data.TestDbSet<ECA.Data.ParticipantStatus>();
			this.ParticipantTypes = new ECA.Core.Data.TestDbSet<ECA.Data.ParticipantType>();
			this.People = new ECA.Core.Data.TestDbSet<ECA.Data.Person>();
			this.PhoneNumbers = new ECA.Core.Data.TestDbSet<ECA.Data.PhoneNumber>();
			this.ProfessionEducations = new ECA.Core.Data.TestDbSet<ECA.Data.ProfessionEducation>();
			this.Programs = new ECA.Core.Data.TestDbSet<ECA.Data.Program>();
			this.ProgramTypes = new ECA.Core.Data.TestDbSet<ECA.Data.ProgramType>();
			this.Projects = new ECA.Core.Data.TestDbSet<ECA.Data.Project>();
			this.ProjectStatuses = new ECA.Core.Data.TestDbSet<ECA.Data.ProjectStatus>();
			this.ProminentCategories = new ECA.Core.Data.TestDbSet<ECA.Data.ProminentCategory>();
			this.Publications = new ECA.Core.Data.TestDbSet<ECA.Data.Publication>();
			this.SocialMedias = new ECA.Core.Data.TestDbSet<ECA.Data.SocialMedia>();
			this.SpecialStatuses = new ECA.Core.Data.TestDbSet<ECA.Data.SpecialStatus>();
			this.Themes = new ECA.Core.Data.TestDbSet<ECA.Data.Theme>();
			this.Transportations = new ECA.Core.Data.TestDbSet<ECA.Data.Transportation>();
		}
	}
	#endregion
}
