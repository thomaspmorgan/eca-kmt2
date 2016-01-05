using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation
{
    public class SEVISBatchCreateUpdateEVValidator : AbstractValidator<SEVISBatchCreateUpdateEV>
    {
        public const int ID_MAX_LENGTH = 20;

        public SEVISBatchCreateUpdateEVValidator()
        {
            RuleFor(update => update.userID).NotNull().Length(1, ID_MAX_LENGTH).WithMessage("User ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(update => update.BatchHeader).NotNull().WithMessage("Batch header is required").SetValidator(new BatchHeaderValidator());
            RuleFor(update => update.CreateEV).SetValidator(new CreateExchVisitorValidator()).When(update => update.CreateEV != null);
            RuleFor(update => update.UpdateEV).SetValidator(new UpdateExchVisitorValidator()).When(update => update.UpdateEV != null);
        }
    }
}