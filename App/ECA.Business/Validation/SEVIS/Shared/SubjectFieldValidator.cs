using ECA.Business.Validation.SEVIS;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Model.Shared
{
    public class SubjectFieldValidator : AbstractValidator<SubjectField>
    {
        public const int FIELD_CODE_MAX_LENGTH = 7;
        public const int FOREIGN_FIELD_MAX_LENGTH = 100;
        public const int REMARKS_MAX_LENGTH = 500;

        public const string FIELD_OF_STUDY_CODE_REGEX = @"^[0-9]{2}[.][0-9]{4}$";

        public const string SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE = "Subject Field: The field of study code is required, must be 7 characters long, and in a ##.#### format.";

        public static string SUBJECT_FIELD_FOREIGN_DEGREE_ERROR_MESSAGE = string.Format("Subject Field: Foreign Degree Level can be up to {0} characters", FOREIGN_FIELD_MAX_LENGTH);

        public static string SUBJECT_FIELD_OF_STUDY_MAX_LENGTH_ERROR_MESSAGE = string.Format("Subject Field: Foreign Field of Study can be up to {0} characters", FOREIGN_FIELD_MAX_LENGTH);

        public const string SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE = "Subject Field: Remarks are required";

        public static string REMARKS_MAX_LENGTH_ERROR_MESSAGE = string.Format("Subject Field: Remarks are required and can be can be up to {0} characters", REMARKS_MAX_LENGTH);

        public SubjectFieldValidator()
        {
            RuleFor(visitor => visitor.SubjectFieldCode)
                .NotNull()
                .WithMessage(SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath())
                .Matches(new Regex(FIELD_OF_STUDY_CODE_REGEX))
                .WithMessage(SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath());

            RuleFor(visitor => visitor.ForeignDegreeLevel)
                .Length(0, FOREIGN_FIELD_MAX_LENGTH)
                .WithMessage(SUBJECT_FIELD_FOREIGN_DEGREE_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath());

            RuleFor(visitor => visitor.ForeignFieldOfStudy)
                .Length(0, FOREIGN_FIELD_MAX_LENGTH)
                .WithMessage(SUBJECT_FIELD_OF_STUDY_MAX_LENGTH_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath());

            RuleFor(visitor => visitor.Remarks)
                .NotEmpty()
                .WithMessage(SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath())
                .Length(0, REMARKS_MAX_LENGTH)
                .WithMessage(REMARKS_MAX_LENGTH_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath());
        }
    }
}