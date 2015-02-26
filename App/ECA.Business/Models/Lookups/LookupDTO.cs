using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Models.Lookups
{
    public class LookupDTO : ILookup
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string LookupType { get; set; }
    }
}
