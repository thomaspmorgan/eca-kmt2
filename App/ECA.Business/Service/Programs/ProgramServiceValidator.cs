using ECA.Business.Models.Programs;
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
        public const string NO_CATEGORIES_GIVEN_ERROR_MESSAGE = "There must be at least one focus/category for a program with this organization.";

        public const string NO_OBJECTIVES_GIVEN_ERROR_MESSAGE = "There must be at least one one justification/objective for a program with this organization.";

        /// <summary>
        /// The error message when at least one location is inactive.
        /// </summary>
        public const string INACTIVE_LOCATIONS_ERROR_MESSAGE = "At least one location is no longer active.";


        /// <summary>
        /// The error message to display when a program is having it's parent program changed to a program that is already a child of that program.
        /// </summary>
        public const string CIRCULAR_PARENT_PROGRAM_ERROR_MESSAGE = "The given parent program is already a child program of this program.";

        /// <summary>
        /// The error message when a program is configured without any goals.
        /// </summary>
        public const string NO_GOALS_GIVEN_ERROR_MESSAGE = "There must be at least one goal for a program.";

        /// <summary>
        /// The error message when a program is configured without any themes.
        /// </summary>
        public const string NO_THEMES_GIVEN_ERROR_MESSAGE = "There must be at least one theme for a program.";

        /// <summary>
        /// The error message when a program is configured without any regions.
        /// </summary>
        public const string NO_REGIONS_GIVEN_ERROR_MESSAGE = "There must be at least one region for a program.";

        /// <summary>
        /// The error message when a program is configured without any points of contact.
        /// </summary>
        public const string NO_POINTS_OF_CONTACT_GIVEN_ERROR_MESSAGE = "There must be at least one point of contact for a program.";

        /// <summary>
        /// The error message when a location is not a region.
        /// </summary>
        public const string GIVEN_LOCATION_IS_NOT_A_REGION_ERROR_MESSAGE = "The given location is not a region.";

        /// <summary>
        /// The error message when some locations are not regions.
        /// </summary>
        public const string NOT_ALL_LOCATIONS_ARE_REGIONS_ERROR_MESSAGE = "The given locations are not all regions.";

        /// <summary>
        /// The error message when organization does not exist.
        /// </summary>
        public const string ORGANIZATION_DOES_NOT_EXIST_ERROR_MESSAGE = "The Organization does not exist.";

        /// <summary>
        /// The error message when the parent program does not exist.
        /// </summary>
        public const string PARENT_PROGRAM_DOES_NOT_EXIST_ERROR_MESSAGE = "The Parent Program does not exist.";

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
            if (validationEntity.RegionLocationTypeIds.Count > 1)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.RegionIds, NOT_ALL_LOCATIONS_ARE_REGIONS_ERROR_MESSAGE);
            }
            if (validationEntity.RegionLocationTypeIds.Count == 1 && validationEntity.RegionLocationTypeIds.First() != LocationType.Region.Id)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.RegionIds, GIVEN_LOCATION_IS_NOT_A_REGION_ERROR_MESSAGE);
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
            if (validationEntity.ContactIds == null || validationEntity.ContactIds.Count == 0)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.ContactIds, NO_POINTS_OF_CONTACT_GIVEN_ERROR_MESSAGE);
            }
            if (validationEntity.RegionIds == null || validationEntity.RegionIds.Count == 0)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.RegionIds, NO_REGIONS_GIVEN_ERROR_MESSAGE);
            }
            if (validationEntity.ThemeIds == null || validationEntity.ThemeIds.Count == 0)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.ThemeIds, NO_THEMES_GIVEN_ERROR_MESSAGE);
            }
            if (validationEntity.GoalIds == null || validationEntity.GoalIds.Count == 0)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.GoalIds, NO_GOALS_GIVEN_ERROR_MESSAGE);
            }
            if (validationEntity.OwnerOfficeSettings.IsObjectiveRequired)
            {
                if (validationEntity.ObjectiveIds == null || validationEntity.ObjectiveIds.Count == 0)
                {
                    yield return new BusinessValidationResult<EcaProgram>(x => x.JustificationObjectiveIds, NO_OBJECTIVES_GIVEN_ERROR_MESSAGE);
                }
            }
            if (validationEntity.OwnerOfficeSettings.IsCategoryRequired)
            {
                if (validationEntity.CategoryIds == null || validationEntity.CategoryIds.Count == 0)
                {
                    yield return new BusinessValidationResult<EcaProgram>(x => x.FocusCategoryIds, NO_CATEGORIES_GIVEN_ERROR_MESSAGE);
                }
            }
            if(validationEntity.ParentProgramParentPrograms.Select(x => x.ProgramId).ToList().Contains(validationEntity.ProgramId))
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.ParentProgramId, CIRCULAR_PARENT_PROGRAM_ERROR_MESSAGE);
            }
            if(validationEntity.InactiveRegionIds.Count() > 0)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.RegionIds, INACTIVE_LOCATIONS_ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Validates the given ProgramServiceValidationEntity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(ProgramServiceValidationEntity validationEntity)
        {
            return Validate(validationEntity);
        }

        /// <summary>
        /// Validates the given ProgramServiceValidationEntity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(ProgramServiceValidationEntity validationEntity)
        {
            return Validate(validationEntity);
        }
    }
}
