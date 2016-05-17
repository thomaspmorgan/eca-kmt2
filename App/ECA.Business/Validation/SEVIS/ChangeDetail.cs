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
    /// A ChangeDetail is an object detailing the changes found between two instances.
    /// </summary>
    public class ChangeDetail
    {
        private bool hasChanges;

        /// <summary>
        /// Creates a new ChangeDetail instance with the boolean value indicating changes.
        /// </summary>
        /// <param name="hasChanges">True if changes have occurred.</param>
        public ChangeDetail(bool hasChanges)
        {
            this.hasChanges = hasChanges;
        }

        /// <summary>
        /// Creates a new instance with the given ComparisonResult.
        /// </summary>
        /// <param name="result">The comparison result.</param>
        public ChangeDetail(ComparisonResult result) : this(!result.AreEqual)
        {
            Contract.Requires(result != null, "The result must not be null.");
        }

        /// <summary>
        /// Returns true, if changes were detected, otherwise false.
        /// </summary>
        /// <returns>True, if changes were detected, otherwise false.</returns>
        public bool HasChanges()
        {
            return this.hasChanges;
        }
    }
}
