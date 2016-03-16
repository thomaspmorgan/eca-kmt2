using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.SEVIS;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Finance
{
    public class OtherFundsValidator : AbstractValidator<OtherFunds>
    {
        public const int AMOUNT_MAX_LENGTH = 8;

        public const string AMOUNT_REGEX = @"^\d{1,8}$";

        public static string EV_GOVT_ERROR_MESSAGE = string.Format("Financial Info: Government funding can be up to {0} digits.", AMOUNT_MAX_LENGTH);

        public static string BINATIONAL_ERROR_MESSAGE = string.Format("Financial Info: Binational funding can be up to {0} digits.", AMOUNT_MAX_LENGTH);

        public static string PERSONAL_ERROR_MESSAGE = string.Format("Financial Info: Personal funding can be up to {0} digits.", AMOUNT_MAX_LENGTH);

        public OtherFundsValidator()
        {
            RuleFor(visitor => visitor.Other).SetValidator(new OtherValidator()).When(visitor => visitor.Other != null);
            RuleFor(visitor => visitor.USGovt).SetValidator(new USGovtValidator()).When(visitor => visitor.USGovt != null);
            RuleFor(visitor => visitor.International).SetValidator(new InternationalValidator()).When(visitor => visitor.International != null);
            When(visitor => visitor.EVGovt != null, () =>
            {
                RuleFor(x => x.EVGovt)
                 .Matches(new Regex(AMOUNT_REGEX))
                 .WithMessage(EV_GOVT_ERROR_MESSAGE)
                 .WithState(x => new FundingErrorPath());
            });

            When(visitor => visitor.BinationalCommission != null, () =>
            {
                RuleFor(x => x.BinationalCommission)
                 .Matches(new Regex(AMOUNT_REGEX))
                 .WithMessage(BINATIONAL_ERROR_MESSAGE)
                 .WithState(x => new FundingErrorPath());
            });
            When(visitor => visitor.Personal != null, () =>
            {
                RuleFor(x => x.Personal)
                 .Matches(new Regex(AMOUNT_REGEX))
                 .WithMessage(PERSONAL_ERROR_MESSAGE)
                 .WithState(x => new FundingErrorPath());
            });
        }
    }
}