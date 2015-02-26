using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Models.Lookups
{
    public interface ILookup
    {
        int Id { get; set; }

        string Value { get; set; }

        string LookupType { get; set; }
    }
}
