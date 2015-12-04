using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EngProficiencyValidator))]
    public class EngProficiency
    {
        public bool EngRequired { get; set; }

        public bool RequirementsMet { get; set; }
        
        public string NotRequiredReason { get; set; }
    }
}
