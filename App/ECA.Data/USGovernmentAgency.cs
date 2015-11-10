using System.ComponentModel.DataAnnotations;

namespace ECA.Data
{
    public class USGovernmentAgency
    {
        private const int USGOVERNMENT_AGENCY_CODE_LENGTH = 10;
        private const int USGOVERNMENT_AGENCY_DESCRIPTION_LENGTH = 250;

        public USGovernmentAgency()
        {
            this.History = new History();
        }

        /// <summary>
        /// Id of the US Government Agency
        /// </summary>
        [Key]
        public int AgencyId { get; set; }

        /// <summary>
        /// Code for the US Government Agency
        /// </summary>
        [MinLength(USGOVERNMENT_AGENCY_CODE_LENGTH), MaxLength(USGOVERNMENT_AGENCY_CODE_LENGTH)]
        public string AgencyCode { get; set; }
        
        /// <summary>
        ///  Description of the US Government Agency
        /// </summary>
        [MaxLength(USGOVERNMENT_AGENCY_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// History of the US Government Agency SEVIS lookup
        /// </summary>
        public History History { get; set; }
    }
}
