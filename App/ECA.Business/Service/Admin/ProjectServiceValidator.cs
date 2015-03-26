using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class ProjectServiceValidator : BusinessValidatorBase<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity>
    {
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(ProjectServiceCreateValidationEntity validationEntity)
        {
            return Enumerable.Empty<BusinessValidationResult>();
        }

        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(ProjectServiceUpdateValidationEntity validationEntity)
        {
            return Enumerable.Empty<BusinessValidationResult>();
        }
    }
}
