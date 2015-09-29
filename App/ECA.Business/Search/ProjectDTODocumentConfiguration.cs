using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public class ProjectDTODocumentConfiguration : DocumentConfiguration<ProjectDTO, int>
    {
        public ProjectDTODocumentConfiguration()
        {
            IsDocumentType(DocumentType.Project);
            HasKey(x => x.Id);
            HasName(x => x.Name);
            HasDescription(x => x.Description);
            HasOfficeSymbol(x => x.OwnerOfficeSymbol);
            HasPointsOfContact(x => x.Contacts.Select(c => c.Value).ToList());
            HasFoci(x => x.Categories.Select(c => c.Name).ToList());
            HasGoals(x => x.Goals.Select(c => c.Value).ToList());
            HasObjectives(x => x.Objectives.Select(y => y.Name).ToList());
            HasThemes(x => x.Themes.Select(t => t.Value).ToList());
        }
    }
}
