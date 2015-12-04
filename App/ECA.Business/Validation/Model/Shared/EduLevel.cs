using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EduLevelValidator))]
    public class EduLevel
    {
        public string Level { get; set; }
        
        public string OtherRemarks { get; set; }
    }
}
