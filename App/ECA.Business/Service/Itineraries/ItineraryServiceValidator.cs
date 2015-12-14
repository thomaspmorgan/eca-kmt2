using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class ItineraryServiceValidator : BusinessValidatorBase<AddedEcaItineraryValidationEntity, UpdatedEcaItineraryValidationEntity>
    {
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(AddedEcaItineraryValidationEntity validationEntity)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(UpdatedEcaItineraryValidationEntity validationEntity)
        {
            throw new NotImplementedException();
        }
    }
}
