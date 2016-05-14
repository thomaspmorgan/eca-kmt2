using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// Holds information on whether or not financial info has changed.
    /// </summary>
    public class FinancialInfoChangeDetail : ChangeDetail
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="result">The comparison result.</param>
        public FinancialInfoChangeDetail(ComparisonResult result) : base(result)
        {

        }
    }
}
