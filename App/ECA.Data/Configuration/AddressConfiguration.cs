using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The EntityTypeConfiguration for the Address entity.
    /// </summary>
    public class AddressConfiguration : EntityTypeConfiguration<Address>
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        public AddressConfiguration()
        {
            HasKey(x => x.AddressId);
            HasRequired(x => x.AddressType).WithMany().HasForeignKey(x => x.AddressTypeId).WillCascadeOnDelete(false);
            HasRequired(x => x.Location).WithMany().HasForeignKey(x => x.LocationId).WillCascadeOnDelete(false);  
          
            HasOptional(x => x.Person).WithMany(x => x.Addresses).HasForeignKey(x => x.PersonId).WillCascadeOnDelete(false);
            HasOptional(x => x.Organization).WithMany(x => x.Addresses).HasForeignKey(x => x.OrganizationId).WillCascadeOnDelete(false);

            Property(x => x.DisplayName).IsRequired();
            Property(x => x.OrganizationId).HasColumnName("OrganizationId");
            Property(x => x.PersonId).HasColumnName("PersonId");
        }
    }
}
