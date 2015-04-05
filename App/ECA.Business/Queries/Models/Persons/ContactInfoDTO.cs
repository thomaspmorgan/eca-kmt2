using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// Contact info data transfer object for a person
    /// </summary>
    public class ContactInfoDTO
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ContactInfoDTO()
        {
            Emails = new List<SimpleLookupDTO>();
            SocialMedias = new List<SimpleTypeLookupDTO>();
            PhoneNumbers = new List<SimpleTypeLookupDTO>();
        }

        /// <summary>
        /// Gets and sets emails
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Emails { get; set; }

        /// <summary>
        /// Gets and sets social medias
        /// </summary>
        public IEnumerable<SimpleTypeLookupDTO> SocialMedias { get; set; }

        /// <summary>
        /// Gets and set phone numbers
        /// </summary>
        public IEnumerable<SimpleTypeLookupDTO> PhoneNumbers { get; set; }
    }
}
