﻿




#region ActorType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ActorType
	{
		///<summary>
		/// Returns the Person lookup with id 1.
		///</summary>
		public static StaticLookup Person { get { return new StaticLookup("Person", 1); } }
		///<summary>
		/// Returns the Organization lookup with id 2.
		///</summary>
		public static StaticLookup Organization { get { return new StaticLookup("Organization", 2); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(1 == id) return ActorType.Person;
			if(2 == id) return ActorType.Organization;
			return null;
		}
	}
}
#endregion

#region Gender
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class Gender
	{
		///<summary>
		/// Returns the Male lookup with id 1.
		///</summary>
		public static StaticLookup Male { get { return new StaticLookup("Male", 1); } }
		///<summary>
		/// Returns the Female lookup with id 2.
		///</summary>
		public static StaticLookup Female { get { return new StaticLookup("Female", 2); } }
		///<summary>
		/// Returns the Other lookup with id 3.
		///</summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 3); } }
		///<summary>
		/// Returns the NotSpecified lookup with id 4.
		///</summary>
		public static StaticLookup Notspecified { get { return new StaticLookup("NotSpecified", 4); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(1 == id) return Gender.Male;
			if(2 == id) return Gender.Female;
			if(3 == id) return Gender.Other;
			if(4 == id) return Gender.Notspecified;
			return null;
		}
	}
}
#endregion

#region ItineraryStatus
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ItineraryStatus
	{

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			return null;
		}
	}
}
#endregion

#region LocationType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class LocationType
	{
		///<summary>
		/// Returns the Region lookup with id 2.
		///</summary>
		public static StaticLookup Region { get { return new StaticLookup("Region", 2); } }
		///<summary>
		/// Returns the Country lookup with id 3.
		///</summary>
		public static StaticLookup Country { get { return new StaticLookup("Country", 3); } }
		///<summary>
		/// Returns the State lookup with id 4.
		///</summary>
		public static StaticLookup State { get { return new StaticLookup("State", 4); } }
		///<summary>
		/// Returns the City lookup with id 5.
		///</summary>
		public static StaticLookup City { get { return new StaticLookup("City", 5); } }
		///<summary>
		/// Returns the Building lookup with id 6.
		///</summary>
		public static StaticLookup Building { get { return new StaticLookup("Building", 6); } }
		///<summary>
		/// Returns the Post lookup with id 7.
		///</summary>
		public static StaticLookup Post { get { return new StaticLookup("Post", 7); } }
		///<summary>
		/// Returns the Place lookup with id 8.
		///</summary>
		public static StaticLookup Place { get { return new StaticLookup("Place", 8); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(2 == id) return LocationType.Region;
			if(3 == id) return LocationType.Country;
			if(4 == id) return LocationType.State;
			if(5 == id) return LocationType.City;
			if(6 == id) return LocationType.Building;
			if(7 == id) return LocationType.Post;
			if(8 == id) return LocationType.Place;
			return null;
		}
	}
}
#endregion

#region MoneyFlowSourceRecipientType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class MoneyFlowSourceRecipientType
	{

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			return null;
		}
	}
}
#endregion

#region MoneyFlowType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class MoneyFlowType
	{

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			return null;
		}
	}
}
#endregion

#region NameType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class NameType
	{
		///<summary>
		/// Returns the LastName lookup with id 1.
		///</summary>
		public static StaticLookup Lastname { get { return new StaticLookup("LastName", 1); } }
		///<summary>
		/// Returns the FirstName lookup with id 2.
		///</summary>
		public static StaticLookup Firstname { get { return new StaticLookup("FirstName", 2); } }
		///<summary>
		/// Returns the Prefix lookup with id 3.
		///</summary>
		public static StaticLookup Prefix { get { return new StaticLookup("Prefix", 3); } }
		///<summary>
		/// Returns the Suffix lookup with id 4.
		///</summary>
		public static StaticLookup Suffix { get { return new StaticLookup("Suffix", 4); } }
		///<summary>
		/// Returns the GivenName lookup with id 5.
		///</summary>
		public static StaticLookup Givenname { get { return new StaticLookup("GivenName", 5); } }
		///<summary>
		/// Returns the FamilyName lookup with id 6.
		///</summary>
		public static StaticLookup Familyname { get { return new StaticLookup("FamilyName", 6); } }
		///<summary>
		/// Returns the MiddleName lookup with id 7.
		///</summary>
		public static StaticLookup Middlename { get { return new StaticLookup("MiddleName", 7); } }
		///<summary>
		/// Returns the Patronym lookup with id 8.
		///</summary>
		public static StaticLookup Patronym { get { return new StaticLookup("Patronym", 8); } }
		///<summary>
		/// Returns the Alias lookup with id 9.
		///</summary>
		public static StaticLookup Alias { get { return new StaticLookup("Alias", 9); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(1 == id) return NameType.Lastname;
			if(2 == id) return NameType.Firstname;
			if(3 == id) return NameType.Prefix;
			if(4 == id) return NameType.Suffix;
			if(5 == id) return NameType.Givenname;
			if(6 == id) return NameType.Familyname;
			if(7 == id) return NameType.Middlename;
			if(8 == id) return NameType.Patronym;
			if(9 == id) return NameType.Alias;
			return null;
		}
	}
}
#endregion

