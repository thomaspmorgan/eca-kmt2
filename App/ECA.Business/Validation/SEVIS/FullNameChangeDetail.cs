using ECA.Business.Validation.Sevis.Bio;
using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// Contains information on whether or not a full name has changed.
    /// </summary>
    public class FullNameChangeDetail : ChangeDetail
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="result">The change reuslt.</param>
        public FullNameChangeDetail(ComparisonResult result) : base(result)
        {
            Contract.Requires(result != null, "The result must not be null."); 
        }        
    }
}
