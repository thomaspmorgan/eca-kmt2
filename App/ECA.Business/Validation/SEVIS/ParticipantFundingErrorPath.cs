using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS
{
    public class ParticipantFundingErrorPath : ErrorPath
    {
        public ParticipantFundingErrorPath()
        {
            Category = ElementCategory.Project.ToString();
            CategorySub = ElementCategorySub.Participant.ToString();
            Tab = ElementCategorySectionTab.Funding.ToString();
        }
    }
}
