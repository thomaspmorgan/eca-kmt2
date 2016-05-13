using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// Holds information on whether or not an exchange visitor has changed.
    /// </summary>
    public class ExchangeVisitorChangeDetail : ChangeDetail
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="result">The comparison result.</param>
        public ExchangeVisitorChangeDetail(ComparisonResult result) : base(result)
        {

        }
    }
}
