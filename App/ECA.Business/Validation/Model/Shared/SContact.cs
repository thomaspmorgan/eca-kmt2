using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(SContactValidator))]
    public class SContact
    {
        public SContact()
        { }

        public string LastName { get; set; }

        public string FirsName { get; set; }
    }
}