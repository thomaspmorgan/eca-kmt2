﻿using FluentValidation;

namespace ECA.Business.Validation
{
    public class SEVISBatchCreateUpdateEVValidator : AbstractValidator<SEVISBatchCreateUpdateEV>
    {
        public const int ID_MAX_LENGTH = 20;

        public SEVISBatchCreateUpdateEVValidator()
        {
            RuleFor(update => update.userID).Length(1, ID_MAX_LENGTH).WithMessage("User ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(update => update.BatchHeader).NotNull().WithMessage("Batch header is required");
            //RuleFor(update => update.CreateEV).SetValidator(new CreateExchVisitorValidator()).When(update => update.UpdateEV == null);
            //RuleFor(update => update.UpdateEV).SetValidator(new UpdateExchVisitorValidator()).When(update => update.CreateEV == null);
        }
    }
}