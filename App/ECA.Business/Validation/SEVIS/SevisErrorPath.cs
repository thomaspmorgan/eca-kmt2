using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS
{
    /// <summary>
    /// The SevisErrorPath is used to direct a client to a sevis tab section to fix sevis validation errors.
    /// </summary>
    public class SevisErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public SevisErrorPath()
        {
            Category = ElementCategory.Project.ToString();
            CategorySub = ElementCategorySub.Participant.ToString();
            Tab = ElementCategorySectionTab.Sevis.ToString();
        }
    }
}
