using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public class ProgramDTODocumentConfiguration : DocumentConfiguration<ProgramDTO, int>
    {
        public ProgramDTODocumentConfiguration()
        {
            IsDocumentType(DocumentType.Program);
            HasKey(x => x.Id);
            HasName(x => x.Name);
            HasDescription(x => x.Description);
            HasFoci(x => x.Categories.Select(c => c.Name).ToList());
            HasGoals(x => x.Goals.Select(c => c.Value).ToList());
            HasObjectives(x => x.Objectives.Select(y => y.Name).ToList());
            HasThemes(x => x.Themes.Select(t => t.Value).ToList());
        }
    }
}
