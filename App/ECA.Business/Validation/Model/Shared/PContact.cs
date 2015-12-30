using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(PContactValidator))]
    public class PContact
    {
        public string LastName { get; set; }

        public string FirsName { get; set; }
    }
}