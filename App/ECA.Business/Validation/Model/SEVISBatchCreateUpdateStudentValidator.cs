using ECA.Business.Validation.Model.Create;
using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class SEVISBatchCreateUpdateStudentValidator : AbstractValidator<SEVISBatchCreateUpdateStudent>
    {
        public const int ID_MAX_LENGTH = 20;

        public SEVISBatchCreateUpdateStudentValidator()
        {
            RuleFor(update => update.userID).NotNull().Length(1, ID_MAX_LENGTH).WithMessage("User ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(update => update.batchHeader).NotNull().WithMessage("Batch header is required").SetValidator(new BatchHeaderValidator());
            RuleFor(update => update.createStudent).SetValidator(new CreateStudentValidator());
        }
    }
}
