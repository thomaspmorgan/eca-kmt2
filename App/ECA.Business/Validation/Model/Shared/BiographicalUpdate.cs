using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// Edit exchange visitor biographical information
    /// </summary>
    [Validator(typeof(BiographicalUpdateValidator))]
    public class BiographicalUpdate : Biographical
    {
        public BiographicalUpdate()
        {
            USAddress = new USAddress();
            MailAddress = new USAddress();
            ResidentialAddress = new ResidentialAddress();
        }

        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool printForm { get; set; }

        /// <summary>
        /// Current phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Position held in home country
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// US physical address
        /// </summary>
        public USAddress USAddress { get; set; }

        /// <summary>
        /// US mailing address
        /// </summary>
        public USAddress MailAddress { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Residential address for Au Pair or secondary school student
        /// </summary>
        public ResidentialAddress ResidentialAddress { get; set; }        
    }
}
