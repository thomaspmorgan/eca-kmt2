




#region AccountStatus
namespace CAM.Data
{
	using ECA.Core.Generation;
	public partial class AccountStatus : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Active lookup with id 1.
		/// </summary>
		public static StaticLookup Active { get { return new StaticLookup("Active", 1); } }
		/// <summary>
		/// Returns the Expired lookup with id 2.
		/// </summary>
		public static StaticLookup Expired { get { return new StaticLookup("Expired", 2); } }
		/// <summary>
		/// Returns the Suspended lookup with id 3.
		/// </summary>
		public static StaticLookup Suspended { get { return new StaticLookup("Suspended", 3); } }
		/// <summary>
		/// Returns the Revoked lookup with id 4.
		/// </summary>
		public static StaticLookup Revoked { get { return new StaticLookup("Revoked", 4); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return AccountStatus.Active;
			if (2 == id) return AccountStatus.Expired;
			if (3 == id) return AccountStatus.Suspended;
			if (4 == id) return AccountStatus.Revoked;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Active".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AccountStatus.Active;
			if ("Expired".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AccountStatus.Expired;
			if ("Suspended".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AccountStatus.Suspended;
			if ("Revoked".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return AccountStatus.Revoked;
			return null;
		}


		/// <summary>
		/// Returns the Active string value.
		/// </summary>
		///Active
		public const string ACTIVE_VALUE = "Active";

		/// <summary>
		/// Returns the 1 integer id value.
		/// </summary>
		///Active
		public const int ACTIVE_ID = 1;

		/// <summary>
		/// Returns the Expired string value.
		/// </summary>
		///Expired
		public const string EXPIRED_VALUE = "Expired";

		/// <summary>
		/// Returns the 2 integer id value.
		/// </summary>
		///Expired
		public const int EXPIRED_ID = 2;

		/// <summary>
		/// Returns the Suspended string value.
		/// </summary>
		///Suspended
		public const string SUSPENDED_VALUE = "Suspended";

		/// <summary>
		/// Returns the 3 integer id value.
		/// </summary>
		///Suspended
		public const int SUSPENDED_ID = 3;

		/// <summary>
		/// Returns the Revoked string value.
		/// </summary>
		///Revoked
		public const string REVOKED_VALUE = "Revoked";

		/// <summary>
		/// Returns the 4 integer id value.
		/// </summary>
		///Revoked
		public const int REVOKED_ID = 4;

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "CAM.Data", ClassName = "AccountStatus", TableName = "cam.AccountStatus", IdColumnName = "AccountStatusId", ValueColumnName = "Status" };
		}
	}
}
#endregion

#region Permission
namespace CAM.Data
{
	using ECA.Core.Generation;
	public partial class Permission : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the View Office lookup with id 1.
		/// </summary>
		public static StaticLookup ViewOffice { get { return new StaticLookup("View Office", 1); } }
		/// <summary>
		/// Returns the View Program lookup with id 2.
		/// </summary>
		public static StaticLookup ViewProgram { get { return new StaticLookup("View Program", 2); } }
		/// <summary>
		/// Returns the View Project lookup with id 3.
		/// </summary>
		public static StaticLookup ViewProject { get { return new StaticLookup("View Project", 3); } }
		/// <summary>
		/// Returns the Edit Office lookup with id 4.
		/// </summary>
		public static StaticLookup EditOffice { get { return new StaticLookup("Edit Office", 4); } }
		/// <summary>
		/// Returns the Edit Program lookup with id 5.
		/// </summary>
		public static StaticLookup EditProgram { get { return new StaticLookup("Edit Program", 5); } }
		/// <summary>
		/// Returns the Edit Project lookup with id 7.
		/// </summary>
		public static StaticLookup EditProject { get { return new StaticLookup("Edit Project", 7); } }
		/// <summary>
		/// Returns the Project Owner lookup with id 8.
		/// </summary>
		public static StaticLookup ProjectOwner { get { return new StaticLookup("Project Owner", 8); } }
		/// <summary>
		/// Returns the Program Owner lookup with id 9.
		/// </summary>
		public static StaticLookup ProgramOwner { get { return new StaticLookup("Program Owner", 9); } }
		/// <summary>
		/// Returns the Office Owner lookup with id 10.
		/// </summary>
		public static StaticLookup OfficeOwner { get { return new StaticLookup("Office Owner", 10); } }
		/// <summary>
		/// Returns the Administrator lookup with id 11.
		/// </summary>
		public static StaticLookup Administrator { get { return new StaticLookup("Administrator", 11); } }
		/// <summary>
		/// Returns the Search lookup with id 12.
		/// </summary>
		public static StaticLookup Search { get { return new StaticLookup("Search", 12); } }
		/// <summary>
		/// Returns the Edit Sevis lookup with id 13.
		/// </summary>
		public static StaticLookup EditSevis { get { return new StaticLookup("Edit Sevis", 13); } }
		/// <summary>
		/// Returns the Send To Sevis lookup with id 14.
		/// </summary>
		public static StaticLookup SendToSevis { get { return new StaticLookup("Send To Sevis", 14); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return Permission.ViewOffice;
			if (2 == id) return Permission.ViewProgram;
			if (3 == id) return Permission.ViewProject;
			if (4 == id) return Permission.EditOffice;
			if (5 == id) return Permission.EditProgram;
			if (7 == id) return Permission.EditProject;
			if (8 == id) return Permission.ProjectOwner;
			if (9 == id) return Permission.ProgramOwner;
			if (10 == id) return Permission.OfficeOwner;
			if (11 == id) return Permission.Administrator;
			if (12 == id) return Permission.Search;
			if (13 == id) return Permission.EditSevis;
			if (14 == id) return Permission.SendToSevis;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("View Office".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.ViewOffice;
			if ("View Program".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.ViewProgram;
			if ("View Project".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.ViewProject;
			if ("Edit Office".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.EditOffice;
			if ("Edit Program".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.EditProgram;
			if ("Edit Project".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.EditProject;
			if ("Project Owner".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.ProjectOwner;
			if ("Program Owner".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.ProgramOwner;
			if ("Office Owner".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.OfficeOwner;
			if ("Administrator".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.Administrator;
			if ("Search".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.Search;
			if ("Edit Sevis".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.EditSevis;
			if ("Send To Sevis".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return Permission.SendToSevis;
			return null;
		}


		/// <summary>
		/// Returns the View Office string value.
		/// </summary>
		///View Office
		public const string VIEW_OFFICE_VALUE = "View Office";

		/// <summary>
		/// Returns the 1 integer id value.
		/// </summary>
		///View Office
		public const int VIEW_OFFICE_ID = 1;

		/// <summary>
		/// Returns the View Program string value.
		/// </summary>
		///View Program
		public const string VIEW_PROGRAM_VALUE = "View Program";

		/// <summary>
		/// Returns the 2 integer id value.
		/// </summary>
		///View Program
		public const int VIEW_PROGRAM_ID = 2;

		/// <summary>
		/// Returns the View Project string value.
		/// </summary>
		///View Project
		public const string VIEW_PROJECT_VALUE = "View Project";

		/// <summary>
		/// Returns the 3 integer id value.
		/// </summary>
		///View Project
		public const int VIEW_PROJECT_ID = 3;

		/// <summary>
		/// Returns the Edit Office string value.
		/// </summary>
		///Edit Office
		public const string EDIT_OFFICE_VALUE = "Edit Office";

		/// <summary>
		/// Returns the 4 integer id value.
		/// </summary>
		///Edit Office
		public const int EDIT_OFFICE_ID = 4;

		/// <summary>
		/// Returns the Edit Program string value.
		/// </summary>
		///Edit Program
		public const string EDIT_PROGRAM_VALUE = "Edit Program";

		/// <summary>
		/// Returns the 5 integer id value.
		/// </summary>
		///Edit Program
		public const int EDIT_PROGRAM_ID = 5;

		/// <summary>
		/// Returns the Edit Project string value.
		/// </summary>
		///Edit Project
		public const string EDIT_PROJECT_VALUE = "Edit Project";

		/// <summary>
		/// Returns the 7 integer id value.
		/// </summary>
		///Edit Project
		public const int EDIT_PROJECT_ID = 7;

		/// <summary>
		/// Returns the Project Owner string value.
		/// </summary>
		///Project Owner
		public const string PROJECT_OWNER_VALUE = "Project Owner";

		/// <summary>
		/// Returns the 8 integer id value.
		/// </summary>
		///Project Owner
		public const int PROJECT_OWNER_ID = 8;

		/// <summary>
		/// Returns the Program Owner string value.
		/// </summary>
		///Program Owner
		public const string PROGRAM_OWNER_VALUE = "Program Owner";

		/// <summary>
		/// Returns the 9 integer id value.
		/// </summary>
		///Program Owner
		public const int PROGRAM_OWNER_ID = 9;

		/// <summary>
		/// Returns the Office Owner string value.
		/// </summary>
		///Office Owner
		public const string OFFICE_OWNER_VALUE = "Office Owner";

		/// <summary>
		/// Returns the 10 integer id value.
		/// </summary>
		///Office Owner
		public const int OFFICE_OWNER_ID = 10;

		/// <summary>
		/// Returns the Administrator string value.
		/// </summary>
		///Administrator
		public const string ADMINISTRATOR_VALUE = "Administrator";

		/// <summary>
		/// Returns the 11 integer id value.
		/// </summary>
		///Administrator
		public const int ADMINISTRATOR_ID = 11;

		/// <summary>
		/// Returns the Search string value.
		/// </summary>
		///Search
		public const string SEARCH_VALUE = "Search";

		/// <summary>
		/// Returns the 12 integer id value.
		/// </summary>
		///Search
		public const int SEARCH_ID = 12;

		/// <summary>
		/// Returns the Edit Sevis string value.
		/// </summary>
		///Edit Sevis
		public const string EDIT_SEVIS_VALUE = "Edit Sevis";

		/// <summary>
		/// Returns the 13 integer id value.
		/// </summary>
		///Edit Sevis
		public const int EDIT_SEVIS_ID = 13;

		/// <summary>
		/// Returns the Send To Sevis string value.
		/// </summary>
		///Send To Sevis
		public const string SEND_TO_SEVIS_VALUE = "Send To Sevis";

		/// <summary>
		/// Returns the 14 integer id value.
		/// </summary>
		///Send To Sevis
		public const int SEND_TO_SEVIS_ID = 14;

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "CAM.Data", ClassName = "Permission", TableName = "cam.Permission", IdColumnName = "PermissionId", ValueColumnName = "PermissionName" };
		}
	}
}
#endregion

#region PrincipalType
namespace CAM.Data
{
	using ECA.Core.Generation;
	public partial class PrincipalType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Person     lookup with id 1.
		/// </summary>
		public static StaticLookup Person { get { return new StaticLookup("Person    ", 1); } }
		/// <summary>
		/// Returns the Group      lookup with id 2.
		/// </summary>
		public static StaticLookup Group { get { return new StaticLookup("Group     ", 2); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return PrincipalType.Person;
			if (2 == id) return PrincipalType.Group;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Person    ".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return PrincipalType.Person;
			if ("Group     ".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return PrincipalType.Group;
			return null;
		}


		/// <summary>
		/// Returns the Person     string value.
		/// </summary>
		///Person    
		public const string PERSON_VALUE = "Person    ";

		/// <summary>
		/// Returns the 1 integer id value.
		/// </summary>
		///Person    
		public const int PERSON_ID = 1;

		/// <summary>
		/// Returns the Group      string value.
		/// </summary>
		///Group     
		public const string GROUP_VALUE = "Group     ";

		/// <summary>
		/// Returns the 2 integer id value.
		/// </summary>
		///Group     
		public const int GROUP_ID = 2;

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "CAM.Data", ClassName = "PrincipalType", TableName = "cam.PrincipalType", IdColumnName = "PrincipalTypeId", ValueColumnName = "PrincipalTypeName" };
		}
	}
}
#endregion

#region ResourceType
namespace CAM.Data
{
	using ECA.Core.Generation;
	public partial class ResourceType : ECA.Core.Generation.IStaticLookup
	{
		/// <summary>
		/// Returns the Application lookup with id 1.
		/// </summary>
		public static StaticLookup Application { get { return new StaticLookup("Application", 1); } }
		/// <summary>
		/// Returns the Office lookup with id 2.
		/// </summary>
		public static StaticLookup Office { get { return new StaticLookup("Office", 2); } }
		/// <summary>
		/// Returns the Program lookup with id 3.
		/// </summary>
		public static StaticLookup Program { get { return new StaticLookup("Program", 3); } }
		/// <summary>
		/// Returns the Project lookup with id 4.
		/// </summary>
		public static StaticLookup Project { get { return new StaticLookup("Project", 4); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return ResourceType.Application;
			if (2 == id) return ResourceType.Office;
			if (3 == id) return ResourceType.Program;
			if (4 == id) return ResourceType.Project;
			return null;
		}
		///<summary>
		/// Returns the lookup value of this entity with the given value, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given value, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(string value)
		{
			if ("Application".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ResourceType.Application;
			if ("Office".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ResourceType.Office;
			if ("Program".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ResourceType.Program;
			if ("Project".Equals(value, System.StringComparison.OrdinalIgnoreCase)) return ResourceType.Project;
			return null;
		}


		/// <summary>
		/// Returns the Application string value.
		/// </summary>
		///Application
		public const string APPLICATION_VALUE = "Application";

		/// <summary>
		/// Returns the 1 integer id value.
		/// </summary>
		///Application
		public const int APPLICATION_ID = 1;

		/// <summary>
		/// Returns the Office string value.
		/// </summary>
		///Office
		public const string OFFICE_VALUE = "Office";

		/// <summary>
		/// Returns the 2 integer id value.
		/// </summary>
		///Office
		public const int OFFICE_ID = 2;

		/// <summary>
		/// Returns the Program string value.
		/// </summary>
		///Program
		public const string PROGRAM_VALUE = "Program";

		/// <summary>
		/// Returns the 3 integer id value.
		/// </summary>
		///Program
		public const int PROGRAM_ID = 3;

		/// <summary>
		/// Returns the Project string value.
		/// </summary>
		///Project
		public const string PROJECT_VALUE = "Project";

		/// <summary>
		/// Returns the 4 integer id value.
		/// </summary>
		///Project
		public const int PROJECT_ID = 4;

		/// <summary>
		/// Returns the static lookup config used to generate this type's static lookups.
		/// <returns>The static lookup config used to generate this type's static lookups.</returns>
		/// </summary>
		public StaticLookupConfig GetConfig()
		{
			return new StaticLookupConfig { Namespace = "CAM.Data", ClassName = "ResourceType", TableName = "cam.ResourceType", IdColumnName = "ResourceTypeId", ValueColumnName = "ResourceTypeName" };
		}
	}
}
#endregion

#region Validator
namespace CAM.Data
{
	using ECA.Core.Generation;
	using System.Collections.Generic;
	///<summary>Validates all classes that have static lookups defined.</summary>
	public static class CamDataValidator
	{
		///<summary>
		/// Validates all static lookup classes with the given validator.
		///<param name="validator">The validator.</param>
		/// <returns>The list of validation errors, or an empty list if no errors are found.</returns>
		///</summary>
		public static List<string> ValidateAll(IStaticGeneratorValidator validator)
		{
			var errors = new List<string>();
			errors.AddRange(validator.Validate<AccountStatus>());
			errors.AddRange(validator.Validate<Permission>());
			errors.AddRange(validator.Validate<PrincipalType>());
			errors.AddRange(validator.Validate<ResourceType>());
			return errors;
		}
	}
}
#endregion
