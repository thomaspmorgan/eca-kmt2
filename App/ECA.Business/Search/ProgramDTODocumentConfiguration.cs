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
            HasTitle(x => x.Name);
            HasDescription(x => x.Description);
            HasSubtitle(x => x.OwnerOfficeSymbol);

            //HasAdditionalField<FocusCategoryDTO>(x => x.Categories, y => String.Join(", ", y.Select(z => z.FocusName).ToList()));

            //Func<IEnumerable<object>, string> objectiveDelegate = (objectives) =>
            //{
            //    var values = objectives.Select(x => x.Name).ToList();
            //    var value = String.Join(", ", values);
            //    return value;
            //};

            HasAdditionalField(x => x.Objectives, y => String.Join(", ", y.Objectives.Select(o => o.Name).ToList()));
        }
    }
}
