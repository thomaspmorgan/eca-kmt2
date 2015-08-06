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
    public class BookmarkConfiguration : EntityTypeConfiguration<Bookmark>
    {
        /// <summary>
        /// Creates a new instance of a Bookmark Configuration
        /// </summary>
        public BookmarkConfiguration()
        {
            HasOptional(x => x.Office).WithMany().HasForeignKey(x => x.OfficeId);
            HasRequired(x => x.User).WithMany().HasForeignKey(x => x.PrincipalId);
        }
    }
}