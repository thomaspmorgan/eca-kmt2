using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ReactivateDependentValidator))]
    public class ReactivateDependent
    {
        public string dependentSevisID { get; set; }

        public bool PrintForm { get; set; }
    }
}
