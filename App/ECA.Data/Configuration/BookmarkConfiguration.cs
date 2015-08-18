using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity configuration for the bookmark class
    /// </summary>
    public class BookmarkConfiguration : EntityTypeConfiguration<Bookmark>
    {
        /// <summary>
        /// Creates a new instance of a bookmark configuration
        /// </summary>
        public BookmarkConfiguration()
        {
            HasKey(x => x.BookmarkId);

            HasOptional(x => x.Office).WithMany().HasForeignKey(x => x.OfficeId).WillCascadeOnDelete(false);
            Property(x => x.OfficeId).HasColumnName("OfficeId");

            HasOptional(x => x.Program).WithMany().HasForeignKey(x => x.ProgramId).WillCascadeOnDelete(false);
            Property(x => x.ProgramId).HasColumnName("ProgramId");

            HasOptional(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).WillCascadeOnDelete(false);
            Property(x => x.ProjectId).HasColumnName("ProjectId");

            HasOptional(x => x.Person).WithMany().HasForeignKey(x => x.PersonId).WillCascadeOnDelete(false);
            Property(x => x.PersonId).HasColumnName("PersonId");

            HasOptional(x => x.Organization).WithMany().HasForeignKey(x => x.OrganizationId).WillCascadeOnDelete(false);
            Property(x => x.OrganizationId).HasColumnName("OrganizationId");
        }
    }
}