#region ParticipantType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ParticipantType
	{
		///<summary>
		/// Returns the Foreign Educational Institution lookup with id 1.
		///</summary>
		public static StaticLookup ForeignEducationalInstitution { get { return new StaticLookup("Foreign Educational Institution", 1); } }
		///<summary>
		/// Returns the Foreign Government lookup with id 2.
		///</summary>
		public static StaticLookup ForeignGovernment { get { return new StaticLookup("Foreign Government", 2); } }
		///<summary>
		/// Returns the U.S. Educational Institution lookup with id 3.
		///</summary>
		public static StaticLookup USEducationalInstitution { get { return new StaticLookup("U.S. Educational Institution", 3); } }
		///<summary>
		/// Returns the Public International Organization (PIO) lookup with id 4.
		///</summary>
		public static StaticLookup PublicInternationalOrganizationPio { get { return new StaticLookup("Public International Organization (PIO)", 4); } }
		///<summary>
		/// Returns the U.S. Non-Profit Organization (501(c)(3)) lookup with id 5.
		///</summary>
		public static StaticLookup USNonProfitOrganization501C3 { get { return new StaticLookup("U.S. Non-Profit Organization (501(c)(3))", 5); } }
		///<summary>
		/// Returns the Individual lookup with id 6.
		///</summary>
		public static StaticLookup Individual { get { return new StaticLookup("Individual", 6); } }
		///<summary>
		/// Returns the Foreign NGO/PVO lookup with id 7.
		///</summary>
		public static StaticLookup ForeignNgoPvo { get { return new StaticLookup("Foreign NGO/PVO", 7); } }
		///<summary>
		/// Returns the Other lookup with id 8.
		///</summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 8); } }
		///<summary>
		/// Returns the Marc lookup with id 9.
		///</summary>
		public static StaticLookup Marc { get { return new StaticLookup("Marc", 9); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(1 == id) return ParticipantType.ForeignEducationalInstitution;
			if(2 == id) return ParticipantType.ForeignGovernment;
			if(3 == id) return ParticipantType.USEducationalInstitution;
			if(4 == id) return ParticipantType.PublicInternationalOrganizationPio;
			if(5 == id) return ParticipantType.USNonProfitOrganization501C3;
			if(6 == id) return ParticipantType.Individual;
			if(7 == id) return ParticipantType.ForeignNgoPvo;
			if(8 == id) return ParticipantType.Other;
			if(9 == id) return ParticipantType.Marc;
			return null;
		}
	}
}
#endregion

#region PhoneNumberType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class PhoneNumberType
	{
		///<summary>
		/// Returns the Home lookup with id 1.
		///</summary>
		public static StaticLookup Home { get { return new StaticLookup("Home", 1); } }
		///<summary>
		/// Returns the Work lookup with id 2.
		///</summary>
		public static StaticLookup Work { get { return new StaticLookup("Work", 2); } }
		///<summary>
		/// Returns the Cell lookup with id 3.
		///</summary>
		public static StaticLookup Cell { get { return new StaticLookup("Cell", 3); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(1 == id) return PhoneNumberType.Home;
			if(2 == id) return PhoneNumberType.Work;
			if(3 == id) return PhoneNumberType.Cell;
			return null;
		}
	}
}
#endregion

