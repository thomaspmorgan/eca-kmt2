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
            HasMany<Contact>(p => p.Contacts).WithMany(t => t.Organizations)
            .Map(p =>
            {
                p.MapLeftKey("OrganizationId");
                p.MapRightKey("ContactId");
                p.ToTable("OrganizationContact");
            });
        }
    }
}
