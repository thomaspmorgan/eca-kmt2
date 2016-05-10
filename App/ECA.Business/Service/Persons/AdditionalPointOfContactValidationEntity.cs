
namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An AdditionalPointOfContactValidationEntity is used to validate a new point of contact.
    /// </summary>
    public class AdditionalPointOfContactValidationEntity
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="fullName">The full name of the point of the contact.</param>
        /// <param name="position">The position of the point of contact.</param>
        /// <param name="likeEmailAddressCount">The number of email addresses that are equal to the point of contact's email address.</param>
        /// <param name="numberOfPrimaryEmailAddresses">The number of primary email addresses.</param>
        /// <param name="numberOfPrimaryPhoneNumbers">The number of primary phone numbers.</param>
        public AdditionalPointOfContactValidationEntity(string fullName, string position, int numberOfPrimaryEmailAddresses, int numberOfPrimaryPhoneNumbers)
        {
            this.FullName = fullName;
            this.Position = position;
            this.NumberOfPrimaryEmailAddresses = numberOfPrimaryEmailAddresses;
            this.NumberOfPrimaryPhoneNumbers = numberOfPrimaryPhoneNumbers;
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public string Position { get; private set; }
        
        /// <summary>
        /// Gets the number of primary email addresses.
        /// </summary>
        public int NumberOfPrimaryEmailAddresses { get; private set; }

        /// <summary>
        /// Gets the number of primary phone numbers.
        /// </summary>
        public int NumberOfPrimaryPhoneNumbers { get; private set; }
    }
}
