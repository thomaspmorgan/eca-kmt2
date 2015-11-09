using System.ComponentModel.DataAnnotations;

namespace ECA.Data
{
    public class InternationalOrganization
    {
        private const int INTERNATIONAL_ORGANIZATION_CODE_LENGTH = 10;
        private const int INTERNATIONAL_ORGANIZATION_DESCRIPTION_LENGTH = 250;

        public InternationalOrganization()
        {
            this.History = new History();
        }

        /// <summary>
        /// Id of the International Organization
        /// </summary>
        [Key]
        public int OrganizationId { get; set; }

        /// <summary>
        /// Code for the International Organizational
        /// </summary>
        [MinLength(INTERNATIONAL_ORGANIZATION_CODE_LENGTH), MaxLength(INTERNATIONAL_ORGANIZATION_CODE_LENGTH)]
        public string OrganizationCode { get; set; }
        
        /// <summary>
        ///  Description of the International Organizational
        /// </summary>
        [MaxLength(INTERNATIONAL_ORGANIZATION_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// History of the US  International Organizational
        /// </summary>
        public History History { get; set; }
    }
}
