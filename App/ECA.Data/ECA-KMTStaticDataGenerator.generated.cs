




#region ActorType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ActorType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Person lookup with id 1.
		/// </summary>
		public static StaticLookup Person { get { return new StaticLookup("Person", 1); } }
		/// <summary>
		/// Returns the Organization lookup with id 2.
		/// </summary>
		public static StaticLookup Organization { get { return new StaticLookup("Organization", 2); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return ActorType.Person;
			if (2 == id) return ActorType.Organization;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Person".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ActorType.Person;
			if ("Organization".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ActorType.Organization;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "ActorType", TableName = "ActorType", IdColumnName = "ActorTypeId", ValueColumnName = "ActorName" };
		}
	}
}
#endregion

#region AddressType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class AddressType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Home lookup with id 1.
		/// </summary>
		public static StaticLookup Home { get { return new StaticLookup("Home", 1); } }
		/// <summary>
		/// Returns the Host lookup with id 2.
		/// </summary>
		public static StaticLookup Host { get { return new StaticLookup("Host", 2); } }
		/// <summary>
		/// Returns the Business lookup with id 3.
		/// </summary>
		public static StaticLookup Business { get { return new StaticLookup("Business", 3); } }
		/// <summary>
		/// Returns the Organization lookup with id 4.
		/// </summary>
		public static StaticLookup Organization { get { return new StaticLookup("Organization", 4); } }
		/// <summary>
		/// Returns the Country lookup with id 5.
		/// </summary>
		public static StaticLookup Country { get { return new StaticLookup("Country", 5); } }
		/// <summary>
		/// Returns the Provider Implementation Location lookup with id 6.
		/// </summary>
		public static StaticLookup ProviderImplementationLocation { get { return new StaticLookup("Provider Implementation Location", 6); } }
		/// <summary>
		/// Returns the Visiting lookup with id 7.
		/// </summary>
		public static StaticLookup Visiting { get { return new StaticLookup("Visiting", 7); } }
		/// <summary>
		/// Returns the Undetermined lookup with id 8.
		/// </summary>
		public static StaticLookup Undetermined { get { return new StaticLookup("Undetermined", 8); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return AddressType.Home;
			if (2 == id) return AddressType.Host;
			if (3 == id) return AddressType.Business;
			if (4 == id) return AddressType.Organization;
			if (5 == id) return AddressType.Country;
			if (6 == id) return AddressType.ProviderImplementationLocation;
			if (7 == id) return AddressType.Visiting;
			if (8 == id) return AddressType.Undetermined;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Home".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AddressType.Home;
			if ("Host".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AddressType.Host;
			if ("Business".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AddressType.Business;
			if ("Organization".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AddressType.Organization;
			if ("Country".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AddressType.Country;
			if ("Provider Implementation Location".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AddressType.ProviderImplementationLocation;
			if ("Visiting".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AddressType.Visiting;
			if ("Undetermined".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AddressType.Undetermined;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "AddressType", TableName = "AddressType", IdColumnName = "AddressTypeId", ValueColumnName = "AddressName" };
		}
	}
}
#endregion

#region Gender
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class Gender : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Male lookup with id 1.
		/// </summary>
		public static StaticLookup Male { get { return new StaticLookup("Male", 1); } }
		/// <summary>
		/// Returns the Female lookup with id 2.
		/// </summary>
		public static StaticLookup Female { get { return new StaticLookup("Female", 2); } }
		/// <summary>
		/// Returns the Other lookup with id 3.
		/// </summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 3); } }
		/// <summary>
		/// Returns the Not Specified lookup with id 4.
		/// </summary>
		public static StaticLookup NotSpecified { get { return new StaticLookup("Not Specified", 4); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return Gender.Male;
			if (2 == id) return Gender.Female;
			if (3 == id) return Gender.Other;
			if (4 == id) return Gender.NotSpecified;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Male".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Gender.Male;
			if ("Female".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Gender.Female;
			if ("Other".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Gender.Other;
			if ("Not Specified".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Gender.NotSpecified;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "Gender", TableName = "Gender", IdColumnName = "GenderId", ValueColumnName = "GenderName" };
		}
	}
}
#endregion

#region ItineraryStatus
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ItineraryStatus : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Planned lookup with id 1.
		/// </summary>
		public static StaticLookup Planned { get { return new StaticLookup("Planned", 1); } }
		/// <summary>
		/// Returns the InProgress lookup with id 2.
		/// </summary>
		public static StaticLookup Inprogress { get { return new StaticLookup("InProgress", 2); } }
		/// <summary>
		/// Returns the Completed lookup with id 3.
		/// </summary>
		public static StaticLookup Completed { get { return new StaticLookup("Completed", 3); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return ItineraryStatus.Planned;
			if (2 == id) return ItineraryStatus.Inprogress;
			if (3 == id) return ItineraryStatus.Completed;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Planned".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ItineraryStatus.Planned;
			if ("InProgress".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ItineraryStatus.Inprogress;
			if ("Completed".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ItineraryStatus.Completed;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "ItineraryStatus", TableName = "ItineraryStatus", IdColumnName = "ItineraryStatusId", ValueColumnName = "ItineraryStatusName" };
		}
	}
}
#endregion

#region LocationType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class LocationType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Region lookup with id 2.
		/// </summary>
		public static StaticLookup Region { get { return new StaticLookup("Region", 2); } }
		/// <summary>
		/// Returns the Country lookup with id 3.
		/// </summary>
		public static StaticLookup Country { get { return new StaticLookup("Country", 3); } }
		/// <summary>
		/// Returns the State lookup with id 4.
		/// </summary>
		public static StaticLookup State { get { return new StaticLookup("State", 4); } }
		/// <summary>
		/// Returns the City lookup with id 5.
		/// </summary>
		public static StaticLookup City { get { return new StaticLookup("City", 5); } }
		/// <summary>
		/// Returns the Building lookup with id 6.
		/// </summary>
		public static StaticLookup Building { get { return new StaticLookup("Building", 6); } }
		/// <summary>
		/// Returns the Post lookup with id 7.
		/// </summary>
		public static StaticLookup Post { get { return new StaticLookup("Post", 7); } }
		/// <summary>
		/// Returns the Place lookup with id 8.
		/// </summary>
		public static StaticLookup Place { get { return new StaticLookup("Place", 8); } }
		/// <summary>
		/// Returns the Address lookup with id 9.
		/// </summary>
		public static StaticLookup Address { get { return new StaticLookup("Address", 9); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (2 == id) return LocationType.Region;
			if (3 == id) return LocationType.Country;
			if (4 == id) return LocationType.State;
			if (5 == id) return LocationType.City;
			if (6 == id) return LocationType.Building;
			if (7 == id) return LocationType.Post;
			if (8 == id) return LocationType.Place;
			if (9 == id) return LocationType.Address;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Region".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return LocationType.Region;
			if ("Country".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return LocationType.Country;
			if ("State".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return LocationType.State;
			if ("City".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return LocationType.City;
			if ("Building".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return LocationType.Building;
			if ("Post".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return LocationType.Post;
			if ("Place".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return LocationType.Place;
			if ("Address".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return LocationType.Address;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "LocationType", TableName = "LocationType", IdColumnName = "LocationTypeId", ValueColumnName = "LocationTypeName" };
		}
	}
}
#endregion

#region MaritalStatus
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class MaritalStatus : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Married              lookup with id 1.
		/// </summary>
		public static StaticLookup Married { get { return new StaticLookup("Married             ", 1); } }
		/// <summary>
		/// Returns the Unmarried            lookup with id 2.
		/// </summary>
		public static StaticLookup Unmarried { get { return new StaticLookup("Unmarried           ", 2); } }
		/// <summary>
		/// Returns the Divorced             lookup with id 3.
		/// </summary>
		public static StaticLookup Divorced { get { return new StaticLookup("Divorced            ", 3); } }
		/// <summary>
		/// Returns the Single               lookup with id 4.
		/// </summary>
		public static StaticLookup Single { get { return new StaticLookup("Single              ", 4); } }
		/// <summary>
		/// Returns the Separated            lookup with id 5.
		/// </summary>
		public static StaticLookup Separated { get { return new StaticLookup("Separated           ", 5); } }
		/// <summary>
		/// Returns the Widowed              lookup with id 6.
		/// </summary>
		public static StaticLookup Widowed { get { return new StaticLookup("Widowed             ", 6); } }
		/// <summary>
		/// Returns the Not Disclosed        lookup with id 7.
		/// </summary>
		public static StaticLookup NotDisclosed { get { return new StaticLookup("Not Disclosed       ", 7); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return MaritalStatus.Married;
			if (2 == id) return MaritalStatus.Unmarried;
			if (3 == id) return MaritalStatus.Divorced;
			if (4 == id) return MaritalStatus.Single;
			if (5 == id) return MaritalStatus.Separated;
			if (6 == id) return MaritalStatus.Widowed;
			if (7 == id) return MaritalStatus.NotDisclosed;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Married             ".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MaritalStatus.Married;
			if ("Unmarried           ".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MaritalStatus.Unmarried;
			if ("Divorced            ".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MaritalStatus.Divorced;
			if ("Single              ".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MaritalStatus.Single;
			if ("Separated           ".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MaritalStatus.Separated;
			if ("Widowed             ".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MaritalStatus.Widowed;
			if ("Not Disclosed       ".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MaritalStatus.NotDisclosed;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "MaritalStatus", TableName = "MaritalStatus", IdColumnName = "MaritalStatusId", ValueColumnName = "Description" };
		}
	}
}
#endregion

#region MoneyFlowSourceRecipientType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class MoneyFlowSourceRecipientType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Organization lookup with id 1.
		/// </summary>
		public static StaticLookup Organization { get { return new StaticLookup("Organization", 1); } }
		/// <summary>
		/// Returns the Program lookup with id 2.
		/// </summary>
		public static StaticLookup Program { get { return new StaticLookup("Program", 2); } }
		/// <summary>
		/// Returns the Project lookup with id 3.
		/// </summary>
		public static StaticLookup Project { get { return new StaticLookup("Project", 3); } }
		/// <summary>
		/// Returns the Participant lookup with id 4.
		/// </summary>
		public static StaticLookup Participant { get { return new StaticLookup("Participant", 4); } }
		/// <summary>
		/// Returns the ItineraryStop lookup with id 5.
		/// </summary>
		public static StaticLookup Itinerarystop { get { return new StaticLookup("ItineraryStop", 5); } }
		/// <summary>
		/// Returns the Accomodation lookup with id 6.
		/// </summary>
		public static StaticLookup Accomodation { get { return new StaticLookup("Accomodation", 6); } }
		/// <summary>
		/// Returns the Transportation lookup with id 7.
		/// </summary>
		public static StaticLookup Transportation { get { return new StaticLookup("Transportation", 7); } }
		/// <summary>
		/// Returns the Expense lookup with id 8.
		/// </summary>
		public static StaticLookup Expense { get { return new StaticLookup("Expense", 8); } }
		/// <summary>
		/// Returns the Post lookup with id 9.
		/// </summary>
		public static StaticLookup Post { get { return new StaticLookup("Post", 9); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return MoneyFlowSourceRecipientType.Organization;
			if (2 == id) return MoneyFlowSourceRecipientType.Program;
			if (3 == id) return MoneyFlowSourceRecipientType.Project;
			if (4 == id) return MoneyFlowSourceRecipientType.Participant;
			if (5 == id) return MoneyFlowSourceRecipientType.Itinerarystop;
			if (6 == id) return MoneyFlowSourceRecipientType.Accomodation;
			if (7 == id) return MoneyFlowSourceRecipientType.Transportation;
			if (8 == id) return MoneyFlowSourceRecipientType.Expense;
			if (9 == id) return MoneyFlowSourceRecipientType.Post;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Organization".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowSourceRecipientType.Organization;
			if ("Program".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowSourceRecipientType.Program;
			if ("Project".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowSourceRecipientType.Project;
			if ("Participant".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowSourceRecipientType.Participant;
			if ("ItineraryStop".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowSourceRecipientType.Itinerarystop;
			if ("Accomodation".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowSourceRecipientType.Accomodation;
			if ("Transportation".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowSourceRecipientType.Transportation;
			if ("Expense".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowSourceRecipientType.Expense;
			if ("Post".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowSourceRecipientType.Post;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "MoneyFlowSourceRecipientType", TableName = "MoneyFlowSourceRecipientType", IdColumnName = "MoneyFlowSourceRecipientTypeId", ValueColumnName = "TypeName" };
		}
	}
}
#endregion

#region MoneyFlowStatus
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class MoneyFlowStatus : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Budgeted lookup with id 1.
		/// </summary>
		public static StaticLookup Budgeted { get { return new StaticLookup("Budgeted", 1); } }
		/// <summary>
		/// Returns the Appropriated lookup with id 3.
		/// </summary>
		public static StaticLookup Appropriated { get { return new StaticLookup("Appropriated", 3); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return MoneyFlowStatus.Budgeted;
			if (3 == id) return MoneyFlowStatus.Appropriated;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Budgeted".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowStatus.Budgeted;
			if ("Appropriated".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowStatus.Appropriated;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "MoneyFlowStatus", TableName = "MoneyFlowStatus", IdColumnName = "MoneyFlowStatusId", ValueColumnName = "MoneyFlowStatusName" };
		}
	}
}
#endregion

#region MoneyFlowType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class MoneyFlowType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Incoming lookup with id 1.
		/// </summary>
		public static StaticLookup Incoming { get { return new StaticLookup("Incoming", 1); } }
		/// <summary>
		/// Returns the Outgoing lookup with id 2.
		/// </summary>
		public static StaticLookup Outgoing { get { return new StaticLookup("Outgoing", 2); } }
		/// <summary>
		/// Returns the Internal lookup with id 3.
		/// </summary>
		public static StaticLookup Internal { get { return new StaticLookup("Internal", 3); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return MoneyFlowType.Incoming;
			if (2 == id) return MoneyFlowType.Outgoing;
			if (3 == id) return MoneyFlowType.Internal;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Incoming".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowType.Incoming;
			if ("Outgoing".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowType.Outgoing;
			if ("Internal".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return MoneyFlowType.Internal;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "MoneyFlowType", TableName = "MoneyFlowType", IdColumnName = "MoneyFlowTypeId", ValueColumnName = "MoneyFlowTypeName" };
		}
	}
}
#endregion

#region OrganizationType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class OrganizationType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Office lookup with id 1.
		/// </summary>
		public static StaticLookup Office { get { return new StaticLookup("Office", 1); } }
		/// <summary>
		/// Returns the Branch lookup with id 2.
		/// </summary>
		public static StaticLookup Branch { get { return new StaticLookup("Branch", 2); } }
		/// <summary>
		/// Returns the Division lookup with id 3.
		/// </summary>
		public static StaticLookup Division { get { return new StaticLookup("Division", 3); } }
		/// <summary>
		/// Returns the Foreign Educational Institution lookup with id 4.
		/// </summary>
		public static StaticLookup ForeignEducationalInstitution { get { return new StaticLookup("Foreign Educational Institution", 4); } }
		/// <summary>
		/// Returns the Foreign Government lookup with id 5.
		/// </summary>
		public static StaticLookup ForeignGovernment { get { return new StaticLookup("Foreign Government", 5); } }
		/// <summary>
		/// Returns the Foreign NGO/PVO lookup with id 6.
		/// </summary>
		public static StaticLookup ForeignNgoPvo { get { return new StaticLookup("Foreign NGO/PVO", 6); } }
		/// <summary>
		/// Returns the Other lookup with id 7.
		/// </summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 7); } }
		/// <summary>
		/// Returns the Public International Organization (PIO) lookup with id 8.
		/// </summary>
		public static StaticLookup PublicInternationalOrganizationPio { get { return new StaticLookup("Public International Organization (PIO)", 8); } }
		/// <summary>
		/// Returns the U.S. Educational Institution lookup with id 9.
		/// </summary>
		public static StaticLookup USEducationalInstitution { get { return new StaticLookup("U.S. Educational Institution", 9); } }
		/// <summary>
		/// Returns the U.S. Non-Profit Organization (501(c)(3)) lookup with id 10.
		/// </summary>
		public static StaticLookup USNonProfitOrganization501C3 { get { return new StaticLookup("U.S. Non-Profit Organization (501(c)(3))", 10); } }
		/// <summary>
		/// Returns the Individual lookup with id 11.
		/// </summary>
		public static StaticLookup Individual { get { return new StaticLookup("Individual", 11); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return OrganizationType.Office;
			if (2 == id) return OrganizationType.Branch;
			if (3 == id) return OrganizationType.Division;
			if (4 == id) return OrganizationType.ForeignEducationalInstitution;
			if (5 == id) return OrganizationType.ForeignGovernment;
			if (6 == id) return OrganizationType.ForeignNgoPvo;
			if (7 == id) return OrganizationType.Other;
			if (8 == id) return OrganizationType.PublicInternationalOrganizationPio;
			if (9 == id) return OrganizationType.USEducationalInstitution;
			if (10 == id) return OrganizationType.USNonProfitOrganization501C3;
			if (11 == id) return OrganizationType.Individual;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Office".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.Office;
			if ("Branch".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.Branch;
			if ("Division".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.Division;
			if ("Foreign Educational Institution".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.ForeignEducationalInstitution;
			if ("Foreign Government".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.ForeignGovernment;
			if ("Foreign NGO/PVO".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.ForeignNgoPvo;
			if ("Other".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.Other;
			if ("Public International Organization (PIO)".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.PublicInternationalOrganizationPio;
			if ("U.S. Educational Institution".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.USEducationalInstitution;
			if ("U.S. Non-Profit Organization (501(c)(3))".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.USNonProfitOrganization501C3;
			if ("Individual".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return OrganizationType.Individual;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "OrganizationType", TableName = "OrganizationType", IdColumnName = "OrganizationTypeId", ValueColumnName = "OrganizationTypeName" };
		}
	}
}
#endregion

#region ParticipantType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ParticipantType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Foreign Educational Institution lookup with id 1.
		/// </summary>
		public static StaticLookup ForeignEducationalInstitution { get { return new StaticLookup("Foreign Educational Institution", 1); } }
		/// <summary>
		/// Returns the Foreign Government lookup with id 2.
		/// </summary>
		public static StaticLookup ForeignGovernment { get { return new StaticLookup("Foreign Government", 2); } }
		/// <summary>
		/// Returns the U.S. Educational Institution lookup with id 3.
		/// </summary>
		public static StaticLookup USEducationalInstitution { get { return new StaticLookup("U.S. Educational Institution", 3); } }
		/// <summary>
		/// Returns the Public International Organization (PIO) lookup with id 4.
		/// </summary>
		public static StaticLookup PublicInternationalOrganizationPio { get { return new StaticLookup("Public International Organization (PIO)", 4); } }
		/// <summary>
		/// Returns the U.S. Non-Profit Organization (501(c)(3)) lookup with id 5.
		/// </summary>
		public static StaticLookup USNonProfitOrganization501C3 { get { return new StaticLookup("U.S. Non-Profit Organization (501(c)(3))", 5); } }
		/// <summary>
		/// Returns the Individual lookup with id 6.
		/// </summary>
		public static StaticLookup Individual { get { return new StaticLookup("Individual", 6); } }
		/// <summary>
		/// Returns the Foreign NGO/PVO lookup with id 7.
		/// </summary>
		public static StaticLookup ForeignNgoPvo { get { return new StaticLookup("Foreign NGO/PVO", 7); } }
		/// <summary>
		/// Returns the Other lookup with id 8.
		/// </summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 8); } }
		/// <summary>
		/// Returns the Foreign Non Traveling Participant lookup with id 9.
		/// </summary>
		public static StaticLookup ForeignNonTravelingParticipant { get { return new StaticLookup("Foreign Non Traveling Participant", 9); } }
		/// <summary>
		/// Returns the U.S. Non Traveling Participant lookup with id 10.
		/// </summary>
		public static StaticLookup USNonTravelingParticipant { get { return new StaticLookup("U.S. Non Traveling Participant", 10); } }
		/// <summary>
		/// Returns the Foreign Traveling Participant lookup with id 11.
		/// </summary>
		public static StaticLookup ForeignTravelingParticipant { get { return new StaticLookup("Foreign Traveling Participant", 11); } }
		/// <summary>
		/// Returns the U.S. Traveling Participant lookup with id 12.
		/// </summary>
		public static StaticLookup USTravelingParticipant { get { return new StaticLookup("U.S. Traveling Participant", 12); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return ParticipantType.ForeignEducationalInstitution;
			if (2 == id) return ParticipantType.ForeignGovernment;
			if (3 == id) return ParticipantType.USEducationalInstitution;
			if (4 == id) return ParticipantType.PublicInternationalOrganizationPio;
			if (5 == id) return ParticipantType.USNonProfitOrganization501C3;
			if (6 == id) return ParticipantType.Individual;
			if (7 == id) return ParticipantType.ForeignNgoPvo;
			if (8 == id) return ParticipantType.Other;
			if (9 == id) return ParticipantType.ForeignNonTravelingParticipant;
			if (10 == id) return ParticipantType.USNonTravelingParticipant;
			if (11 == id) return ParticipantType.ForeignTravelingParticipant;
			if (12 == id) return ParticipantType.USTravelingParticipant;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Foreign Educational Institution".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.ForeignEducationalInstitution;
			if ("Foreign Government".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.ForeignGovernment;
			if ("U.S. Educational Institution".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.USEducationalInstitution;
			if ("Public International Organization (PIO)".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.PublicInternationalOrganizationPio;
			if ("U.S. Non-Profit Organization (501(c)(3))".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.USNonProfitOrganization501C3;
			if ("Individual".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.Individual;
			if ("Foreign NGO/PVO".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.ForeignNgoPvo;
			if ("Other".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.Other;
			if ("Foreign Non Traveling Participant".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.ForeignNonTravelingParticipant;
			if ("U.S. Non Traveling Participant".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.USNonTravelingParticipant;
			if ("Foreign Traveling Participant".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.ForeignTravelingParticipant;
			if ("U.S. Traveling Participant".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ParticipantType.USTravelingParticipant;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "ParticipantType", TableName = "ParticipantType", IdColumnName = "ParticipantTypeId", ValueColumnName = "Name" };
		}
	}
}
#endregion

#region PhoneNumberType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class PhoneNumberType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Home lookup with id 1.
		/// </summary>
		public static StaticLookup Home { get { return new StaticLookup("Home", 1); } }
		/// <summary>
		/// Returns the Work lookup with id 2.
		/// </summary>
		public static StaticLookup Work { get { return new StaticLookup("Work", 2); } }
		/// <summary>
		/// Returns the Cell lookup with id 3.
		/// </summary>
		public static StaticLookup Cell { get { return new StaticLookup("Cell", 3); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return PhoneNumberType.Home;
			if (2 == id) return PhoneNumberType.Work;
			if (3 == id) return PhoneNumberType.Cell;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Home".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return PhoneNumberType.Home;
			if ("Work".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return PhoneNumberType.Work;
			if ("Cell".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return PhoneNumberType.Cell;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "PhoneNumberType", TableName = "PhoneNumberType", IdColumnName = "PhoneNumberTypeId", ValueColumnName = "PhoneNumberTypeName" };
		}
	}
}
#endregion

#region ProgramStatus
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ProgramStatus : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Active lookup with id 1.
		/// </summary>
		public static StaticLookup Active { get { return new StaticLookup("Active", 1); } }
		/// <summary>
		/// Returns the Pending lookup with id 2.
		/// </summary>
		public static StaticLookup Pending { get { return new StaticLookup("Pending", 2); } }
		/// <summary>
		/// Returns the Completed lookup with id 3.
		/// </summary>
		public static StaticLookup Completed { get { return new StaticLookup("Completed", 3); } }
		/// <summary>
		/// Returns the Draft lookup with id 4.
		/// </summary>
		public static StaticLookup Draft { get { return new StaticLookup("Draft", 4); } }
		/// <summary>
		/// Returns the Canceled lookup with id 5.
		/// </summary>
		public static StaticLookup Canceled { get { return new StaticLookup("Canceled", 5); } }
		/// <summary>
		/// Returns the Other lookup with id 6.
		/// </summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 6); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return ProgramStatus.Active;
			if (2 == id) return ProgramStatus.Pending;
			if (3 == id) return ProgramStatus.Completed;
			if (4 == id) return ProgramStatus.Draft;
			if (5 == id) return ProgramStatus.Canceled;
			if (6 == id) return ProgramStatus.Other;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Active".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProgramStatus.Active;
			if ("Pending".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProgramStatus.Pending;
			if ("Completed".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProgramStatus.Completed;
			if ("Draft".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProgramStatus.Draft;
			if ("Canceled".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProgramStatus.Canceled;
			if ("Other".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProgramStatus.Other;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "ProgramStatus", TableName = "ProgramStatus", IdColumnName = "ProgramStatusId", ValueColumnName = "Status" };
		}
	}
}
#endregion

#region ProgramType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ProgramType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Program lookup with id 1.
		/// </summary>
		public static StaticLookup Program { get { return new StaticLookup("Program", 1); } }
		/// <summary>
		/// Returns the Office lookup with id 2.
		/// </summary>
		public static StaticLookup Office { get { return new StaticLookup("Office", 2); } }
		/// <summary>
		/// Returns the Branch lookup with id 3.
		/// </summary>
		public static StaticLookup Branch { get { return new StaticLookup("Branch", 3); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return ProgramType.Program;
			if (2 == id) return ProgramType.Office;
			if (3 == id) return ProgramType.Branch;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Program".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProgramType.Program;
			if ("Office".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProgramType.Office;
			if ("Branch".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProgramType.Branch;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "ProgramType", TableName = "ProgramType", IdColumnName = "ProgramTypeId", ValueColumnName = "ProgramTypeName" };
		}
	}
}
#endregion

#region ProjectStatus
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ProjectStatus : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Active lookup with id 1.
		/// </summary>
		public static StaticLookup Active { get { return new StaticLookup("Active", 1); } }
		/// <summary>
		/// Returns the Pending lookup with id 2.
		/// </summary>
		public static StaticLookup Pending { get { return new StaticLookup("Pending", 2); } }
		/// <summary>
		/// Returns the Completed lookup with id 4.
		/// </summary>
		public static StaticLookup Completed { get { return new StaticLookup("Completed", 4); } }
		/// <summary>
		/// Returns the Draft lookup with id 5.
		/// </summary>
		public static StaticLookup Draft { get { return new StaticLookup("Draft", 5); } }
		/// <summary>
		/// Returns the Canceled lookup with id 6.
		/// </summary>
		public static StaticLookup Canceled { get { return new StaticLookup("Canceled", 6); } }
		/// <summary>
		/// Returns the Other lookup with id 7.
		/// </summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 7); } }
		/// <summary>
		/// Returns the Active - Use Actuals lookup with id 8.
		/// </summary>
		public static StaticLookup ActiveUseActuals { get { return new StaticLookup("Active - Use Actuals", 8); } }
		/// <summary>
		/// Returns the Project Postponed lookup with id 9.
		/// </summary>
		public static StaticLookup ProjectPostponed { get { return new StaticLookup("Project Postponed", 9); } }
		/// <summary>
		/// Returns the Proposed lookup with id 10.
		/// </summary>
		public static StaticLookup Proposed { get { return new StaticLookup("Proposed", 10); } }
		/// <summary>
		/// Returns the Rejected lookup with id 11.
		/// </summary>
		public static StaticLookup Rejected { get { return new StaticLookup("Rejected", 11); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return ProjectStatus.Active;
			if (2 == id) return ProjectStatus.Pending;
			if (4 == id) return ProjectStatus.Completed;
			if (5 == id) return ProjectStatus.Draft;
			if (6 == id) return ProjectStatus.Canceled;
			if (7 == id) return ProjectStatus.Other;
			if (8 == id) return ProjectStatus.ActiveUseActuals;
			if (9 == id) return ProjectStatus.ProjectPostponed;
			if (10 == id) return ProjectStatus.Proposed;
			if (11 == id) return ProjectStatus.Rejected;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Active".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.Active;
			if ("Pending".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.Pending;
			if ("Completed".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.Completed;
			if ("Draft".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.Draft;
			if ("Canceled".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.Canceled;
			if ("Other".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.Other;
			if ("Active - Use Actuals".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.ActiveUseActuals;
			if ("Project Postponed".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.ProjectPostponed;
			if ("Proposed".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.Proposed;
			if ("Rejected".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ProjectStatus.Rejected;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "ProjectStatus", TableName = "ProjectStatus", IdColumnName = "ProjectStatusId", ValueColumnName = "Status" };
		}
	}
}
#endregion

#region SocialMediaType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class SocialMediaType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Facebook lookup with id 1.
		/// </summary>
		public static StaticLookup Facebook { get { return new StaticLookup("Facebook", 1); } }
		/// <summary>
		/// Returns the LinkedIn lookup with id 2.
		/// </summary>
		public static StaticLookup Linkedin { get { return new StaticLookup("LinkedIn", 2); } }
		/// <summary>
		/// Returns the Twitter lookup with id 3.
		/// </summary>
		public static StaticLookup Twitter { get { return new StaticLookup("Twitter", 3); } }
		/// <summary>
		/// Returns the Weibo lookup with id 4.
		/// </summary>
		public static StaticLookup Weibo { get { return new StaticLookup("Weibo", 4); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return SocialMediaType.Facebook;
			if (2 == id) return SocialMediaType.Linkedin;
			if (3 == id) return SocialMediaType.Twitter;
			if (4 == id) return SocialMediaType.Weibo;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Facebook".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return SocialMediaType.Facebook;
			if ("LinkedIn".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return SocialMediaType.Linkedin;
			if ("Twitter".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return SocialMediaType.Twitter;
			if ("Weibo".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return SocialMediaType.Weibo;
			return null;
		}

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "ECA.Data", ClassName = "SocialMediaType", TableName = "SocialMediaType", IdColumnName = "SocialMediaTypeId", ValueColumnName = "SocialMediaTypeName" };
		}
	}
}
#endregion

#region Validator
namespace ECA.Data
{
	using ECA.Core.Generation;
	using System.Collections.Generic;
	///<summary>Validates all classes that have static lookups defined.</summary>
	public static class EcaDataValidator
	{
		///<summary>
		/// Validates all static lookup classes with the given validator.
		///<param name="validator">The validator.</param>
		/// <returns>The list of validation errors, or an empty list if no errors are found.</returns>
		///</summary>
		public static List<string> ValidateAll(IStaticGeneratorValidator validator)
		{
			var errors = new List<string>();
			errors.AddRange(validator.Validate<ActorType>());
			errors.AddRange(validator.Validate<AddressType>());
			errors.AddRange(validator.Validate<Gender>());
			errors.AddRange(validator.Validate<ItineraryStatus>());
			errors.AddRange(validator.Validate<LocationType>());
			errors.AddRange(validator.Validate<MaritalStatus>());
			errors.AddRange(validator.Validate<MoneyFlowSourceRecipientType>());
			errors.AddRange(validator.Validate<MoneyFlowStatus>());
			errors.AddRange(validator.Validate<MoneyFlowType>());
			errors.AddRange(validator.Validate<OrganizationType>());
			errors.AddRange(validator.Validate<ParticipantType>());
			errors.AddRange(validator.Validate<PhoneNumberType>());
			errors.AddRange(validator.Validate<ProgramStatus>());
			errors.AddRange(validator.Validate<ProgramType>());
			errors.AddRange(validator.Validate<ProjectStatus>());
			errors.AddRange(validator.Validate<SocialMediaType>());
			return errors;
		}
	}
}
#endregion
