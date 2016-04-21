using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    public class SevisOrgId
    {
        private string sevisOrgId;

        public SevisOrgId(string sevisOrgId)
        {
            this.sevisOrgId = sevisOrgId;
        }

        public override string ToString()
        {
            return this.sevisOrgId;
        }
    }
}
