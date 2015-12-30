using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITest.Core
{
    public class AddFundConfig
    {
        public bool IsOutgoing { get; set; }
        public int SourceRecipientTypeID { get; set; }
        public string SourceRecipientTypeName { get; set; }
        public int FiscalYear { get; set; }
        public int Date { get; set; }
            

    }
}
