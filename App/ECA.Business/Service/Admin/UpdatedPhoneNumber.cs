using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An UpdatedPhoneNumber is used by a business layer client to update a phone number entity.
    /// </summary>
    public class UpdatedPhoneNumber : IAuditable
    {
        /// <summary>
        /// Creates a new UpdatedPhoneNumber and initialized it with the given values.
        /// </summary>
        /// <param name="updator">The updator.</param>
        /// <param name="id">The id of the phone number.</param>
        /// <param name="number">The value.</param>
        /// <param name="phoneNumberTypeId">The phone number type id</param>
        /// <param name="isPrimary">The is primary flag.</param>
        public UpdatedPhoneNumber(User updator, int id, string number, string extension, int phoneNumberTypeId, bool isPrimary)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.Audit = new Update(updator);
            this.Id = id;
            this.Number = number;
            this.Extension = extension;
            this.PhoneNumberTypeId = phoneNumberTypeId;
            this.IsPrimary = isPrimary;
        }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Audit Audit { get; private set; }

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

        /// <summary>
        /// Gets or sets the phone number extension.
        /// </summary>
        public string Extension { get; private set; }

        /// <summary>
        /// Gets the is primary flag.
        /// </summary>
        public bool IsPrimary { get; private set; }

    }
}
