using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An UpdatedEmailAddress is used by a business layer client to update a email address entity.
    /// </summary>
    public class UpdatedEmailAddress
    {
        /// <summary>
        /// Creates a new UpdatedEmailAddress and initialized it with the given values.
        /// </summary>
        /// <param name="updator">The updator.</param>
        /// <param name="id">The id of the email address.</param>
        /// <param name="address">The value.</param>
        /// <param name="emailAddressTypeId">The email address type id</param>
        public UpdatedEmailAddress(User updator, int id, string address, int emailAddressTypeId)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.Update = new Update(updator);
            this.Id = id;
            this.Address = address;
            this.EmailAddressTypeId = emailAddressTypeId;
        }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Update Update { get; private set; }

        /// <summary>
        /// Gets the Id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// gets/sets the email address type id
        /// </summary>
        public int EmailAddressTypeId { get; set; }

        /// <summary>
        /// Gets the value of the Address
        /// </summary>
        public string Address { get; private set; }

    }
}
