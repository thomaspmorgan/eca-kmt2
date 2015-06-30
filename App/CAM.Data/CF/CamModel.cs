namespace CAM.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CamModel : DbContext
    {
        public CamModel()
            : base("name=CamModel")
        {
        }

        public virtual DbSet<AccountStatus> AccountStatus { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PermissionAssignment> PermissionAssignments { get; set; }
        public virtual DbSet<Principal> Principals { get; set; }
        public virtual DbSet<PrincipalRole> PrincipalRoles { get; set; }
        public virtual DbSet<PrincipalType> PrincipalTypes { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<ResourceType> ResourceTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleResourcePermission> RoleResourcePermissions { get; set; }
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

            //modelBuilder.Entity<ResourceType>()
            //    .HasMany(e => e.Permissions)
            //    .WithOptional(e => e.ResourceType)
            //    .HasForeignKey(e => e.ParentResourceTypeId);

            //modelBuilder.Entity<ResourceType>()
            //    .HasMany(e => e.Permissions1)
            //    .WithOptional(e => e.ParentResourceType)
            //    .HasForeignKey(e => e.ResourceTypeId);

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
    }
}
