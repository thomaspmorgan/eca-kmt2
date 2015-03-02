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
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Position { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public ICollection<EmailAddress> EmailAddresses { get; set; }
        // Relationships
        public ICollection<Organization> Organizations { get; set; }
        public ICollection<Program> Programs { get; set; }
        public ICollection<Project> Projects { get; set; }

        public History History { get; set; }

    }
}
