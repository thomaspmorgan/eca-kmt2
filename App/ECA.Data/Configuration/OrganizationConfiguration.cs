using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class OrganizationConfiguration : EntityTypeConfiguration<Organization>
    {
        public OrganizationConfiguration()
        {
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
