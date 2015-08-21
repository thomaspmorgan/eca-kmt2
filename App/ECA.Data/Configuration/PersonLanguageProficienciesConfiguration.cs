using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    class PersonLanguageProficiencyConfiguration : EntityTypeConfiguration<PersonLanguageProficiency>
    {
        public PersonLanguageProficiencyConfiguration()
        {
            HasRequired(e => e.Language).WithMany(e => e.LanguageProficiencies).HasForeignKey(x => x.LanguageId).WillCascadeOnDelete(false);
            HasRequired(a => a.Person).WithMany(e => e.LanguageProficiencies).HasForeignKey(x => x.PersonId).WillCascadeOnDelete(false);
        }
    }
}
