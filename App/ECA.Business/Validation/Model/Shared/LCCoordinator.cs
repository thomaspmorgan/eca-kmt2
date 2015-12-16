using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(LCCoordinatorValidator))]
    public class LCCoordinator
    {
        public string LastName { get; set; }

        public string FirsName { get; set; }
    }
}