using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
    public class OrganizationDTO
    {
        public int OrganizationId { get; set; }
        public OrganizationTypeDTO OrganizationType { get; set; }
        public int OrganizationTypeId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        //public virtual ICollection<Address> Addresses { get; set; }
        //public virtual ICollection<Contact> Contacts { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        //public virtual ICollection<SocialMedia> SocialMediaPresence { get; set; }
        public List<DateTimeOffset> ContactHistory { get; set; }

        //relationships

        //public virtual ICollection<MoneyFlow> MoneyFlowSources { get; set; }
        //public virtual ICollection<MoneyFlow> MoneyFlowRecipients { get; set; }
        public ICollection<ProgramDTO> OwnerPrograms { get; set; }  
    }
}