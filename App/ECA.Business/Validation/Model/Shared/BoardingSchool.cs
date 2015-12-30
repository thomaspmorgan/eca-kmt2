using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(BoardingSchoolValidator))]
    public class BoardingSchool
    {
        public BoardingSchool()
        { }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string PhoneExt { get; set; }
    }
}