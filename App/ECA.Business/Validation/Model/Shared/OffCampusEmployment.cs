using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(OffCampusEmploymentValidator))]
    public class OffCampusEmployment
    {
        public AddOCEmployment addOCEmployment { get; set; }

        public CancelOCEmployment cancelOCEmployment { get; set; }

        public EditOCEmployment editOCEmployment { get; set; }
    }
}
