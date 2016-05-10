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
    public class FullNameChangeDetail : ChangeDetail
    {
        public FullNameChangeDetail(ComparisonResult result) : base(result)
        {
            Contract.Requires(result != null, "The result must not be null.");            
            this.FirstNameChanged = HasPropertyChanged(nameof(FullName.FirstName), result);
            this.LastNameChanged = HasPropertyChanged(nameof(FullName.LastName), result);
            this.PassportNameChanged = HasPropertyChanged(nameof(FullName.PassportName), result);
            this.PreferredNameChanged = HasPropertyChanged(nameof(FullName.PreferredName), result);
            this.SuffixChanged = HasPropertyChanged(nameof(FullName.Suffix), result);
        }


        public bool FirstNameChanged { get; private set; }

        public bool LastNameChanged { get; private set; }

        public bool PassportNameChanged { get; private set; }

        public bool PreferredNameChanged { get; private set; }

        public bool SuffixChanged { get; private set; }
    }
}
