using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity configuration for the Organization class.
    /// </summary>
    public class OrganizationConfiguration : EntityTypeConfiguration<Organization>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public OrganizationConfiguration()
        {
            Property(a => a.OfficeSymbol).IsOptional().HasMaxLength(Organization.OFFICE_SYMBOL_MAX_LENGTH);
            Property(a => a.ParentOrganizationId).HasColumnName("ParentOrganization_OrganizationId");
            Property(x => x.Website).IsOptional().HasMaxLength(Organization.WEBSITE_MAX_LENGTH);
            Property(x => x.Description).IsRequired().HasMaxLength(Organization.DESCRIPTION_MAX_LENGTH);
            Property(x => x.Name).IsRequired().HasMaxLength(Organization.NAME_MAX_LENGTH);

            HasMany<Contact>(p => p.Contacts).WithMany(t => t.Organizations)
            .Map(p =>
            {
                p.MapLeftKey("OrganizationId");
                p.MapRightKey("ContactId");
                p.ToTable("OrganizationContact");
            });

            HasMany<Focus>(p => p.Foci).WithRequired(t => t.Office).HasForeignKey(p => p.OfficeId);
            HasMany<Justification>(p => p.Justifications).WithRequired(t => t.Office).HasForeignKey(p => p.OfficeId);
            HasMany<OfficeSetting>(p => p.OfficeSettings).WithRequired(t => t.Office).HasForeignKey(p => p.OfficeId);
            HasOptional(x => x.ParentOrganization).WithMany().HasForeignKey(x => x.ParentOrganizationId);
        }
    }
}
