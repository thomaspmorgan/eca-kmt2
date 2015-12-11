using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EducationalInfoValidator))]
    public class EducationLevel
    {
        public CancelEducationLevel cancelEducationLevel { get; set; }        
    }
}
