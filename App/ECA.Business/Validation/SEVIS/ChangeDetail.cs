using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    public class ChangeDetail
    {
        private bool hasChanges;

        public ChangeDetail(bool hasChanges)
        {
            this.hasChanges = hasChanges;
        }

        public ChangeDetail(ComparisonResult result)
        {
            Contract.Requires(result != null, "The result must not be null.");
            this.hasChanges = !result.AreEqual;
        }

        public bool HasChanges()
        {
            return this.hasChanges;
        }

        public bool HasPropertyChanged(string propertyName, ComparisonResult result)
        {
            return result.Differences.Where(x => x.PropertyName.Contains(propertyName)).FirstOrDefault() != null;
        }
    }
}
