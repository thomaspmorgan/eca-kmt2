using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ReprintFormValidator : AbstractValidator<ReprintForm>
    {
        public const int SEVIS_MAX_LENGTH = 11;

        public ReprintFormValidator()
        {

        }
    }
}