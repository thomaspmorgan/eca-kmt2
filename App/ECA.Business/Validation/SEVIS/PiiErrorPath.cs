using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS
{
    /// <summary>
    /// A PiiErrorPath is used to direct a client to the pii section when sevis validation fails.
    /// </summary>
    public class PiiErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public PiiErrorPath()
        {
            Category = ElementCategory.Person.ToString();
            CategorySub = ElementCategorySub.PersonalInfo.ToString();
            Section = ElementCategorySection.PII.ToString();
            Tab = ElementCategorySectionTab.PersonalInfo.ToString();
        }
    }
}
