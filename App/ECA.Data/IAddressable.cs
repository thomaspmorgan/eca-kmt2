using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    public interface IAddressable : IIdentifiable
    {
        ICollection<Address> Addresses { get; set; }
    }
}
