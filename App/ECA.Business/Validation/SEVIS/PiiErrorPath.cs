using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS
{
    public class PiiErrorPath : ErrorPath
    {
        public PiiErrorPath()
        {
            Category = ElementCategory.Person.ToString();
            CategorySub = ElementCategorySub.PersonalInfo.ToString();
            Section = ElementCategorySection.PII.ToString();
            Tab = ElementCategorySectionTab.PersonalInfo.ToString();
        }
    }
}
