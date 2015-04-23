




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
		/// Returns the ViewOffice lookup with id 1.
		/// </summary>
		public static StaticLookup Viewoffice { get { return new StaticLookup("ViewOffice", 1); } }
		/// <summary>
		/// Returns the ViewProgram lookup with id 2.
		/// </summary>
		public static StaticLookup Viewprogram { get { return new StaticLookup("ViewProgram", 2); } }
		/// <summary>
		/// Returns the ViewProject lookup with id 3.
		/// </summary>
		public static StaticLookup Viewproject { get { return new StaticLookup("ViewProject", 3); } }
		/// <summary>
		/// Returns the EditOffice lookup with id 4.
		/// </summary>
		public static StaticLookup Editoffice { get { return new StaticLookup("EditOffice", 4); } }
		/// <summary>
		/// Returns the EditProgram lookup with id 5.
		/// </summary>
		public static StaticLookup Editprogram { get { return new StaticLookup("EditProgram", 5); } }
		/// <summary>
		/// Returns the EditProject lookup with id 7.
		/// </summary>
		public static StaticLookup Editproject { get { return new StaticLookup("EditProject", 7); } }
		///<summary>
		/// Returns the lookup value of this entity with the given id, or null if it does not exist.
		///<param name="id">The lookup id.</param>
		/// <returns>The lookup with the given id, or null if it does not exist.</returns>
		///</summary>
		public static StaticLookup GetStaticLookup(int id)
		{
			if (1 == id) return Permission.Viewoffice;
			if (2 == id) return Permission.Viewprogram;
			if (3 == id) return Permission.Viewproject;
			if (4 == id) return Permission.Editoffice;
			if (5 == id) return Permission.Editprogram;
			if (7 == id) return Permission.Editproject;
			return null;
		}


		/// <summary>
		/// Returns the ViewOffice string value.
		/// </summary>
		///ViewOffice
		public const string VIEW_OFFICE_VALUE = "ViewOffice";

		/// <summary>
		/// Returns the 1 integer id value.
		/// </summary>
		///ViewOffice
		public const int VIEW_OFFICE_ID = 1;

		/// <summary>
		/// Returns the ViewProgram string value.
		/// </summary>
		///ViewProgram
		public const string VIEW_PROGRAM_VALUE = "ViewProgram";

		/// <summary>
		/// Returns the 2 integer id value.
		/// </summary>
		///ViewProgram
		public const int VIEW_PROGRAM_ID = 2;

		/// <summary>
		/// Returns the ViewProject string value.
		/// </summary>
		///ViewProject
		public const string VIEW_PROJECT_VALUE = "ViewProject";

		/// <summary>
		/// Returns the 3 integer id value.
		/// </summary>
		///ViewProject
		public const int VIEW_PROJECT_ID = 3;

		/// <summary>
		/// Returns the EditOffice string value.
		/// </summary>
		///EditOffice
		public const string EDIT_OFFICE_VALUE = "EditOffice";

		/// <summary>
		/// Returns the 4 integer id value.
		/// </summary>
		///EditOffice
		public const int EDIT_OFFICE_ID = 4;

		/// <summary>
		/// Returns the EditProgram string value.
		/// </summary>
		///EditProgram
		public const string EDIT_PROGRAM_VALUE = "EditProgram";

		/// <summary>
		/// Returns the 5 integer id value.
		/// </summary>
		///EditProgram
		public const int EDIT_PROGRAM_ID = 5;

		/// <summary>
		/// Returns the EditProject string value.
		/// </summary>
		///EditProject
		public const string EDIT_PROJECT_VALUE = "EditProject";

		/// <summary>
		/// Returns the 7 integer id value.
		/// </summary>
		///EditProject
		public const int EDIT_PROJECT_ID = 7;

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


		/// <summary>
		/// Returns the Person     string value.
		/// </summary>
		///Person    
		public const string PERSON_____VALUE = "Person    ";

		/// <summary>
		/// Returns the 1 integer id value.
		/// </summary>
		///Person    
		public const int PERSON_____ID = 1;

		/// <summary>
		/// Returns the Group      string value.
		/// </summary>
		///Group     
		public const string GROUP______VALUE = "Group     ";

		/// <summary>
		/// Returns the 2 integer id value.
		/// </summary>
		///Group     
		public const int GROUP______ID = 2;

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
