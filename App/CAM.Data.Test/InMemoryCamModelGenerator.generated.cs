






namespace CAM.Data.Test
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	#region CamModel
	public class AccountStatusTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.AccountStatus>
	{
		public override CAM.Data.AccountStatus Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AccountStatusId
			return this.SingleOrDefault(x => x.AccountStatusId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.AccountStatus> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///AccountStatusId
			return Task.FromResult<CAM.Data.AccountStatus>(this.SingleOrDefault(x => x.AccountStatusId.Equals(keyValues.First())));
		}
	}
	public class ApplicationTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.Application>
	{
		public override CAM.Data.Application Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ResourceId
			return this.SingleOrDefault(x => x.ResourceId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.Application> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ResourceId
			return Task.FromResult<CAM.Data.Application>(this.SingleOrDefault(x => x.ResourceId.Equals(keyValues.First())));
		}
	}
	public class PermissionAssignmentTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.PermissionAssignment>
	{
		public override CAM.Data.PermissionAssignment Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.PermissionAssignment> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return Task.FromResult<CAM.Data.PermissionAssignment>(this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First())));
		}
	}
	public class PermissionTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.Permission>
	{
		public override CAM.Data.Permission Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PermissionId
			return this.SingleOrDefault(x => x.PermissionId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.Permission> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PermissionId
			return Task.FromResult<CAM.Data.Permission>(this.SingleOrDefault(x => x.PermissionId.Equals(keyValues.First())));
		}
	}
	public class PrincipalRoleTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.PrincipalRole>
	{
		public override CAM.Data.PrincipalRole Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.PrincipalRole> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return Task.FromResult<CAM.Data.PrincipalRole>(this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First())));
		}
	}
	public class PrincipalTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.Principal>
	{
		public override CAM.Data.Principal Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.Principal> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return Task.FromResult<CAM.Data.Principal>(this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First())));
		}
	}
	public class PrincipalTypeTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.PrincipalType>
	{
		public override CAM.Data.PrincipalType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalTypeId
			return this.SingleOrDefault(x => x.PrincipalTypeId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.PrincipalType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalTypeId
			return Task.FromResult<CAM.Data.PrincipalType>(this.SingleOrDefault(x => x.PrincipalTypeId.Equals(keyValues.First())));
		}
	}
	public class ResourceTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.Resource>
	{
		public override CAM.Data.Resource Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ResourceId
			return this.SingleOrDefault(x => x.ResourceId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.Resource> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ResourceId
			return Task.FromResult<CAM.Data.Resource>(this.SingleOrDefault(x => x.ResourceId.Equals(keyValues.First())));
		}
	}
	public class ResourceTypeTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.ResourceType>
	{
		public override CAM.Data.ResourceType Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ResourceTypeId
			return this.SingleOrDefault(x => x.ResourceTypeId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.ResourceType> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///ResourceTypeId
			return Task.FromResult<CAM.Data.ResourceType>(this.SingleOrDefault(x => x.ResourceTypeId.Equals(keyValues.First())));
		}
	}
	public class RoleResourcePermissionTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.RoleResourcePermission>
	{
		public override CAM.Data.RoleResourcePermission Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///RoleId
			return this.SingleOrDefault(x => x.RoleId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.RoleResourcePermission> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///RoleId
			return Task.FromResult<CAM.Data.RoleResourcePermission>(this.SingleOrDefault(x => x.RoleId.Equals(keyValues.First())));
		}
	}
	public class RoleTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.Role>
	{
		public override CAM.Data.Role Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///RoleId
			return this.SingleOrDefault(x => x.RoleId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.Role> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///RoleId
			return Task.FromResult<CAM.Data.Role>(this.SingleOrDefault(x => x.RoleId.Equals(keyValues.First())));
		}
	}
	public class UserAccountTestDbSet : ECA.Core.Data.TestDbSet<CAM.Data.UserAccount>
	{
		public override CAM.Data.UserAccount Find(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First()));
		}
		public override Task<CAM.Data.UserAccount> FindAsync(params object[] keyValues)
		{
			if(keyValues.Length != 1) throw new System.NotSupportedException();
			///PrincipalId
			return Task.FromResult<CAM.Data.UserAccount>(this.SingleOrDefault(x => x.PrincipalId.Equals(keyValues.First())));
		}
	}
	public class InMemoryCamModel : CAM.Data.CamModel
	{
		public InMemoryCamModel()
		{
			InitializeDbSets();
			this.SetupActions = new List<Action>();
		}

		public void InitializeDbSets()
		{
			this.AccountStatus = new AccountStatusTestDbSet();
			this.Applications = new ApplicationTestDbSet();
			this.PermissionAssignments = new PermissionAssignmentTestDbSet();
			this.Permissions = new PermissionTestDbSet();
			this.PrincipalRoles = new PrincipalRoleTestDbSet();
			this.Principals = new PrincipalTestDbSet();
			this.PrincipalTypes = new PrincipalTypeTestDbSet();
			this.Resources = new ResourceTestDbSet();
			this.ResourceTypes = new ResourceTypeTestDbSet();
			this.RoleResourcePermissions = new RoleResourcePermissionTestDbSet();
			this.Roles = new RoleTestDbSet();
			this.UserAccounts = new UserAccountTestDbSet();
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
