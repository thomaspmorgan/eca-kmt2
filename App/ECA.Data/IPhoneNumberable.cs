using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// An IPhoneNumberable entity is an entity that has phone numbers
    /// </summary>
    public interface IPhoneNumberable
    {
        /// <summary>
        /// Gets or sets the phone numbers of this entity.
        /// </summary>
        ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}