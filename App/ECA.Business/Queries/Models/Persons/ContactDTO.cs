using ECA.Business.Queries.Models.Admin;
using System.Collections.Generic;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A ContactDTO is to represent a simplified contact within the eca system.
    /// </summary>
    public class ContactDTO
    {
        public ContactDTO()
        {
            this.EmailAddresses = new List<EmailAddressDTO>();
            this.PhoneNumbers = new List<PhoneNumberDTO>();
            this.EmailAddressValues = new List<string>();
            this.PhoneNumberValues = new List<string>();
            //this.Projects = new List<ProjectDTO>();
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the FullName.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets the email addresses.
        /// </summary>
        public IEnumerable<EmailAddressDTO> EmailAddresses { get; set; }

        /// <summary>
        /// Gets or sets the email address values, useful for filtering.
        /// </summary>
        public IEnumerable<string> EmailAddressValues { get; set; }

        /// <summary>
        /// Gets or sets the phone numbers.
        /// </summary>
        public IEnumerable<PhoneNumberDTO> PhoneNumbers { get; set; }

        /// <summary>
        /// Gets or sets the phone number values, useful for filtering.
        /// </summary>
        public IEnumerable<string> PhoneNumberValues { get; set; }


        //public IEnumerable<ProjectDTO> Projects { get; set; }

    }
}
