using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class ParticipantInfoUpdateValidator : AbstractValidator<ParticipantInfoUpdate>
    {
        public const int EMAIL_MAX_LENGTH = 255;
        public const int FOS_MAX_LENGTH = 100;
        public const int DEGREE_MAX_LENGTH = 100;
        public const int YOE_MAX_LENGTH = 2;

        public ParticipantInfoUpdateValidator()
        {
            RuleFor(visitor => visitor.EmailAddress).NotNull().WithMessage("Participant Info: Email is required").Length(1, EMAIL_MAX_LENGTH).WithMessage("Participant Info: Email can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters").EmailAddress().WithMessage("Participant Info: Email is invalid");
            RuleFor(visitor => visitor.FieldOfStudy).NotNull().WithMessage("Participant Info: Field of Study is required").Length(1, FOS_MAX_LENGTH).WithMessage("Participant Info: Field of Study can be up to " + FOS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.TypeOfDegree).NotNull().WithMessage("Participant Info: Type of Degree is required").Length(1, DEGREE_MAX_LENGTH).WithMessage("Participant Info: Type of Degree can be up to " + DEGREE_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.YearsOfExperience).Length(0, YOE_MAX_LENGTH).WithMessage("Participant Info: Years of Experience can be up to " + YOE_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.DateAwardedOrExpected).NotNull().WithMessage("Participant Info: Date awarded or expected is required");
        }
    }
}