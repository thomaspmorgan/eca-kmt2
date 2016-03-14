using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Bio
{
    public class PersonValidator : BiographicalValidator<Person>
    {
        public const int POSITION_CODE_LENGTH = 3;
        public const int CATEGORY_CODE_LENGTH = 2;

        public const string CATEGORY_CODE_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Program category is required.";

        public static string PROGRAM_CATEGORY_CODE_ERROR_MESSAGE = string.Format("Exch. Visitor: Program category is required and can be up to {0} characters.", CATEGORY_CODE_LENGTH);

        public const string POSITION_CODE_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Participant position is required.";

        public static Regex POSITION_CODE_REGEX = new Regex(@"^\d{1,3}$");

        public const string POSITION_CODE_MUST_BE_DIGITS_ERROR_MESSAGE = "Exch. Visitor:  Pariticipant positon code must be a number with 1 to 3 digits.";

        public const string SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Field of Study is required.";

        public PersonValidator()
            : base()
        {
            RuleFor(x => x.ProgramCategoryCode)
                .NotNull()
                .WithMessage(CATEGORY_CODE_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new ProgramCategoryCodeErrorPath())
                .Length(CATEGORY_CODE_LENGTH)
                .WithMessage(PROGRAM_CATEGORY_CODE_ERROR_MESSAGE)
                .WithState(x => new ProgramCategoryCodeErrorPath());

            RuleFor(visitor => visitor.PositionCode)
                .NotNull()
                .WithMessage(POSITION_CODE_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new PositionCodeErrorPath())
                .Matches(POSITION_CODE_REGEX)
                .WithMessage(POSITION_CODE_MUST_BE_DIGITS_ERROR_MESSAGE)
                .WithState(x => new PositionCodeErrorPath());

            RuleFor(visitor => visitor.SubjectField)
                .NotNull()
                .WithMessage(SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new FieldOfStudyErrorPath())
                .SetValidator(new SubjectFieldValidator());
        }
    }
}
