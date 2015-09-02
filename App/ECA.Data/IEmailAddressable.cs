using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// An ISocialable entity is an entity that has email addresses
    /// </summary>
    public interface IEmailAddressable
    {
        /// <summary>
        /// Gets or sets the email addresses of this entity.
        /// </summary>
        ICollection<EmailAddress> EmailAddresses { get; set; }
    }
}