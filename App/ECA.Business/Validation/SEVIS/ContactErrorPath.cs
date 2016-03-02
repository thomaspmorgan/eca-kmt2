using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS
{
    /// <summary>
    /// The ContactErrorPath is used to direct a client to a pii section to fix sevis validation errors.
    /// </summary>
    public class ContactErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public ContactErrorPath()
        {
            Category = ElementCategory.Person.ToString();
            CategorySub = ElementCategorySub.PersonalInfo.ToString();
            Section = ElementCategorySection.Contact.ToString();
            Tab = ElementCategorySectionTab.PersonalInfo.ToString();
        }
    }
}
