using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity configuration for the Impace class.
    /// </summary>
    public class ImpactConfiguration : EntityTypeConfiguration<Impact>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ImpactConfiguration()
        {
            HasOptional(e => e.Program).WithMany(e => e.Impacts).WillCascadeOnDelete(false);
            HasOptional(e => e.Project).WithMany(e => e.Impacts).WillCascadeOnDelete(false);
            HasMany<Person>(p => p.People).WithMany(t => t.Impacts).Map(p =>
            {
                p.MapLeftKey("ImpactId");
                p.MapRightKey("PersonId");
                p.ToTable("ImpactPerson");
            });
        }
    }
}
