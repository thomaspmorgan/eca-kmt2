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
    /// An organization is an entity which participates in the implementation or execution of a project, such as a cooperating organization, a host family or an ECA office.
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// The max length of the office symbol.
        /// </summary>
        public const int OFFICE_SYMBOL_MAX_LENGTH = 128;

        public Organization()
        {
            this.History = new History();
            this.SocialMediaPresence = new HashSet<SocialMedia>();
            this.MoneyFlowSources = new HashSet<MoneyFlow>();
            this.MoneyFlowRecipients = new HashSet<MoneyFlow>();
            this.OwnerPrograms = new HashSet<Program>();
            this.Contacts = new HashSet<Contact>();
            this.Addresses = new HashSet<Address>();

        }

        [Key]
        public int OrganizationId { get; set; }
        [Required]
        public OrganizationType OrganizationType { get; set; }
        public int OrganizationTypeId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Organization ParentOrganization { get; set; }
        public string Status { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the office symbol, i.e. abbreviation of the organization.
        /// </summary>
        public string OfficeSymbol { get; set; }

        public string Website { get; set; }
        public ICollection<SocialMedia> SocialMediaPresence { get; set; }
        //public List<DateTimeOffset> ContactHistory { get; set; }
        //need to add some type of contact history element, most likely off of Contact

        //relationships

        public ICollection<MoneyFlow> MoneyFlowSources { get; set; }
        public ICollection<MoneyFlow> MoneyFlowRecipients { get; set; }
        public ICollection<Program> OwnerPrograms { get; set; }

        public History History { get; set; }
    }

}
