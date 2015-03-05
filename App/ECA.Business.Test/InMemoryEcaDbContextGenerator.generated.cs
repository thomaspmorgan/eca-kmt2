






namespace ECA.Business.Test
{
	using System.Linq;
	using System.Threading.Tasks;
	#region EcaContext
	public class AccommodationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Accommodation>
	{
		public override ECA.Data.Accommodation Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AccommodationId
			return this.SingleOrDefault(x => x.AccommodationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Accommodation> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AccommodationId
			return Task.FromResult<ECA.Data.Accommodation>(this.SingleOrDefault(x => x.AccommodationId.Equals(keyValues.First())));
		}
	}
	public class ActorTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Actor>
	{
		public override ECA.Data.Actor Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ActorId
			return this.SingleOrDefault(x => x.ActorId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Actor> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ActorId
			return Task.FromResult<ECA.Data.Actor>(this.SingleOrDefault(x => x.ActorId.Equals(keyValues.First())));
		}
	}
	public class AddressTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Address>
	{
		public override ECA.Data.Address Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AddressId
			return this.SingleOrDefault(x => x.AddressId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Address> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AddressId
			return Task.FromResult<ECA.Data.Address>(this.SingleOrDefault(x => x.AddressId.Equals(keyValues.First())));
		}
	}
	public class AddressTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.AddressType>
	{
		public override ECA.Data.AddressType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AddressTypeId
			return this.SingleOrDefault(x => x.AddressTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.AddressType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AddressTypeId
			return Task.FromResult<ECA.Data.AddressType>(this.SingleOrDefault(x => x.AddressTypeId.Equals(keyValues.First())));
		}
	}
	public class ArtifactTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Artifact>
	{
		public override ECA.Data.Artifact Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ArtifactId
			return this.SingleOrDefault(x => x.ArtifactId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Artifact> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ArtifactId
			return Task.FromResult<ECA.Data.Artifact>(this.SingleOrDefault(x => x.ArtifactId.Equals(keyValues.First())));
		}
	}
	public class ArtifactTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ArtifactType>
	{
		public override ECA.Data.ArtifactType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ArtifactTypeId
			return this.SingleOrDefault(x => x.ArtifactTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ArtifactType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ArtifactTypeId
			return Task.FromResult<ECA.Data.ArtifactType>(this.SingleOrDefault(x => x.ArtifactTypeId.Equals(keyValues.First())));
		}
	}
	public class ContactTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Contact>
	{
		public override ECA.Data.Contact Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ContactId
			return this.SingleOrDefault(x => x.ContactId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Contact> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ContactId
			return Task.FromResult<ECA.Data.Contact>(this.SingleOrDefault(x => x.ContactId.Equals(keyValues.First())));
		}
	}
	public class CourseTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Course>
	{
		public override ECA.Data.Course Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return this.SingleOrDefault(x => x.Id.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Course> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return Task.FromResult<ECA.Data.Course>(this.SingleOrDefault(x => x.Id.Equals(keyValues.First())));
		}
	}
	public class EmailAddressTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.EmailAddress>
	{
		public override ECA.Data.EmailAddress Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EmailAddressId
			return this.SingleOrDefault(x => x.EmailAddressId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.EmailAddress> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EmailAddressId
			return Task.FromResult<ECA.Data.EmailAddress>(this.SingleOrDefault(x => x.EmailAddressId.Equals(keyValues.First())));
		}
	}
	public class EventTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Event>
	{
		public override ECA.Data.Event Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EventId
			return this.SingleOrDefault(x => x.EventId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Event> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EventId
			return Task.FromResult<ECA.Data.Event>(this.SingleOrDefault(x => x.EventId.Equals(keyValues.First())));
		}
	}
	public class EventTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.EventType>
	{
		public override ECA.Data.EventType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EventTypeId
			return this.SingleOrDefault(x => x.EventTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.EventType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EventTypeId
			return Task.FromResult<ECA.Data.EventType>(this.SingleOrDefault(x => x.EventTypeId.Equals(keyValues.First())));
		}
	}
	public class ExternalIdTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ExternalId>
	{
		public override ECA.Data.ExternalId Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ExternalIdId
			return this.SingleOrDefault(x => x.ExternalIdId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ExternalId> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ExternalIdId
			return Task.FromResult<ECA.Data.ExternalId>(this.SingleOrDefault(x => x.ExternalIdId.Equals(keyValues.First())));
		}
	}
	public class FocusTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Focus>
	{
		public override ECA.Data.Focus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///FocusId
			return this.SingleOrDefault(x => x.FocusId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Focus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///FocusId
			return Task.FromResult<ECA.Data.Focus>(this.SingleOrDefault(x => x.FocusId.Equals(keyValues.First())));
		}
	}
	public class GenderTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Gender>
	{
		public override ECA.Data.Gender Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///GenderId
			return this.SingleOrDefault(x => x.GenderId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Gender> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///GenderId
			return Task.FromResult<ECA.Data.Gender>(this.SingleOrDefault(x => x.GenderId.Equals(keyValues.First())));
		}
	}
	public class GoalTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Goal>
	{
		public override ECA.Data.Goal Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///GoalId
			return this.SingleOrDefault(x => x.GoalId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Goal> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///GoalId
			return Task.FromResult<ECA.Data.Goal>(this.SingleOrDefault(x => x.GoalId.Equals(keyValues.First())));
		}
	}
	public class ImpactTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Impact>
	{
		public override ECA.Data.Impact Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ImpactId
			return this.SingleOrDefault(x => x.ImpactId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Impact> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ImpactId
			return Task.FromResult<ECA.Data.Impact>(this.SingleOrDefault(x => x.ImpactId.Equals(keyValues.First())));
		}
	}
	public class ImpactTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ImpactType>
	{
		public override ECA.Data.ImpactType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ImpactTypeId
			return this.SingleOrDefault(x => x.ImpactTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ImpactType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ImpactTypeId
			return Task.FromResult<ECA.Data.ImpactType>(this.SingleOrDefault(x => x.ImpactTypeId.Equals(keyValues.First())));
		}
	}
	public class InterestSpecializationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.InterestSpecialization>
	{
		public override ECA.Data.InterestSpecialization Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///InterestSpecializationId
			return this.SingleOrDefault(x => x.InterestSpecializationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.InterestSpecialization> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///InterestSpecializationId
			return Task.FromResult<ECA.Data.InterestSpecialization>(this.SingleOrDefault(x => x.InterestSpecializationId.Equals(keyValues.First())));
		}
	}
	public class ItineraryTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Itinerary>
	{
		public override ECA.Data.Itinerary Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ItineraryId
			return this.SingleOrDefault(x => x.ItineraryId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Itinerary> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ItineraryId
			return Task.FromResult<ECA.Data.Itinerary>(this.SingleOrDefault(x => x.ItineraryId.Equals(keyValues.First())));
		}
	}
	public class ItineraryStopTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ItineraryStop>
	{
		public override ECA.Data.ItineraryStop Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ItineraryStopId
			return this.SingleOrDefault(x => x.ItineraryStopId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ItineraryStop> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ItineraryStopId
			return Task.FromResult<ECA.Data.ItineraryStop>(this.SingleOrDefault(x => x.ItineraryStopId.Equals(keyValues.First())));
		}
	}
	public class LanguageProficiencyTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.LanguageProficiency>
	{
		public override ECA.Data.LanguageProficiency Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LanguageProficiencyId
			return this.SingleOrDefault(x => x.LanguageProficiencyId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.LanguageProficiency> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LanguageProficiencyId
			return Task.FromResult<ECA.Data.LanguageProficiency>(this.SingleOrDefault(x => x.LanguageProficiencyId.Equals(keyValues.First())));
		}
	}
	public class LocationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Location>
	{
		public override ECA.Data.Location Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LocationId
			return this.SingleOrDefault(x => x.LocationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Location> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LocationId
			return Task.FromResult<ECA.Data.Location>(this.SingleOrDefault(x => x.LocationId.Equals(keyValues.First())));
		}
	}
	public class LocationTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.LocationType>
	{
		public override ECA.Data.LocationType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LocationTypeId
			return this.SingleOrDefault(x => x.LocationTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.LocationType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LocationTypeId
			return Task.FromResult<ECA.Data.LocationType>(this.SingleOrDefault(x => x.LocationTypeId.Equals(keyValues.First())));
		}
	}
	public class MaterialTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Material>
	{
		public override ECA.Data.Material Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MaterialId
			return this.SingleOrDefault(x => x.MaterialId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Material> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MaterialId
			return Task.FromResult<ECA.Data.Material>(this.SingleOrDefault(x => x.MaterialId.Equals(keyValues.First())));
		}
	}
	public class MembershipTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Membership>
	{
		public override ECA.Data.Membership Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MembershipId
			return this.SingleOrDefault(x => x.MembershipId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Membership> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MembershipId
			return Task.FromResult<ECA.Data.Membership>(this.SingleOrDefault(x => x.MembershipId.Equals(keyValues.First())));
		}
	}
	public class MoneyFlowTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.MoneyFlow>
	{
		public override ECA.Data.MoneyFlow Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MoneyFlowId
			return this.SingleOrDefault(x => x.MoneyFlowId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.MoneyFlow> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MoneyFlowId
			return Task.FromResult<ECA.Data.MoneyFlow>(this.SingleOrDefault(x => x.MoneyFlowId.Equals(keyValues.First())));
		}
	}
	public class MoneyFlowSourceRecipientTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.MoneyFlowSourceRecipientType>
	{
		public override ECA.Data.MoneyFlowSourceRecipientType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MoneyFlowSourceRecipientTypeId
			return this.SingleOrDefault(x => x.MoneyFlowSourceRecipientTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.MoneyFlowSourceRecipientType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MoneyFlowSourceRecipientTypeId
			return Task.FromResult<ECA.Data.MoneyFlowSourceRecipientType>(this.SingleOrDefault(x => x.MoneyFlowSourceRecipientTypeId.Equals(keyValues.First())));
		}
	}
	public class MoneyFlowStatusTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.MoneyFlowStatus>
	{
		public override ECA.Data.MoneyFlowStatus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MoneyFlowStatusId
			return this.SingleOrDefault(x => x.MoneyFlowStatusId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.MoneyFlowStatus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MoneyFlowStatusId
			return Task.FromResult<ECA.Data.MoneyFlowStatus>(this.SingleOrDefault(x => x.MoneyFlowStatusId.Equals(keyValues.First())));
		}
	}
	public class MoneyFlowTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.MoneyFlowType>
	{
		public override ECA.Data.MoneyFlowType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MoneyFlowTypeId
			return this.SingleOrDefault(x => x.MoneyFlowTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.MoneyFlowType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MoneyFlowTypeId
			return Task.FromResult<ECA.Data.MoneyFlowType>(this.SingleOrDefault(x => x.MoneyFlowTypeId.Equals(keyValues.First())));
		}
	}
	public class OrganizationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Organization>
	{
		public override ECA.Data.Organization Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OrganizationId
			return this.SingleOrDefault(x => x.OrganizationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Organization> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OrganizationId
			return Task.FromResult<ECA.Data.Organization>(this.SingleOrDefault(x => x.OrganizationId.Equals(keyValues.First())));
		}
	}
	public class OrganizationTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.OrganizationType>
	{
		public override ECA.Data.OrganizationType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OrganizationTypeId
			return this.SingleOrDefault(x => x.OrganizationTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.OrganizationType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OrganizationTypeId
			return Task.FromResult<ECA.Data.OrganizationType>(this.SingleOrDefault(x => x.OrganizationTypeId.Equals(keyValues.First())));
		}
	}
	public class ParticipantTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Participant>
	{
		public override ECA.Data.Participant Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantId
			return this.SingleOrDefault(x => x.ParticipantId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Participant> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantId
			return Task.FromResult<ECA.Data.Participant>(this.SingleOrDefault(x => x.ParticipantId.Equals(keyValues.First())));
		}
	}
	public class ParticipantStatusTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ParticipantStatus>
	{
		public override ECA.Data.ParticipantStatus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantStatusId
			return this.SingleOrDefault(x => x.ParticipantStatusId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ParticipantStatus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantStatusId
			return Task.FromResult<ECA.Data.ParticipantStatus>(this.SingleOrDefault(x => x.ParticipantStatusId.Equals(keyValues.First())));
		}
	}
	public class ParticipantTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ParticipantType>
	{
		public override ECA.Data.ParticipantType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantTypeId
			return this.SingleOrDefault(x => x.ParticipantTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ParticipantType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantTypeId
			return Task.FromResult<ECA.Data.ParticipantType>(this.SingleOrDefault(x => x.ParticipantTypeId.Equals(keyValues.First())));
		}
	}
	public class PersonTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Person>
	{
		public override ECA.Data.Person Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PersonId
			return this.SingleOrDefault(x => x.PersonId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Person> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PersonId
			return Task.FromResult<ECA.Data.Person>(this.SingleOrDefault(x => x.PersonId.Equals(keyValues.First())));
		}
	}
	public class PhoneNumberTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.PhoneNumber>
	{
		public override ECA.Data.PhoneNumber Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PhoneNumberId
			return this.SingleOrDefault(x => x.PhoneNumberId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.PhoneNumber> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PhoneNumberId
			return Task.FromResult<ECA.Data.PhoneNumber>(this.SingleOrDefault(x => x.PhoneNumberId.Equals(keyValues.First())));
		}
	}
	public class ProfessionEducationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ProfessionEducation>
	{
		public override ECA.Data.ProfessionEducation Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProfessionEducationId
			return this.SingleOrDefault(x => x.ProfessionEducationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ProfessionEducation> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProfessionEducationId
			return Task.FromResult<ECA.Data.ProfessionEducation>(this.SingleOrDefault(x => x.ProfessionEducationId.Equals(keyValues.First())));
		}
	}
	public class ProgramTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Program>
	{
		public override ECA.Data.Program Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProgramId
			return this.SingleOrDefault(x => x.ProgramId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Program> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProgramId
			return Task.FromResult<ECA.Data.Program>(this.SingleOrDefault(x => x.ProgramId.Equals(keyValues.First())));
		}
	}
	public class ProgramTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ProgramType>
	{
		public override ECA.Data.ProgramType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProgramTypeId
			return this.SingleOrDefault(x => x.ProgramTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ProgramType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProgramTypeId
			return Task.FromResult<ECA.Data.ProgramType>(this.SingleOrDefault(x => x.ProgramTypeId.Equals(keyValues.First())));
		}
	}
	public class ProjectTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Project>
	{
		public override ECA.Data.Project Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProjectId
			return this.SingleOrDefault(x => x.ProjectId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Project> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProjectId
			return Task.FromResult<ECA.Data.Project>(this.SingleOrDefault(x => x.ProjectId.Equals(keyValues.First())));
		}
	}
	public class ProjectStatusTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ProjectStatus>
	{
		public override ECA.Data.ProjectStatus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProjectStatusId
			return this.SingleOrDefault(x => x.ProjectStatusId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ProjectStatus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProjectStatusId
			return Task.FromResult<ECA.Data.ProjectStatus>(this.SingleOrDefault(x => x.ProjectStatusId.Equals(keyValues.First())));
		}
	}
	public class ProminentCategoryTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ProminentCategory>
	{
		public override ECA.Data.ProminentCategory Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProminentCategoryId
			return this.SingleOrDefault(x => x.ProminentCategoryId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ProminentCategory> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProminentCategoryId
			return Task.FromResult<ECA.Data.ProminentCategory>(this.SingleOrDefault(x => x.ProminentCategoryId.Equals(keyValues.First())));
		}
	}
	public class PublicationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Publication>
	{
		public override ECA.Data.Publication Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PublicationId
			return this.SingleOrDefault(x => x.PublicationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Publication> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PublicationId
			return Task.FromResult<ECA.Data.Publication>(this.SingleOrDefault(x => x.PublicationId.Equals(keyValues.First())));
		}
	}
	public class SocialMediaTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.SocialMedia>
	{
		public override ECA.Data.SocialMedia Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///SocialMediaId
			return this.SingleOrDefault(x => x.SocialMediaId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.SocialMedia> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///SocialMediaId
			return Task.FromResult<ECA.Data.SocialMedia>(this.SingleOrDefault(x => x.SocialMediaId.Equals(keyValues.First())));
		}
	}
	public class SpecialStatusTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.SpecialStatus>
	{
		public override ECA.Data.SpecialStatus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///SpecialStatusId
			return this.SingleOrDefault(x => x.SpecialStatusId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.SpecialStatus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///SpecialStatusId
			return Task.FromResult<ECA.Data.SpecialStatus>(this.SingleOrDefault(x => x.SpecialStatusId.Equals(keyValues.First())));
		}
	}
	public class ThemeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Theme>
	{
		public override ECA.Data.Theme Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ThemeId
			return this.SingleOrDefault(x => x.ThemeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Theme> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ThemeId
			return Task.FromResult<ECA.Data.Theme>(this.SingleOrDefault(x => x.ThemeId.Equals(keyValues.First())));
		}
	}
	public class TransportationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Transportation>
	{
		public override ECA.Data.Transportation Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///TransportationId
			return this.SingleOrDefault(x => x.TransportationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Transportation> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///TransportationId
			return Task.FromResult<ECA.Data.Transportation>(this.SingleOrDefault(x => x.TransportationId.Equals(keyValues.First())));
		}
	}
	public class InMemoryEcaContext : ECA.Data.EcaContext
	{
		public InMemoryEcaContext()
		{
			this.Accommodations = new AccommodationTestDbSet();
			this.Actors = new ActorTestDbSet();
			this.Addresses = new AddressTestDbSet();
			this.AddressTypes = new AddressTypeTestDbSet();
			this.Artifacts = new ArtifactTestDbSet();
			this.ArtifactTypes = new ArtifactTypeTestDbSet();
			this.Contacts = new ContactTestDbSet();
			this.Courses = new CourseTestDbSet();
			this.EmailAddresses = new EmailAddressTestDbSet();
			this.Events = new EventTestDbSet();
			this.EventTypes = new EventTypeTestDbSet();
			this.ExternalIds = new ExternalIdTestDbSet();
			this.Foci = new FocusTestDbSet();
			this.Genders = new GenderTestDbSet();
			this.Goals = new GoalTestDbSet();
			this.Impacts = new ImpactTestDbSet();
			this.ImpactTypes = new ImpactTypeTestDbSet();
			this.InterestSpecializations = new InterestSpecializationTestDbSet();
			this.Itineraries = new ItineraryTestDbSet();
			this.ItineraryStops = new ItineraryStopTestDbSet();
			this.LanguangeProficiencies = new LanguageProficiencyTestDbSet();
			this.Locations = new LocationTestDbSet();
			this.LocationTypes = new LocationTypeTestDbSet();
			this.Materials = new MaterialTestDbSet();
			this.Memberships = new MembershipTestDbSet();
			this.MoneyFlows = new MoneyFlowTestDbSet();
			this.MoneyFlowSourceRecipientTypes = new MoneyFlowSourceRecipientTypeTestDbSet();
			this.MoneyFlowStatuses = new MoneyFlowStatusTestDbSet();
			this.MoneyFlowTypes = new MoneyFlowTypeTestDbSet();
			this.Organizations = new OrganizationTestDbSet();
			this.OrganizationTypes = new OrganizationTypeTestDbSet();
			this.Participants = new ParticipantTestDbSet();
			this.ParticipantStatuses = new ParticipantStatusTestDbSet();
			this.ParticipantTypes = new ParticipantTypeTestDbSet();
			this.People = new PersonTestDbSet();
			this.PhoneNumbers = new PhoneNumberTestDbSet();
			this.ProfessionEducations = new ProfessionEducationTestDbSet();
			this.Programs = new ProgramTestDbSet();
			this.ProgramTypes = new ProgramTypeTestDbSet();
			this.Projects = new ProjectTestDbSet();
			this.ProjectStatuses = new ProjectStatusTestDbSet();
			this.ProminentCategories = new ProminentCategoryTestDbSet();
			this.Publications = new PublicationTestDbSet();
			this.SocialMedias = new SocialMediaTestDbSet();
			this.SpecialStatuses = new SpecialStatusTestDbSet();
			this.Themes = new ThemeTestDbSet();
			this.Transportations = new TransportationTestDbSet();
		}
	}
	#endregion
}
