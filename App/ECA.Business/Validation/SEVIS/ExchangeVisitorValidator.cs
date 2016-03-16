using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Business.Validation.Sevis.Finance;
using FluentValidation;
using System;

namespace ECA.Business.Validation.Sevis
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
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Person)
                .NotNull()
                .WithMessage(BIOGRAPHICAL_INFORMATION_REQUIRED_ERROR_MESSAGE)
                .SetValidator(new PersonValidator());            

            RuleFor(x => x.ProgramStartDate)
                .NotEqual(default(DateTime))
                .WithState(x => new StartDateErrorPath())
                .WithMessage(PROGRAM_START_DATE_REQUIRED_ERROR_MESSAGE);

            RuleFor(visitor => visitor.ProgramEndDate)
                .NotEqual(default(DateTime))
                .WithState(x => new EndDateErrorPath())
                .WithMessage(PROGRAM_END_DATE_REQUIRED_ERROR_MESSAGE);

            RuleFor(visitor => visitor.ProgramEndDate)
                 .GreaterThan(x => x.ProgramStartDate)
                 .WithMessage(PROGRAM_END_DATE_MUST_BE_AFTER_START_DATE_ERROR)
                 .WithState(x => new EndDateErrorPath());

            When(x => x.OccupationCategoryCode != null, () =>
            {
                RuleFor(visitor => visitor.OccupationCategoryCode)
                .Length(OCCUPATION_CATEGORY_CODE_LENGTH)
                .WithMessage(OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE);
            });

            RuleFor(visitor => visitor.FinancialInfo)
                .NotNull()
                .WithMessage(FINANCIAL_INFO_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath())
                .SetValidator(new FinancialInfoValidator());

            RuleFor(visitor => visitor.Dependents)
                .SetCollectionValidator(new DependentValidator())
                .When(visitor => visitor.Dependents != null);

            RuleFor(visitor => visitor.SiteOfActivity)
                .NotNull()
                .WithMessage(SITE_OF_ACTIVITY_REQUIRED_ERROR_MESSAGE)
                .SetValidator(new AddressDTOValidator(AddressDTOValidator.C_STREET_ADDRESS));
        }
    }
}