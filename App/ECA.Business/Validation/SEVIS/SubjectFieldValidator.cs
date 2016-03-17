using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// The SubjectFieldValidator is used to validate a participant's field of study.
    /// </summary>
    public class SubjectFieldValidator : AbstractValidator<SubjectField>
    {
        /// <summary>
        /// The subject field code max length.
        /// </summary>
        public const int FIELD_CODE_MAX_LENGTH = 7;

        /// <summary>
        /// The max length of the foreign field of study.
        /// </summary>
        public const int FOREIGN_FIELD_MAX_LENGTH = 100;

        /// <summary>
        /// The max length of the remarks.
        /// </summary>
        public const int REMARKS_MAX_LENGTH = 500;

        /// <summary>
        /// The field of study code regular expression.
        /// </summary>
        public const string FIELD_OF_STUDY_CODE_REGEX = @"^[0-9]{2}[.][0-9]{4}$";

        /// <summary>
        /// The error message to return when a subject field code is invalid.
        /// </summary>
        public const string SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE = "The participant's field of study code is required, must be 7 characters long, and in a ##.#### format.";

        /// <summary>
        /// The error message to return when a foreign degree level is invalid.
        /// </summary>
        public static string SUBJECT_FIELD_FOREIGN_DEGREE_ERROR_MESSAGE = string.Format("The participant's field of study foreign degree level can be up to {0} characters.", FOREIGN_FIELD_MAX_LENGTH);

        /// <summary>
        /// The error message to return when a foreign field of study exceeds the max length.
        /// </summary>
        public static string SUBJECT_FIELD_OF_STUDY_MAX_LENGTH_ERROR_MESSAGE = string.Format("The participant's foreign field of study can be up to {0} characters.", FOREIGN_FIELD_MAX_LENGTH);

        /// <summary>
        /// The error message to return when the remarks are invalid.
        /// </summary>
        public const string SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE = "The participant's field of study remarks are required.";

        /// <summary>
        /// The error message to return when the field of study remarks exceed the max length.
        /// </summary>
        public static string REMARKS_MAX_LENGTH_ERROR_MESSAGE = string.Format("The participant's field of study remarks are required and can be can be up to {0} characters.", REMARKS_MAX_LENGTH);

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public SubjectFieldValidator()
        {
            RuleFor(visitor => visitor.SubjectFieldCode)
                .NotNull()
                .WithMessage(SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE)
                .WithState(x => new FieldOfStudyErrorPath())
                .Matches(new Regex(FIELD_OF_STUDY_CODE_REGEX))
                .WithMessage(SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE)
                .WithState(x => new FieldOfStudyErrorPath());

            RuleFor(visitor => visitor.ForeignDegreeLevel)
                .Length(0, FOREIGN_FIELD_MAX_LENGTH)
                .WithMessage(SUBJECT_FIELD_FOREIGN_DEGREE_ERROR_MESSAGE)
                .WithState(x => new FieldOfStudyErrorPath());

            RuleFor(visitor => visitor.ForeignFieldOfStudy)
                .Length(0, FOREIGN_FIELD_MAX_LENGTH)
                .WithMessage(SUBJECT_FIELD_OF_STUDY_MAX_LENGTH_ERROR_MESSAGE)
                .WithState(x => new FieldOfStudyErrorPath());

            RuleFor(visitor => visitor.Remarks)
                .NotEmpty()
                .WithMessage(SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new FieldOfStudyErrorPath())
                .Length(0, REMARKS_MAX_LENGTH)
                .WithMessage(REMARKS_MAX_LENGTH_ERROR_MESSAGE)
                .WithState(x => new FieldOfStudyErrorPath());
        }
    }
}