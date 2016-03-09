using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Bio
{
    public class UpdatedPerson : PersonBiography, IFormPrintable, ISevisExchangeVisitorUpdatableComponent
    {
        public bool BirthCountryCodeSpecified { get; set; }

        public bool BirthDateSpecified { get; set; }

        public bool CitizenshipCodeSpecified { get; set; }

        public bool GenderSpecifieid { get; set; }

        public bool PermanentResidenceCountryCodeSpecified { get; set; }

        public bool PrintForm { get; set; }

        public USAddress MailAddress { get; set; }

        public USAddress USAddress { get; set; }

        /// <summary>
        /// Returns a registered sevis exchange visitor's updated biographical information model to be used
        /// to update an exchange visitor's biography.
        /// </summary>
        /// <returns>A registered sevis exchange visitor's updated biographical information model to be used
        /// to update an exchange visitor's biography.</returns>
        public SEVISEVBatchTypeExchangeVisitorBiographical GetSEVISEVBatchTypeExchangeVisitorBiographical()
        {
            return new SEVISEVBatchTypeExchangeVisitorBiographical
            {

            };
        }

        public object GetSevisEvBatchTypeExchangeVisitorUpdateComponent()
        {
            return GetSEVISEVBatchTypeExchangeVisitorBiographical();
        }
    }
}
