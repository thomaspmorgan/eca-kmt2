






namespace ECA.Business.Test
{
	using System;
	using System.Collections.Generic;
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
	public class ActivityTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Activity>
	{
		public override ECA.Data.Activity Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ActivityId
			return this.SingleOrDefault(x => x.ActivityId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Activity> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ActivityId
			return Task.FromResult<ECA.Data.Activity>(this.SingleOrDefault(x => x.ActivityId.Equals(keyValues.First())));
		}
	}
	public class ActivityTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ActivityType>
	{
		public override ECA.Data.ActivityType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ActivityTypeId
			return this.SingleOrDefault(x => x.ActivityTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ActivityType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ActivityTypeId
			return Task.FromResult<ECA.Data.ActivityType>(this.SingleOrDefault(x => x.ActivityTypeId.Equals(keyValues.First())));
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
	public class BirthCountryTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.BirthCountry>
	{
		public override ECA.Data.BirthCountry Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///BirthCountryId
			return this.SingleOrDefault(x => x.BirthCountryId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.BirthCountry> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///BirthCountryId
			return Task.FromResult<ECA.Data.BirthCountry>(this.SingleOrDefault(x => x.BirthCountryId.Equals(keyValues.First())));
		}
	}
	public class BirthCountryReasonTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.BirthCountryReason>
	{
		public override ECA.Data.BirthCountryReason Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///BirthCountryReasonId
			return this.SingleOrDefault(x => x.BirthCountryReasonId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.BirthCountryReason> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///BirthCountryReasonId
			return Task.FromResult<ECA.Data.BirthCountryReason>(this.SingleOrDefault(x => x.BirthCountryReasonId.Equals(keyValues.First())));
		}
	}
	public class BookmarkTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Bookmark>
	{
		public override ECA.Data.Bookmark Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///BookmarkId
			return this.SingleOrDefault(x => x.BookmarkId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Bookmark> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///BookmarkId
			return Task.FromResult<ECA.Data.Bookmark>(this.SingleOrDefault(x => x.BookmarkId.Equals(keyValues.First())));
		}
	}
	public class CancelledSevisBatchProcessingTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.CancelledSevisBatchProcessing>
	{
		public override ECA.Data.CancelledSevisBatchProcessing Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return this.SingleOrDefault(x => x.Id.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.CancelledSevisBatchProcessing> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return Task.FromResult<ECA.Data.CancelledSevisBatchProcessing>(this.SingleOrDefault(x => x.Id.Equals(keyValues.First())));
		}
	}
	public class CategoryTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Category>
	{
		public override ECA.Data.Category Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///CategoryId
			return this.SingleOrDefault(x => x.CategoryId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Category> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///CategoryId
			return Task.FromResult<ECA.Data.Category>(this.SingleOrDefault(x => x.CategoryId.Equals(keyValues.First())));
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
	public class DataPointCategoryPropertyTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.DataPointCategoryProperty>
	{
		public override ECA.Data.DataPointCategoryProperty Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DataPointCategoryPropertyId
			return this.SingleOrDefault(x => x.DataPointCategoryPropertyId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.DataPointCategoryProperty> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DataPointCategoryPropertyId
			return Task.FromResult<ECA.Data.DataPointCategoryProperty>(this.SingleOrDefault(x => x.DataPointCategoryPropertyId.Equals(keyValues.First())));
		}
	}
	public class DataPointConfigurationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.DataPointConfiguration>
	{
		public override ECA.Data.DataPointConfiguration Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DataPointConfigurationId
			return this.SingleOrDefault(x => x.DataPointConfigurationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.DataPointConfiguration> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DataPointConfigurationId
			return Task.FromResult<ECA.Data.DataPointConfiguration>(this.SingleOrDefault(x => x.DataPointConfigurationId.Equals(keyValues.First())));
		}
	}
	public class DefaultExchangeVisitorFundingTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.DefaultExchangeVisitorFunding>
	{
		public override ECA.Data.DefaultExchangeVisitorFunding Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProjectId
			return this.SingleOrDefault(x => x.ProjectId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.DefaultExchangeVisitorFunding> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProjectId
			return Task.FromResult<ECA.Data.DefaultExchangeVisitorFunding>(this.SingleOrDefault(x => x.ProjectId.Equals(keyValues.First())));
		}
	}
	public class DependentTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.DependentType>
	{
		public override ECA.Data.DependentType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DependentTypeId
			return this.SingleOrDefault(x => x.DependentTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.DependentType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DependentTypeId
			return Task.FromResult<ECA.Data.DependentType>(this.SingleOrDefault(x => x.DependentTypeId.Equals(keyValues.First())));
		}
	}
	public class EducationLevelTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.EducationLevel>
	{
		public override ECA.Data.EducationLevel Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EducationLevelId
			return this.SingleOrDefault(x => x.EducationLevelId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.EducationLevel> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EducationLevelId
			return Task.FromResult<ECA.Data.EducationLevel>(this.SingleOrDefault(x => x.EducationLevelId.Equals(keyValues.First())));
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
	public class EmailAddressTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.EmailAddressType>
	{
		public override ECA.Data.EmailAddressType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EmailAddressTypeId
			return this.SingleOrDefault(x => x.EmailAddressTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.EmailAddressType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EmailAddressTypeId
			return Task.FromResult<ECA.Data.EmailAddressType>(this.SingleOrDefault(x => x.EmailAddressTypeId.Equals(keyValues.First())));
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
	public class FieldOfStudyTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.FieldOfStudy>
	{
		public override ECA.Data.FieldOfStudy Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///FieldOfStudyId
			return this.SingleOrDefault(x => x.FieldOfStudyId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.FieldOfStudy> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///FieldOfStudyId
			return Task.FromResult<ECA.Data.FieldOfStudy>(this.SingleOrDefault(x => x.FieldOfStudyId.Equals(keyValues.First())));
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
	public class InternationalOrganizationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.InternationalOrganization>
	{
		public override ECA.Data.InternationalOrganization Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OrganizationId
			return this.SingleOrDefault(x => x.OrganizationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.InternationalOrganization> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OrganizationId
			return Task.FromResult<ECA.Data.InternationalOrganization>(this.SingleOrDefault(x => x.OrganizationId.Equals(keyValues.First())));
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
	public class JustificationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Justification>
	{
		public override ECA.Data.Justification Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///JustificationId
			return this.SingleOrDefault(x => x.JustificationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Justification> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///JustificationId
			return Task.FromResult<ECA.Data.Justification>(this.SingleOrDefault(x => x.JustificationId.Equals(keyValues.First())));
		}
	}
	public class LanguageTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Language>
	{
		public override ECA.Data.Language Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LanguageId
			return this.SingleOrDefault(x => x.LanguageId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Language> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LanguageId
			return Task.FromResult<ECA.Data.Language>(this.SingleOrDefault(x => x.LanguageId.Equals(keyValues.First())));
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
	public class MaritalStatusTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.MaritalStatus>
	{
		public override ECA.Data.MaritalStatus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MaritalStatusId
			return this.SingleOrDefault(x => x.MaritalStatusId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.MaritalStatus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///MaritalStatusId
			return Task.FromResult<ECA.Data.MaritalStatus>(this.SingleOrDefault(x => x.MaritalStatusId.Equals(keyValues.First())));
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
	public class MoneyFlowSourceRecipientTypeSettingTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.MoneyFlowSourceRecipientTypeSetting>
	{
		public override ECA.Data.MoneyFlowSourceRecipientTypeSetting Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return this.SingleOrDefault(x => x.Id.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.MoneyFlowSourceRecipientTypeSetting> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return Task.FromResult<ECA.Data.MoneyFlowSourceRecipientTypeSetting>(this.SingleOrDefault(x => x.Id.Equals(keyValues.First())));
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
	public class ObjectiveTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Objective>
	{
		public override ECA.Data.Objective Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ObjectiveId
			return this.SingleOrDefault(x => x.ObjectiveId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Objective> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ObjectiveId
			return Task.FromResult<ECA.Data.Objective>(this.SingleOrDefault(x => x.ObjectiveId.Equals(keyValues.First())));
		}
	}
	public class OfficeSettingTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.OfficeSetting>
	{
		public override ECA.Data.OfficeSetting Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OfficeSettingId
			return this.SingleOrDefault(x => x.OfficeSettingId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.OfficeSetting> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OfficeSettingId
			return Task.FromResult<ECA.Data.OfficeSetting>(this.SingleOrDefault(x => x.OfficeSettingId.Equals(keyValues.First())));
		}
	}
	public class OrganizationRoleTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.OrganizationRole>
	{
		public override ECA.Data.OrganizationRole Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OrganizationRoleId
			return this.SingleOrDefault(x => x.OrganizationRoleId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.OrganizationRole> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///OrganizationRoleId
			return Task.FromResult<ECA.Data.OrganizationRole>(this.SingleOrDefault(x => x.OrganizationRoleId.Equals(keyValues.First())));
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
	public class ParticipantExchangeVisitorTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ParticipantExchangeVisitor>
	{
		public override ECA.Data.ParticipantExchangeVisitor Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantId
			return this.SingleOrDefault(x => x.ParticipantId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ParticipantExchangeVisitor> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantId
			return Task.FromResult<ECA.Data.ParticipantExchangeVisitor>(this.SingleOrDefault(x => x.ParticipantId.Equals(keyValues.First())));
		}
	}
	public class ParticipantPersonTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ParticipantPerson>
	{
		public override ECA.Data.ParticipantPerson Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantId
			return this.SingleOrDefault(x => x.ParticipantId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ParticipantPerson> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ParticipantId
			return Task.FromResult<ECA.Data.ParticipantPerson>(this.SingleOrDefault(x => x.ParticipantId.Equals(keyValues.First())));
		}
	}
	public class ParticipantPersonSevisCommStatusTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ParticipantPersonSevisCommStatus>
	{
		public override ECA.Data.ParticipantPersonSevisCommStatus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return this.SingleOrDefault(x => x.Id.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ParticipantPersonSevisCommStatus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return Task.FromResult<ECA.Data.ParticipantPersonSevisCommStatus>(this.SingleOrDefault(x => x.Id.Equals(keyValues.First())));
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
	public class PersonDependentCitizenCountryTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.PersonDependentCitizenCountry>
	{
		public override ECA.Data.PersonDependentCitizenCountry Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DependentId
			return this.SingleOrDefault(x => x.DependentId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.PersonDependentCitizenCountry> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DependentId
			return Task.FromResult<ECA.Data.PersonDependentCitizenCountry>(this.SingleOrDefault(x => x.DependentId.Equals(keyValues.First())));
		}
	}
	public class PersonDependentTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.PersonDependent>
	{
		public override ECA.Data.PersonDependent Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DependentId
			return this.SingleOrDefault(x => x.DependentId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.PersonDependent> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///DependentId
			return Task.FromResult<ECA.Data.PersonDependent>(this.SingleOrDefault(x => x.DependentId.Equals(keyValues.First())));
		}
	}
	public class PersonEvaluationNoteTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.PersonEvaluationNote>
	{
		public override ECA.Data.PersonEvaluationNote Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EvaluationNoteId
			return this.SingleOrDefault(x => x.EvaluationNoteId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.PersonEvaluationNote> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///EvaluationNoteId
			return Task.FromResult<ECA.Data.PersonEvaluationNote>(this.SingleOrDefault(x => x.EvaluationNoteId.Equals(keyValues.First())));
		}
	}
	public class PersonLanguageProficiencyTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.PersonLanguageProficiency>
	{
		public override ECA.Data.PersonLanguageProficiency Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LanguageId
			return this.SingleOrDefault(x => x.LanguageId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.PersonLanguageProficiency> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///LanguageId
			return Task.FromResult<ECA.Data.PersonLanguageProficiency>(this.SingleOrDefault(x => x.LanguageId.Equals(keyValues.First())));
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
	public class PhoneNumberTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.PhoneNumberType>
	{
		public override ECA.Data.PhoneNumberType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PhoneNumberTypeId
			return this.SingleOrDefault(x => x.PhoneNumberTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.PhoneNumberType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PhoneNumberTypeId
			return Task.FromResult<ECA.Data.PhoneNumberType>(this.SingleOrDefault(x => x.PhoneNumberTypeId.Equals(keyValues.First())));
		}
	}
	public class PositionTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Position>
	{
		public override ECA.Data.Position Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PositionId
			return this.SingleOrDefault(x => x.PositionId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Position> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PositionId
			return Task.FromResult<ECA.Data.Position>(this.SingleOrDefault(x => x.PositionId.Equals(keyValues.First())));
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
	public class ProgramCategoryTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ProgramCategory>
	{
		public override ECA.Data.ProgramCategory Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProgramCategoryId
			return this.SingleOrDefault(x => x.ProgramCategoryId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ProgramCategory> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProgramCategoryId
			return Task.FromResult<ECA.Data.ProgramCategory>(this.SingleOrDefault(x => x.ProgramCategoryId.Equals(keyValues.First())));
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
	public class ProgramStatusTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.ProgramStatus>
	{
		public override ECA.Data.ProgramStatus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProgramStatusId
			return this.SingleOrDefault(x => x.ProgramStatusId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.ProgramStatus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ProgramStatusId
			return Task.FromResult<ECA.Data.ProgramStatus>(this.SingleOrDefault(x => x.ProgramStatusId.Equals(keyValues.First())));
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
	public class SevisBatchProcessingTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.SevisBatchProcessing>
	{
		public override ECA.Data.SevisBatchProcessing Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return this.SingleOrDefault(x => x.Id.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.SevisBatchProcessing> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///Id
			return Task.FromResult<ECA.Data.SevisBatchProcessing>(this.SingleOrDefault(x => x.Id.Equals(keyValues.First())));
		}
	}
	public class SevisCommStatusTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.SevisCommStatus>
	{
		public override ECA.Data.SevisCommStatus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///SevisCommStatusId
			return this.SingleOrDefault(x => x.SevisCommStatusId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.SevisCommStatus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///SevisCommStatusId
			return Task.FromResult<ECA.Data.SevisCommStatus>(this.SingleOrDefault(x => x.SevisCommStatusId.Equals(keyValues.First())));
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
	public class SocialMediaTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.SocialMediaType>
	{
		public override ECA.Data.SocialMediaType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///SocialMediaTypeId
			return this.SingleOrDefault(x => x.SocialMediaTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.SocialMediaType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///SocialMediaTypeId
			return Task.FromResult<ECA.Data.SocialMediaType>(this.SingleOrDefault(x => x.SocialMediaTypeId.Equals(keyValues.First())));
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
	public class StudentCreationTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.StudentCreation>
	{
		public override ECA.Data.StudentCreation Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///StudentCreationId
			return this.SingleOrDefault(x => x.StudentCreationId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.StudentCreation> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///StudentCreationId
			return Task.FromResult<ECA.Data.StudentCreation>(this.SingleOrDefault(x => x.StudentCreationId.Equals(keyValues.First())));
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
	public class UserAccountTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.UserAccount>
	{
		public override ECA.Data.UserAccount Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.UserAccount> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return Task.FromResult<ECA.Data.UserAccount>(this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First())));
		}
	}
	public class USGovernmentAgencyTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.USGovernmentAgency>
	{
		public override ECA.Data.USGovernmentAgency Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AgencyId
			return this.SingleOrDefault(x => x.AgencyId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.USGovernmentAgency> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AgencyId
			return Task.FromResult<ECA.Data.USGovernmentAgency>(this.SingleOrDefault(x => x.AgencyId.Equals(keyValues.First())));
		}
	}
	public class VisitorTypeTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.VisitorType>
	{
		public override ECA.Data.VisitorType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///VisitorTypeId
			return this.SingleOrDefault(x => x.VisitorTypeId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.VisitorType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///VisitorTypeId
			return Task.FromResult<ECA.Data.VisitorType>(this.SingleOrDefault(x => x.VisitorTypeId.Equals(keyValues.First())));
		}
	}
	public class WebsiteTestDbSet : ECA.Core.Data.TestDbSet<ECA.Data.Website>
	{
		public override ECA.Data.Website Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///WebsiteId
			return this.SingleOrDefault(x => x.WebsiteId.Equals(keyValues.First()));
		}
		public override Task<ECA.Data.Website> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///WebsiteId
			return Task.FromResult<ECA.Data.Website>(this.SingleOrDefault(x => x.WebsiteId.Equals(keyValues.First())));
		}
	}
	public class InMemoryEcaContext : ECA.Data.EcaContext
	{
		public InMemoryEcaContext()
		{
			InitializeDbSets();
			this.SetupActions = new List<Action>();
		}

		public void InitializeDbSets()
		{
			this.Accommodations = new AccommodationTestDbSet();
			this.Activities = new ActivityTestDbSet();
			this.ActivityTypes = new ActivityTypeTestDbSet();
			this.Actors = new ActorTestDbSet();
			this.Addresses = new AddressTestDbSet();
			this.AddressTypes = new AddressTypeTestDbSet();
			this.Artifacts = new ArtifactTestDbSet();
			this.ArtifactTypes = new ArtifactTypeTestDbSet();
			this.BirthCountries = new BirthCountryTestDbSet();
			this.BirthCountryReasons = new BirthCountryReasonTestDbSet();
			this.Bookmarks = new BookmarkTestDbSet();
			this.CancelledSevisBatchProcessings = new CancelledSevisBatchProcessingTestDbSet();
			this.Categories = new CategoryTestDbSet();
			this.Contacts = new ContactTestDbSet();
			this.Courses = new CourseTestDbSet();
			this.DataPointCategoryProperties = new DataPointCategoryPropertyTestDbSet();
			this.DataPointConfigurations = new DataPointConfigurationTestDbSet();
			this.DefaultExchangeVisitorFunding = new DefaultExchangeVisitorFundingTestDbSet();
			this.DependentTypes = new DependentTypeTestDbSet();
			this.EducationLevels = new EducationLevelTestDbSet();
			this.EmailAddresses = new EmailAddressTestDbSet();
			this.EmailAddressTypes = new EmailAddressTypeTestDbSet();
			this.ExternalIds = new ExternalIdTestDbSet();
			this.FieldOfStudies = new FieldOfStudyTestDbSet();
			this.Foci = new FocusTestDbSet();
			this.Genders = new GenderTestDbSet();
			this.Goals = new GoalTestDbSet();
			this.Impacts = new ImpactTestDbSet();
			this.ImpactTypes = new ImpactTypeTestDbSet();
			this.InterestSpecializations = new InterestSpecializationTestDbSet();
			this.InternationalOrganizations = new InternationalOrganizationTestDbSet();
			this.Itineraries = new ItineraryTestDbSet();
			this.ItineraryStops = new ItineraryStopTestDbSet();
			this.Justifications = new JustificationTestDbSet();
			this.Languages = new LanguageTestDbSet();
			this.Locations = new LocationTestDbSet();
			this.LocationTypes = new LocationTypeTestDbSet();
			this.MaritalStatuses = new MaritalStatusTestDbSet();
			this.Materials = new MaterialTestDbSet();
			this.Memberships = new MembershipTestDbSet();
			this.MoneyFlows = new MoneyFlowTestDbSet();
			this.MoneyFlowSourceRecipientTypes = new MoneyFlowSourceRecipientTypeTestDbSet();
			this.MoneyFlowSourceRecipientTypeSettings = new MoneyFlowSourceRecipientTypeSettingTestDbSet();
			this.MoneyFlowStatuses = new MoneyFlowStatusTestDbSet();
			this.MoneyFlowTypes = new MoneyFlowTypeTestDbSet();
			this.Objectives = new ObjectiveTestDbSet();
			this.OfficeSettings = new OfficeSettingTestDbSet();
			this.OrganizationRoles = new OrganizationRoleTestDbSet();
			this.Organizations = new OrganizationTestDbSet();
			this.OrganizationTypes = new OrganizationTypeTestDbSet();
			this.ParticipantExchangeVisitors = new ParticipantExchangeVisitorTestDbSet();
			this.ParticipantPersons = new ParticipantPersonTestDbSet();
			this.ParticipantPersonSevisCommStatuses = new ParticipantPersonSevisCommStatusTestDbSet();
			this.Participants = new ParticipantTestDbSet();
			this.ParticipantStatuses = new ParticipantStatusTestDbSet();
			this.ParticipantTypes = new ParticipantTypeTestDbSet();
			this.People = new PersonTestDbSet();
			this.PersonDependentCitizenCountries = new PersonDependentCitizenCountryTestDbSet();
			this.PersonDependents = new PersonDependentTestDbSet();
			this.PersonEvaluationNotes = new PersonEvaluationNoteTestDbSet();
			this.PersonLanguageProficiencies = new PersonLanguageProficiencyTestDbSet();
			this.PhoneNumbers = new PhoneNumberTestDbSet();
			this.PhoneNumberTypes = new PhoneNumberTypeTestDbSet();
			this.Positions = new PositionTestDbSet();
			this.ProfessionEducations = new ProfessionEducationTestDbSet();
			this.ProgramCategories = new ProgramCategoryTestDbSet();
			this.Programs = new ProgramTestDbSet();
			this.ProgramStatuses = new ProgramStatusTestDbSet();
			this.ProgramTypes = new ProgramTypeTestDbSet();
			this.Projects = new ProjectTestDbSet();
			this.ProjectStatuses = new ProjectStatusTestDbSet();
			this.ProminentCategories = new ProminentCategoryTestDbSet();
			this.Publications = new PublicationTestDbSet();
			this.SevisBatchProcessings = new SevisBatchProcessingTestDbSet();
			this.SevisCommStatuses = new SevisCommStatusTestDbSet();
			this.SocialMedias = new SocialMediaTestDbSet();
			this.SocialMediaTypes = new SocialMediaTypeTestDbSet();
			this.SpecialStatuses = new SpecialStatusTestDbSet();
			this.StudentCreations = new StudentCreationTestDbSet();
			this.Themes = new ThemeTestDbSet();
			this.Transportations = new TransportationTestDbSet();
			this.UserAccounts = new UserAccountTestDbSet();
			this.USGovernmentAgencies = new USGovernmentAgencyTestDbSet();
			this.VisitorTypes = new VisitorTypeTestDbSet();
			this.Websites = new WebsiteTestDbSet();
		}

		public List<Action> SetupActions { get; set; }

		public void Revert()
		{
			InitializeDbSets();
			this.SetupActions.ForEach(x => x());
		}
	}
	#endregion
}
