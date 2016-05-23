using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// Holds information on whether or not a dependent has changed.
    /// </summary>
    public class DependentChangeDetail : ChangeDetail
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="result">The comparison result.</param>
        public DependentChangeDetail(ComparisonResult result) : base(result)
        {

        }
    }
}
