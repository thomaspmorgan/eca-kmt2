using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(DisciplinaryActionValidator))]
    public class DisciplinaryAction
    {
        public string Explanation { get; set; }
    }
}
