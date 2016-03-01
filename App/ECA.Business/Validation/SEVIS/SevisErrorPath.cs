using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS
{
    public class SevisErrorPath : ErrorPath
    {
        public SevisErrorPath()
        {
            Category = ElementCategory.Project.ToString();
            CategorySub = ElementCategorySub.Participant.ToString();
            Tab = ElementCategorySectionTab.Sevis.ToString();
        }
    }
}
