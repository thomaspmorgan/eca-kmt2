using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A contact is a point-of-communication for an organization.
    /// </summary>
    public class Contact : IHistorical, IEmailAddressable, IPhoneNumberable
    {
        /// <summary>
        /// Gets the max address length.
        /// </summary>
        public const int FULL_NAME_MAX_LENGTH = 100;

        /// <summary>
        /// Gets the max position length.
        /// </summary>
        public const int POSITION_MAX_LENGTH = 100;

        /// <summary>
        /// Creates a new contact and initializes set properties.
        /// </summary>
        public Contact()
        {
            this.EmailAddresses = new HashSet<EmailAddress>();
            this.PhoneNumbers = new HashSet<PhoneNumber>();
            this.Organizations = new HashSet<Organization>();
            this.Programs = new HashSet<Program>();
            this.Projects = new HashSet<Project>();
            this.History = new History();
        }

        /// <summary>
        /// Gets the contact id.
        /// </summary>
        [Key]
        public int ContactId { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [Required]
        [MaxLength(FULL_NAME_MAX_LENGTH)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        [MaxLength(POSITION_MAX_LENGTH)]
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets the phone numbers.
        /// </summary>
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

        /// <summary>
        /// Gets or sets the email addresses.
        /// </summary>
        public virtual ICollection<EmailAddress> EmailAddresses { get; set; }
        
        /// <summary>
        /// Gets or sets the organizations.
        /// </summary>
        public virtual ICollection<Organization> Organizations { get; set; }

        /// <summary>
        /// Gets or sets the programs.
        /// </summary>
        public virtual ICollection<Program> Programs { get; set; }

        /// <summary>
        /// Gets or sets the projects.
        /// </summary>
        public virtual ICollection<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }
    }
}
