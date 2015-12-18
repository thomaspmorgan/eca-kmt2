using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(BoardingSchoolValidator))]
    public class BoardingSchool
    {
        public string name { get; set; }

        public string phone { get; set; }

        public string phoneExt { get; set; }
    }
}