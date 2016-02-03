using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// A ContactServiceValidator is used to validate points of contact.
    /// </summary>
    public class ContactServiceValidator : BusinessValidatorBase<AdditionalPointOfContactValidationEntity, object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(AdditionalPointOfContactValidationEntity validationEntity)
        {
            return Enumerable.Empty<BusinessValidationResult>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(object validationEntity)
        {
            return Enumerable.Empty<BusinessValidationResult>();
        }
    }
}
