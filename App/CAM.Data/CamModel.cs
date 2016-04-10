namespace CAM.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// The CamModel is the entity framework dbcontext model for CAM.
    /// </summary>
    public partial class CamModel : DbContext
    {
        /// <summary>
        /// The key for retrieving the context in an IValidatatableObject instance.
        /// </summary>
        public const string VALIDATABLE_CONTEXT_KEY = "Context";

        /// <summary>
        /// Creates a new CamModel instance with the connection string or connection string key.
        /// </summary>
        /// <param name="connectionStringOrKey">The connection string or connection string key.</param>
        public CamModel(string connectionStringOrKey)
            : base(connectionStringOrKey)
        {

        }

        /// <summary>
        /// Creates a new instance and initializes with the connection string named CamModel.
        /// </summary>
        public CamModel()
            : base("name=CamModel")
        {
        }

        /// <summary>
        /// Gets or sets the account statuses.
        /// </summary>
        public virtual DbSet<AccountStatus> AccountStatuses { get; set; }

        /// <summary>
        /// Gets or sets the applications.
        /// </summary>
        public virtual DbSet<Application> Applications { get; set; }

        /// <summary>
        /// Gets or sets the sevis accounts.
        /// </summary>
        public virtual DbSet<SevisAccount> SevisAccounts { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        public virtual DbSet<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the permission assignments.
        /// </summary>
        public virtual DbSet<PermissionAssignment> PermissionAssignments { get; set; }

        /// <summary>
        /// Gets or sets the principals.
        /// </summary>
        public virtual DbSet<Principal> Principals { get; set; }

        /// <summary>
        /// Gets or sets the principal roles.
        /// </summary>
        public virtual DbSet<PrincipalRole> PrincipalRoles { get; set; }

        /// <summary>
        /// Gets or sets the principal types.
        /// </summary>
        public virtual DbSet<PrincipalType> PrincipalTypes { get; set; }

        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        public virtual DbSet<Resource> Resources { get; set; }

        /// <summary>
        /// Get or sets the resource types.
        /// </summary>
        public virtual DbSet<ResourceType> ResourceTypes { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public virtual DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the role resource permissions.
        /// </summary>
        public virtual DbSet<RoleResourcePermission> RoleResourcePermissions { get; set; }

        /// <summary>
        /// Gets or sets the user accounts.
        /// </summary>
        public virtual DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountStatus>()
                .HasMany(e => e.UserAccounts)
                .WithRequired(e => e.AccountStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Permission>()
                .Property(e => e.PermissionDescription)
                .IsFixedLength();

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.PermissionAssignments)
                .WithRequired(e => e.Permission)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.RoleResourcePermissions)
                .WithRequired(e => e.Permission)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Permission>().Property(x => x.ParentResourceTypeId).HasColumnName("ParentResourceTypeId");
            modelBuilder.Entity<Permission>().Property(x => x.ResourceTypeId).HasColumnName("ResourceTypeId");
            modelBuilder.Entity<Permission>().HasOptional(x => x.ResourceType).WithMany().HasForeignKey(x => x.ResourceTypeId);
            modelBuilder.Entity<Permission>().HasOptional(x => x.ParentResourceType).WithMany().HasForeignKey(x => x.ParentResourceTypeId);

            modelBuilder.Entity<Principal>()
                .HasMany(e => e.PermissionAssignments)
                .WithRequired(e => e.Principal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Principal>()
                .HasMany(e => e.PrincipalRoles)
                .WithRequired(e => e.Principal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Principal>()
                .HasOptional(e => e.UserAccount)
                .WithRequired(e => e.Principal);

            modelBuilder.Entity<Principal>()
                .HasMany(e => e.SevisAccounts)
                .WithRequired(e => e.Principal);

            modelBuilder.Entity<PrincipalType>()
                .Property(e => e.PrincipalTypeName)
                .IsFixedLength();

            modelBuilder.Entity<Resource>()
                .HasOptional(e => e.Application)
                .WithRequired(e => e.Resource);

            modelBuilder.Entity<Resource>()
                .HasMany(e => e.PermissionAssignments)
                .WithRequired(e => e.Resource)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resource>()
                .HasMany(e => e.ChildResources)
                .WithOptional(e => e.ParentResource)
                .HasForeignKey(e => e.ParentResourceId);

            modelBuilder.Entity<Resource>()
                .HasMany(e => e.RoleResourcePermissions)
                .WithRequired(e => e.Resource)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ResourceType>()
                .HasMany(e => e.Resources)
                .WithRequired(e => e.ResourceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ResourceType>()
                .HasMany(e => e.ChildResourceTypes)
                .WithOptional(e => e.ParentResourceType)
                .HasForeignKey(e => e.ParentResourceTypeId);
            modelBuilder.Entity<ResourceType>().Property(x => x.ParentResourceTypeId).HasColumnName("ParentResourceTypeId");

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleDescription)
                .IsFixedLength();

            modelBuilder.Entity<Role>()
                .HasMany(e => e.PrincipalRoles)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.RoleResourcePermissions)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

        }


        /// <summary>
        /// The ValidateEntity method override that addes this context to instance to the validation items.
        /// </summary>
        /// <param name="entityEntry">The entity entry to validate.</param>
        /// <param name="items">The items that will contain the DbContext.</param>
        /// <returns>The validation results.</returns>
        protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            items.Add(VALIDATABLE_CONTEXT_KEY, this);
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
