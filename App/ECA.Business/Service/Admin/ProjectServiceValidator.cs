using ECA.Business.Validation;
using System;
using System.Collections.Generic;
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
        /// The error message when the focus given is invalid.
        /// </summary>
        public const string FOCUS_REQUIRED_ERROR_MESSAGE = "The focus is required.";

        /// <summary>
        /// The error message when the program given is invalid.
        /// </summary>
        public const string PROGRAM_REQUIRED_ERROR_MESSAGE = "The program is required.";

        /// <summary>
        /// The error message when the start and ends dates of a project are invalid.
        /// </summary>
        public const string INVALID_START_AND_END_DATE_MESSAGE = "The start and end dates are invalid.";

        /// <summary>
        /// eturns enumerated validation results for a project create.
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
            if (String.IsNullOrWhiteSpace(validationEntity.Name))
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.Name, INVALID_NAME_ERROR_MESSAGE);
            }
            if (String.IsNullOrWhiteSpace(validationEntity.Description))
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.Description, INVALID_DESCRIPTION_ERROR_MESSAGE);
            }
            if (validationEntity.Focus == null)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.FocusId, FOCUS_REQUIRED_ERROR_MESSAGE);
            }
            if (validationEntity.StartDate >= validationEntity.EndDate)
            {
                yield return new BusinessValidationResult<PublishedProject>(x => x.StartDate, INVALID_START_AND_END_DATE_MESSAGE);
            }
        }
    }
}
