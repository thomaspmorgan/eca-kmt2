using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// An email address is used to store point of contact or person email addresses.
    /// </summary>
    public class EmailAddress : IHistorical
    {
        /// <summary>
        /// Gets the max address length.
        /// </summary>
        public const int EMAIL_ADDRESS_MAX_LENGTH = 100;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public EmailAddress()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets or sets the email address id.
        /// </summary>
        [Key]
        public int EmailAddressId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        [MaxLength(EMAIL_ADDRESS_MAX_LENGTH)]
        [EmailAddress]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the email address type id.
        /// </summary>
        [Column("EmailAddressTypeId")]
        public int EmailAddressTypeId { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        [Column("Person_PersonId")]
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the contact id.
        /// </summary>
        [Column("Contact_ContactId")]
        public int? ContactId { get; set; }

        /// <summary>
        /// Gets or sets the email address type.
        /// </summary>
        [ForeignKey("EmailAddressTypeId")]
        public virtual EmailAddressType EmailAddressType { get; set; }

        /// <summary>
        /// Gets or sets the person.
        /// </summary>
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        /// <summary>
        /// Gets or sets the dependent id.
        /// </summary>
        [Column("Dependent_DependentId")]
        public int? DependentId { get; set; }

        /// <summary>
        /// Gets or sets the dependent.
        /// </summary>
        [ForeignKey("DependentId")]
        public virtual PersonDependent Dependent { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets is primary.
        /// </summary>
        public bool? IsPrimary { get; set; }
    }
}
