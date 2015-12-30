using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class UpdatedDependentValidator : AbstractValidator<UpdatedDependent>
    {
        public const int USERDEFINEDA_MAX_LENGTH = 10;
        public const int USERDEFINEDB_MAX_LENGTH = 14;

        public UpdatedDependentValidator()
        {
            RuleFor(student => student.Edit).SetValidator(new EditDependentValidator()).When(student => student.Edit != null);
            RuleFor(student => student.Reprint).SetValidator(new ReprintFormValidator()).When(student => student.Reprint != null);
        }
    }
}