#region ProgramStatus
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ProgramStatus
	{
		///<summary>
		/// Returns the Active lookup with id 1.
		///</summary>
		public static StaticLookup Active { get { return new StaticLookup("Active", 1); } }
		///<summary>
		/// Returns the Pending lookup with id 2.
		///</summary>
		public static StaticLookup Pending { get { return new StaticLookup("Pending", 2); } }
		///<summary>
		/// Returns the Completed lookup with id 3.
		///</summary>
		public static StaticLookup Completed { get { return new StaticLookup("Completed", 3); } }
		///<summary>
		/// Returns the Draft lookup with id 4.
		///</summary>
		public static StaticLookup Draft { get { return new StaticLookup("Draft", 4); } }
		///<summary>
		/// Returns the Canceled lookup with id 5.
		///</summary>
		public static StaticLookup Canceled { get { return new StaticLookup("Canceled", 5); } }
		///<summary>
		/// Returns the Other lookup with id 6.
		///</summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 6); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(1 == id) return ProgramStatus.Active;
			if(2 == id) return ProgramStatus.Pending;
			if(3 == id) return ProgramStatus.Completed;
			if(4 == id) return ProgramStatus.Draft;
			if(5 == id) return ProgramStatus.Canceled;
			if(6 == id) return ProgramStatus.Other;
			return null;
		}
	}
}
#endregion

#region ProgramType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ProgramType
	{
		///<summary>
		/// Returns the Program lookup with id 1.
		///</summary>
		public static StaticLookup Program { get { return new StaticLookup("Program", 1); } }
		///<summary>
		/// Returns the Office lookup with id 2.
		///</summary>
		public static StaticLookup Office { get { return new StaticLookup("Office", 2); } }
		///<summary>
		/// Returns the Branch lookup with id 3.
		///</summary>
		public static StaticLookup Branch { get { return new StaticLookup("Branch", 3); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(1 == id) return ProgramType.Program;
			if(2 == id) return ProgramType.Office;
			if(3 == id) return ProgramType.Branch;
			return null;
		}
	}
}
#endregion

#region ProjectStatus
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ProjectStatus
	{
		///<summary>
		/// Returns the Active lookup with id 1.
		///</summary>
		public static StaticLookup Active { get { return new StaticLookup("Active", 1); } }
		///<summary>
		/// Returns the Pending lookup with id 2.
		///</summary>
		public static StaticLookup Pending { get { return new StaticLookup("Pending", 2); } }
		///<summary>
		/// Returns the Completed lookup with id 4.
		///</summary>
		public static StaticLookup Completed { get { return new StaticLookup("Completed", 4); } }
		///<summary>
		/// Returns the Draft lookup with id 5.
		///</summary>
		public static StaticLookup Draft { get { return new StaticLookup("Draft", 5); } }
		///<summary>
		/// Returns the Canceled lookup with id 6.
		///</summary>
		public static StaticLookup Canceled { get { return new StaticLookup("Canceled", 6); } }
		///<summary>
		/// Returns the Other lookup with id 7.
		///</summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 7); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(1 == id) return ProjectStatus.Active;
			if(2 == id) return ProjectStatus.Pending;
			if(4 == id) return ProjectStatus.Completed;
			if(5 == id) return ProjectStatus.Draft;
			if(6 == id) return ProjectStatus.Canceled;
			if(7 == id) return ProjectStatus.Other;
			return null;
		}
	}
}
#endregion

#region SocialMediaType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class SocialMediaType
	{
		///<summary>
		/// Returns the Facebook lookup with id 1.
		///</summary>
		public static StaticLookup Facebook { get { return new StaticLookup("Facebook", 1); } }
		///<summary>
		/// Returns the LinkedIn lookup with id 2.
		///</summary>
		public static StaticLookup Linkedin { get { return new StaticLookup("LinkedIn", 2); } }
		///<summary>
		/// Returns the Twitter lookup with id 3.
		///</summary>
		public static StaticLookup Twitter { get { return new StaticLookup("Twitter", 3); } }
		///<summary>
		/// Returns the Weibo lookup with id 4.
		///</summary>
		public static StaticLookup Weibo { get { return new StaticLookup("Weibo", 4); } }
		///<summary>
		/// Returns the brian lookup with id 5.
		///</summary>
		public static StaticLookup Brian { get { return new StaticLookup("brian", 5); } }

		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if(1 == id) return SocialMediaType.Facebook;
			if(2 == id) return SocialMediaType.Linkedin;
			if(3 == id) return SocialMediaType.Twitter;
			if(4 == id) return SocialMediaType.Weibo;
			if(5 == id) return SocialMediaType.Brian;
			return null;
		}
	}
}
#endregion

