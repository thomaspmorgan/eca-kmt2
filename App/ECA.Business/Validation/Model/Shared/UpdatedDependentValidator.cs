using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class UpdatedDependentValidator : AbstractValidator<UpdatedDependent>
    {
        public const int USERDEFINEDA_MAX_LENGTH = 10;
        public const int USERDEFINEDB_MAX_LENGTH = 14;

        public UpdatedDependentValidator()
        {
            RuleFor(student => student.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Student: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Student: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.addDependent).SetValidator(new AddDependentValidator()).When(student => student.addDependent != null);
            RuleFor(student => student.cancelDependent).SetValidator(new CancelDependentValidator()).When(student => student.cancelDependent != null);
            RuleFor(student => student.editDependent).SetValidator(new EditDependentValidator()).When(student => student.editDependent != null);
            RuleFor(student => student.reactivateDependent).SetValidator(new ReactivateDependentValidator()).When(student => student.reactivateDependent != null);
            RuleFor(student => student.reprintDependent).SetValidator(new ReprintFormValidator()).When(student => student.reprintDependent != null);
        }
    }
}