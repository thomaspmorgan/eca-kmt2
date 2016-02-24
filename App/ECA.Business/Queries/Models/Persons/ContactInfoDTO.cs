using ECA.Business.Queries.Models.Admin;
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
            EmailAddresses = new List<EmailAddressDTO>();
            SocialMedias = new List<SocialMediaDTO>();
            PhoneNumbers = new List<PhoneNumberDTO>();
        }

        /// <summary>
        /// Gets and sets emails
        /// </summary>
        public IEnumerable<EmailAddressDTO> EmailAddresses { get; set; }

        /// <summary>
        /// Gets and sets social medias
        /// </summary>
        public IEnumerable<SocialMediaDTO> SocialMedias { get; set; }

        /// <summary>
        /// Gets and set phone numbers
        /// </summary>
        public IEnumerable<PhoneNumberDTO> PhoneNumbers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasContactAgreement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SimpleLookupDTO> ContactAgreements { get; set; }

        /// <summary>
        /// Gets or sets the Person Id.
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId of this participant.
        /// </summary>
        public int ProjectId { get; set; }
    }
}
