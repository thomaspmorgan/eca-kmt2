using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.SEVIS;
using FluentValidation;
using System;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class ExchangeVisitorValidator : AbstractValidator<ExchangeVisitor>
    {
        public const int POSITION_CODE_LENGTH = 3;
        public const int CATEGORY_CODE_LENGTH = 2;
        public const int OCCUPATION_CATEGORY_CODE_LENGTH = 2;

        public const string SITE_OF_ACTIVITY_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Site of activity address is required.";

        public const string FINANCIAL_INFO_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Financial Information is required.";

        public const string SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Field of Study is required.";

        public const string CATEGORY_CODE_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Program category is required.";

        public const string PROGRAM_END_DATE_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Participant end date is required.";

        public const string PROGRAM_START_DATE_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Participant start date is required.";

        public const string POSITION_CODE_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Participant position is required.";

        public static string OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE = string.Format("Exch. Visitor: Occupational Category Code must be {0} characters.", OCCUPATION_CATEGORY_CODE_LENGTH);

        public static string PROGRAM_CATEGORY_CODE_ERROR_MESSAGE = string.Format("Exch. Visitor: Program category is required and can be up to {0} characters.", CATEGORY_CODE_LENGTH);

        public static string POSITION_CODE_LENGTH_ERROR_MESSAGE = string.Format("Exch. Visitor: Position Code is required and must be {0} characters.", POSITION_CODE_LENGTH);

        public const string BIOGRAPHICAL_INFORMATION_REQUIRED_ERROR_MESSAGE = "Exch. Visitor: Biographical Information is required.";

        public const string PROGRAM_END_DATE_MUST_BE_AFTER_START_DATE_ERROR = "Exch. Visitor: The participant end date must be after the start date.";

        public ExchangeVisitorValidator()
        {
            RuleFor(visitor => visitor.Biographical)
                .NotNull()
                .WithMessage(BIOGRAPHICAL_INFORMATION_REQUIRED_ERROR_MESSAGE)
                .SetValidator(new BiographicalValidator())
                .When(visitor => visitor.requestID != null);

            RuleFor(visitor => visitor.PositionCode)
                .NotNull()
                .WithMessage(POSITION_CODE_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath())
                .Length(POSITION_CODE_LENGTH)
                .WithMessage(POSITION_CODE_LENGTH_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath());

            RuleFor(visitor => visitor.PrgStartDate)
                .NotEqual(default(DateTime))
                .WithState(x => new SevisErrorPath())
                .WithMessage(PROGRAM_START_DATE_REQUIRED_ERROR_MESSAGE);
            
            RuleFor(visitor => visitor.PrgEndDate)
                .NotEqual(default(DateTime))
                .WithState(x => new SevisErrorPath())
                .WithMessage(PROGRAM_END_DATE_REQUIRED_ERROR_MESSAGE);

            RuleFor(visitor => visitor.PrgEndDate)
                 .GreaterThan(x => x.PrgStartDate)
                 .WithMessage(PROGRAM_END_DATE_MUST_BE_AFTER_START_DATE_ERROR)
                 .WithState(x => new SevisErrorPath());

            RuleFor(visitor => visitor.CategoryCode)
                .NotNull()
                .WithMessage(CATEGORY_CODE_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath())
                .Length(CATEGORY_CODE_LENGTH)
                .WithMessage(PROGRAM_CATEGORY_CODE_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath());
            
            When(x => x.OccupationCategoryCode != null, () =>
            {
                RuleFor(visitor => visitor.OccupationCategoryCode)
                .Length(OCCUPATION_CATEGORY_CODE_LENGTH)
                .WithMessage(OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE);
            });

            RuleFor(visitor => visitor.SubjectField)
                .NotNull()
                .WithMessage(SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath())
                .SetValidator(new SubjectFieldValidator())
                .When(visitor => visitor.requestID != null);

            RuleFor(visitor => visitor.USAddress)
                .SetValidator(new USAddressValidator())
                .When(visitor => visitor.USAddress != null);


            RuleFor(visitor => visitor.MailAddress)
                .SetValidator(new USAddressValidator())
                .When(visitor => visitor.MailAddress != null);
                
            RuleFor(visitor => visitor.FinancialInfo)
                .NotNull()
                .WithMessage(FINANCIAL_INFO_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new SevisErrorPath())
                .SetValidator(new FinancialInfoValidator())
                .When(visitor => visitor.requestID != null);

            RuleFor(visitor => visitor.CreateDependent)
                .SetCollectionValidator(new CreateDependentValidator())
                .When(visitor => visitor.CreateDependent != null);

            RuleFor(visitor => visitor.AddSiteOfActivity)
                .NotNull()
                .WithMessage(SITE_OF_ACTIVITY_REQUIRED_ERROR_MESSAGE)
                .SetValidator(new AddSiteOfActivityValidator())
                .When(visitor => visitor.requestID != null);
        }
    }
}