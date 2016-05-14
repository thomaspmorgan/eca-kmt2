using ECA.Data;
using System.Linq;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Allows a business layer client to add an email address to a contact.
    /// </summary>
    public class NewContactEmailAddress : NewEmailAddress<Contact>
    {
        /// <summary>
        /// Creates a new email address with the user, type of email address, and address.
        /// </summary>
        /// <param name="user">The user creating the social media presence.</param>
        /// <param name="emailAddressTypeId">The email address type by id.</param>
        /// <param name="address">The value.</param>
        /// <param name="contactId">The person id.</param>
        /// <param name="isPrimary">The is primary flag.</param>
        public NewContactEmailAddress(User user, int emailAddressTypeId, string address, bool isPrimary, int contactId)
            :base(user, emailAddressTypeId, address, isPrimary)
        {
            this.ContactId = contactId;
        }

        /// <summary>
        /// Gets the organization id.
        /// </summary>
        public int ContactId { get; private set; }

        /// <summary>
        /// Returns a query to retrieve email addresses of the person.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The email address query.</returns>
        public override IQueryable<EmailAddress> CreateGetEmailAddressesQuery(EcaContext context)
        {
            return context.EmailAddresses.Where(x => x.ContactId == this.ContactId);
        }

        /// <summary>
        /// Returns the person id.
        /// </summary>
        /// <returns>The person id.</returns>
        public override int GetEmailAddressableEntityId()
        {
            return this.ContactId;
        }

    }
}