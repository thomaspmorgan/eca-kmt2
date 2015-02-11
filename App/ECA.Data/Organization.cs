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
