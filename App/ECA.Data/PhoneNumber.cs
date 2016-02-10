using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class PhoneNumber : IHistorical
    {
        public PhoneNumber()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public int PhoneNumberId { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [Phone]
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the phone number type.
        /// </summary>
        [ForeignKey("PhoneNumberTypeId")]
        public virtual PhoneNumberType PhoneNumberType { get; set; }

        /// <summary>
        /// Gets or sets the phone number type id.
        /// </summary>
        [Column("PhoneNumberTypeId")]
        public int PhoneNumberTypeId { get; set; }
        
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
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets is primary.
        /// </summary>
        public bool? IsPrimary { get; set; }
    }
}
