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
    /// An UpdatedPhoneNumber is used by a business layer client to update a phone number entity.
    /// </summary>
    public class UpdatedPhoneNumber
    {
        /// <summary>
        /// Creates a new UpdatedPhoneNumber and initialized it with the given values.
        /// </summary>
        /// <param name="updator">The updator.</param>
        /// <param name="id">The id of the phone number.</param>
        /// <param name="number">The value.</param>
        /// <param name="phoneNumberTypeId">The phone number type id</param>
        public UpdatedPhoneNumber(User updator, int id, string number, int phoneNumberTypeId)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.Update = new Update(updator);
            this.Id = id;
            this.Number = number;
            this.PhoneNumberTypeId = phoneNumberTypeId;
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
        /// gets/sets the phone number type id
        /// </summary>
        public int PhoneNumberTypeId { get; set; }

        /// <summary>
        /// Gets the value of the Number
        /// </summary>
        public string Number { get; private set; }

    }
}
