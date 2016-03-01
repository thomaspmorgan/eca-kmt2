using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS
{
    public class PersonMoneyFlowErrorPath : ErrorPath
    {
        public PersonMoneyFlowErrorPath()
        {
            Category = ElementCategory.Person.ToString();
            CategorySub = ElementCategorySub.MoneyFlows.ToString();
        }
    }
}
