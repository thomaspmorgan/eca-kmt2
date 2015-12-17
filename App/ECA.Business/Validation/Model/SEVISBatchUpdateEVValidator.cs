using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation
{
    public class SEVISBatchUpdateEVValidator : AbstractValidator<SEVISBatchUpdateEV>
    {
        public const int ID_MAX_LENGTH = 20;

        public SEVISBatchUpdateEVValidator()
        {
            RuleFor(update => update.userID).NotNull().Length(1, ID_MAX_LENGTH).WithMessage("User ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(update => update.batchHeader).NotNull().WithMessage("Batch header is required").SetValidator(new BatchHeaderValidator());
            // set visitor validator if not null
            RuleFor(update => update.createVisitor).SetValidator(new CreateExchVisitorValidator()).When(update => update.createVisitor != null);
        }
    }
}