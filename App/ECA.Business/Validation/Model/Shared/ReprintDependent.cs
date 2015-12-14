using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ReprintDependentValidator))]
    public class ReprintDependent
    {
        [MaxLength(11)]
        public string dependentSevisID { get; set; }

        public bool PrintForm { get; set; }

    }
}
