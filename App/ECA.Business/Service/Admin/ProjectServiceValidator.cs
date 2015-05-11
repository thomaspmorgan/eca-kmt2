using ECA.Business.Validation;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The ProjectServiceValidator is used to validate a Project on create and update.
    /// </summary>
    public class ProjectServiceValidator : BusinessValidatorBase<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity>
    {
        /// <summary>
        /// The error message when at least one category is required.
        /// </summary>
        public const string CATEGORIES_REQUIRED_ERROR_MESSAGE = "At least one category is required.";

        /// <summary>
        /// The error message when at least one objective is required.
        /// </summary>
        public const string OBJECTIVES_REQUIRED_ERROR_MESSAGE = "At least one objective is required.";

        /// <summary>
        /// The error message when at least one objective does not exist.
        /// </summary>
        public const string OBJECTIVES_DO_NOT_EXIST_ERROR_MESSAGE = "At least one of the given objectives does not exist in the system.";

        /// <summary>
        /// The error message when at least one categories does not exist.
        /// </summary>
        public const string CATEGORIES_DO_NOT_EXIST_ERROR_MESSAGE = "At least one of the given categories does not exist in the system.";

        /// <summary>
        /// The error message when at least one contact does not exist.
        /// </summary>
        public const string CONTACTS_DO_NOT_EXIST_ERROR_MESSAGE = "At least one of the given contacts does not exist in the system.";

        /// <summary>
        /// The error message when at least one goal does not exist.
        /// </summary>
        public const string GOALS_DO_NOT_EXIST_ERROR_MESSAGE = "At least one of the given goals does not exist in the system.";

        /// <summary>
        /// The error message when at least one theme does not exist.
        /// </summary>
        public const string THEMES_DO_NOT_EXIST_ERROR_MESSAGE = "At least one of the given themes does not exist in the system.";

        /// <summary>
        /// The error message when a project is given an invalid name.
        /// </summary>
        public const string INVALID_NAME_ERROR_MESSAGE = "The name of the project is invalid.";

        /// <summary>
        /// The error message when a project is given an invalid description.
        /// </summary>
        public const string INVALID_DESCRIPTION_ERROR_MESSAGE = "The description of the project is invalid.";

        /// <summary>
        /// The error message when the program given is invalid.
        /// </summary>
        public const string PROGRAM_REQUIRED_ERROR_MESSAGE = "The program is required.";

        /// <summary>
        /// The error message when the start and ends dates of a project are invalid.
        /// </summary>
        public const string INVALID_START_AND_END_DATE_MESSAGE = "The start and end dates are invalid.";

        /// <summary>
        /// The error message when the project was not in draft and is set back to draft.
        /// </summary>
        public const string CAN_NOT_SET_PROJECT_BACK_TO_DRAFT_ERROR_MESSAGE = "The project can not be set back to a draft state once it has been out of draft.";


        /// <summary>
        /// Returns enumerated validation results for a project create.
        /// </summary>
        /// <param name="validationEntity">The create entity.</param>
        /// <returns>The enumerated errors.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(ProjectServiceCreateValidationEntity validationEntity)
        {
            if (String.IsNullOrWhiteSpace(validationEntity.Name))
            {
                yield return new BusinessValidationResult<DraftProject>(x => x.Name, INVALID_NAME_ERROR_MESSAGE);
            }
            if (String.IsNullOrWhiteSpace(validationEntity.Description))
            {
                yield return new BusinessValidationResult<DraftProject>(x => x.Description, INVALID_DESCRIPTION_ERROR_MESSAGE);
            }
            if (validationEntity.Program == null)
            {
                yield return new BusinessValidationResult<DraftProject>(x => x.ProgramId, PROGRAM_REQUIRED_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// Returns enumerated validation results for a project update.
        /// </summary>
        /// <param name="validationEntity">The update entity.</param>
        /// <returns>The enumerated errors.</returns>

        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(ProjectServiceUpdateValidationEntity validationEntity)
        {
            Contract.Requires(validationEntity.OfficeSettings != null, "The office settings must not be null.");
            if (!validationEntity.ThemesExist)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.ThemeIds, THEMES_DO_NOT_EXIST_ERROR_MESSAGE);
            }
            if (!validationEntity.GoalsExist)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.GoalIds, GOALS_DO_NOT_EXIST_ERROR_MESSAGE);
            }
            if (!validationEntity.PointsOfContactExist)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.PointsOfContactIds, CONTACTS_DO_NOT_EXIST_ERROR_MESSAGE);
            }
            if (!validationEntity.ObjectivesExist)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.ObjectiveIds, OBJECTIVES_DO_NOT_EXIST_ERROR_MESSAGE);
            }
            if (!validationEntity.CategoriesExist)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.CategoryIds, CATEGORIES_DO_NOT_EXIST_ERROR_MESSAGE);
            }
            if (validationEntity.OfficeSettings.IsObjectiveRequired && validationEntity.NumberOfObjectives < 1)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.ObjectiveIds, OBJECTIVES_REQUIRED_ERROR_MESSAGE);
            }
            if (validationEntity.OfficeSettings.IsCategoryRequired && validationEntity.NumberOfCategories < 1)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.CategoryIds, CATEGORIES_REQUIRED_ERROR_MESSAGE);
            }
            if (String.IsNullOrWhiteSpace(validationEntity.Name))
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.Name, INVALID_NAME_ERROR_MESSAGE);
            }
            if (String.IsNullOrWhiteSpace(validationEntity.Description))
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.Description, INVALID_DESCRIPTION_ERROR_MESSAGE);
            }
            if (validationEntity.StartDate >= validationEntity.EndDate)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.StartDate, INVALID_START_AND_END_DATE_MESSAGE);
            }
            var oldProjectStatus = ProjectStatus.GetStaticLookup(validationEntity.OriginalProjectStatusId);
            var newProjectStatus = ProjectStatus.GetStaticLookup(validationEntity.UpdatedProjectStatusId);
            Contract.Assert(oldProjectStatus != null, "The old project status must not be null.");
            Contract.Assert(newProjectStatus != null, "The new project status must not be null.");
            if (oldProjectStatus != ProjectStatus.Draft && newProjectStatus == ProjectStatus.Draft)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.ProjectStatusId, CAN_NOT_SET_PROJECT_BACK_TO_DRAFT_ERROR_MESSAGE);
            }
        }
    }
}
