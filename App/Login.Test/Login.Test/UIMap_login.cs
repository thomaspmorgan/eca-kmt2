using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Test
{
    public partial class UIMap
    {
        public bool IsUsernameCached()
        {
            HtmlTable uIEcatest1_statedept_uTable = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument.UIEcatest1_statedept_uTable;
            //do a check
            return uIEcatest1_statedept_uTable != null;
        }
    }
}
