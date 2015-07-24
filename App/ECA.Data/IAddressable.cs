using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// An IAddressable entity is an entity that has zero to many addresses.
    /// </summary>
    public interface IAddressable
    {
        /// <summary>
        /// Gets or sets addresses.
        /// </summary>
        ICollection<Address> Addresses { get; set; }
    }
}
