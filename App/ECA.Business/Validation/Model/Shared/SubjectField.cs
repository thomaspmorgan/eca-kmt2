using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(SubjectFieldValidator))]
    public class SubjectField
    {
        public string SubjectFieldCode { get; set; }

        public string ForeignDegreeLevel { get; set; }

        public string ForeignFieldOfStudy { get; set; }

        public string Remarks { get; set; }
    }
}