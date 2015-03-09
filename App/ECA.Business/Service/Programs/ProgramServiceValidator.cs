﻿using ECA.Business.Models.Programs;
using ECA.Business.Validation;
using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// The ProgramServiceValidator is used to validate a Program Business entity on create and update.
    /// </summary>
    public class ProgramServiceValidator : BusinessValidatorBase<ProgramServiceValidationEntity, ProgramServiceValidationEntity>
    {
        /// <summary>
        /// The error message when a location is not a region.
        /// </summary>
        public const string GIVEN_LOCATION_IS_NOT_A_REGION_ERROR_MESSAGE = "The given location is not a region.";

        /// <summary>
        /// The error message when some locations are not regions.
        /// </summary>
        public const string NOT_ALL_LOCATIONS_ARE_REGIONS_ERROR_MESSAGE = "The given locations are not all regions.";

        /// <summary>
        /// The error message when focus is null.
        /// </summary>
        public const string FOCUS_DOES_NOT_EXIST_ERROR_MESSAGE = "The Focus does not exist.";

        /// <summary>
        /// The error message when organization does not exist.
        /// </summary>
        public const string ORGANIZATION_DOES_NOT_EXIST_ERROR_MESSAGE = "The Organization does not exist.";

        /// <summary>
        /// The error message when the parent program does not exist.
        /// </summary>
        public const string PARENT_PROGRAM_DOES_NOT_EXIST_ERROR_MESSAGE = "The parent program does not exist.";

        /// <summary>
        /// The program name is invalid.
        /// </summary>
        public const string INVALID_NAME_ERROR_MESSAGE = "The program name is invalid.";

        /// <summary>
        /// The program description is invalid.
        /// </summary>
        public const string INVALID_DESCRIPTION_ERROR_MESSAGE = "The program description is invalid.";

        /// <summary>
        /// Validates the given ProgramServiceValidationEntity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public IEnumerable<BusinessValidationResult> Validate(ProgramServiceValidationEntity validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity should not be null.");
            if (validationEntity.RegionLocationTypeIds.Count > 1)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.RegionIds, NOT_ALL_LOCATIONS_ARE_REGIONS_ERROR_MESSAGE);
            }
            if (validationEntity.RegionLocationTypeIds.Count == 1 && validationEntity.RegionLocationTypeIds.First() != LocationType.Region.Id)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.RegionIds, GIVEN_LOCATION_IS_NOT_A_REGION_ERROR_MESSAGE);
            }
            if (validationEntity.Focus == null)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.FocusId, FOCUS_DOES_NOT_EXIST_ERROR_MESSAGE);
            }
            if (validationEntity.OwnerOrganization == null)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.OwnerOrganizationId, ORGANIZATION_DOES_NOT_EXIST_ERROR_MESSAGE);
            }
            if (validationEntity.ParentProgramId.HasValue)
            {
                if (validationEntity.ParentProgram == null)
                {
                    yield return new BusinessValidationResult<EcaProgram>(x => x.ParentProgramId, PARENT_PROGRAM_DOES_NOT_EXIST_ERROR_MESSAGE);
                }
            }
            if (String.IsNullOrWhiteSpace(validationEntity.Name))
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.Name, INVALID_NAME_ERROR_MESSAGE);
            }
            if (String.IsNullOrWhiteSpace(validationEntity.Description))
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.Description, INVALID_DESCRIPTION_ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Validates the given ProgramServiceValidationEntity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(ProgramServiceValidationEntity validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity should not be null.");
            return Validate(validationEntity);
        }

        /// <summary>
        /// Validates the given ProgramServiceValidationEntity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(ProgramServiceValidationEntity validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity should not be null.");
            return Validate(validationEntity);
        }
    }
}
