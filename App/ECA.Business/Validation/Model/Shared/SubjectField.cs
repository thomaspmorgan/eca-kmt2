using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(SubjectFieldValidator))]
    public class SubjectField
    {
        public bool printForm { get; set; }

        public string SubjectFieldCode { get; set; }

        public string SubjectFieldRemarks { get; set; }

        public string ForeignDegreeLevel { get; set; }

        public string ForeignFieldOfStudy { get; set; }

        public string Remarks { get; set; }
    }
}