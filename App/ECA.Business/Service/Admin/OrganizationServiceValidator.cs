using ECA.Business.Validation;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The OrganizationServiceValidator is used to validate organization modifications in the ECA system.
    /// </summary>
    public class OrganizationServiceValidator : BusinessValidatorBase<Object, UpdateOrganizationValidationEntity>
    {
        /// <summary>
        /// The error message to display when the name of the organization is not valid.
        /// </summary>
        public const string INVALID_ORGANIZATION_NAME_ERROR_MESSAGE = "The name of the organization is invalid.";
        public const string INVALID_ORGANIZATION_TYPE_ERROR_MESSAGE = "The type of the organization is invalid.";

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(Object validationEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validates the organization update via an UpdateOrganizationValidationEntity.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The validation results.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(UpdateOrganizationValidationEntity validationEntity)
        {
            if (String.IsNullOrWhiteSpace(validationEntity.Name))
            {
                yield return new BusinessValidationResult<UpdateOrganizationValidationEntity>(x => x.Name, INVALID_ORGANIZATION_NAME_ERROR_MESSAGE);
            }
            if (Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(validationEntity.OrganizationTypeId))
            {
                yield return new BusinessValidationResult<UpdateOrganizationValidationEntity>(x => x.OrganizationTypeId, INVALID_ORGANIZATION_TYPE_ERROR_MESSAGE);
            }
        }
    }
}
