using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CancelEducationLevelValidator))]
    public class CancelEducationLevel
    {
        public string Remarks { get; set; }
    }
}
