using ECA.Business.Validation.Sevis.Bio;
using System.Linq;
using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Business.Validation.Sevis.Finance;
using FluentValidation;
using System;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// The ExchangeVisitorValidator is used to validate an Exchange Visitor before it is sent to SEVIS.
    /// </summary>
    public class ExchangeVisitorValidator : AbstractValidator<ExchangeVisitor>
    {
        /// <summary>
        /// The max length of the occupation category code.
        /// </summary>
        public const int OCCUPATION_CATEGORY_CODE_LENGTH = 2;

        /// <summary>
        /// The error message to return when the site of activity is required.
        /// </summary>
        public const string SITE_OF_ACTIVITY_REQUIRED_ERROR_MESSAGE = "The participant's site of activity address is required.";

        /// <summary>
        /// The error message to return when the financial info object is required.
        /// </summary>
        public const string FINANCIAL_INFO_REQUIRED_ERROR_MESSAGE = "The participant's financial information is required.";

        /// <summary>
        /// The error message to return when the field of study is required.
        /// </summary>
        public const string SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE = "The participant's field of study is required.";

        /// <summary>
        /// The error message to return when the participant end date is required.
        /// </summary>
        public const string PROGRAM_END_DATE_REQUIRED_ERROR_MESSAGE = "The participant's end date is required.";

        /// <summary>
        /// The error message to return when the program start date is required.
        /// </summary>
        public const string PROGRAM_START_DATE_REQUIRED_ERROR_MESSAGE = "The participant's start date is required.";

        /// <summary>
        /// The error message to return when the program start date is required.
        /// </summary>
        public const string PROGRAM_START_DATE_MUST_BE_IN_THE_FUTURE = "The participant's start date can not be in the past.";

        /// <summary>
        /// The error message to return when the occupation category code is invalid.
        /// </summary>
        public static string OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE = string.Format("The participant's cccupational category code must be {0} characters.", OCCUPATION_CATEGORY_CODE_LENGTH);

        /// <summary>
        /// The error message to return when the person information is required.
        /// </summary>
        public const string PERSON_INFORMATION_REQUIRED_ERROR_MESSAGE = "The participant's biographical information is required.";

        /// <summary>
        /// The error message to return when the participant program end date is before the participant program start date.
        /// </summary>
        public const string PROGRAM_END_DATE_MUST_BE_AFTER_START_DATE_ERROR = "The participant's end date must be after the start date.";

        /// <summary>
        /// The error message to return when the participant has more than one spouse dependent.
        /// </summary>
        public const string PARTICIPANT_HAS_MORE_THAN_ONE_SPOUSE_DEPENDENT = "The participant has more than one spouse.";

        /// <summary>
        /// The maximum number of spousal dependents.
        /// </summary>
        public const int MAX_SPOUSE_DEPENDENTS = 1;

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public ExchangeVisitorValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Person)
                .NotNull()
                .WithMessage(PERSON_INFORMATION_REQUIRED_ERROR_MESSAGE)
                .SetValidator(x => new PersonValidator());

            RuleFor(x => x.ProgramStartDate)
                .NotEqual(default(DateTime))
                .WithState(x => new StartDateErrorPath())
                .WithMessage(PROGRAM_START_DATE_REQUIRED_ERROR_MESSAGE);

            RuleFor(x => x.ProgramStartDate)
               .GreaterThan(DateTime.UtcNow.Date)
               .WithState(x => new StartDateErrorPath())
               .WithMessage(PROGRAM_START_DATE_MUST_BE_IN_THE_FUTURE);

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
                .SetValidator(new AddressDTOValidator());

            When(x => x.Dependents.Count() > 0, () =>
            {
                RuleFor(x => x.Dependents).Must(d =>
                {
                    return d.Where(x => x.IsSpousalDependent()).Count() <= 1;
                })
                .WithMessage(PARTICIPANT_HAS_MORE_THAN_ONE_SPOUSE_DEPENDENT)
                .WithState(x => new DependentErrorPath(x.Dependents.Where(d => d.IsSpousalDependent()).Select(d => d.PersonId).First()));
            });
        }
    }
}