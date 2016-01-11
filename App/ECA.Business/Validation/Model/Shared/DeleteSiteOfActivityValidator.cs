using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class DeleteSiteOfActivityValidator : AbstractValidator<DeleteSiteOfActivity>
    {
        public const int ID_MAX_LENGTH = 50;

        public DeleteSiteOfActivityValidator()
        {
            RuleFor(soa => soa.SiteId).NotNull().WithMessage("Delete SOA: Site ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Delete SOA: Site ID can be up to " + ID_MAX_LENGTH.ToString() + " characters"); ;
        }
    }
}