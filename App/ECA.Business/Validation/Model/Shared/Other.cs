using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(OtherValidator))]
    public class Other
    {
        public Other()
        { }

        public string name { get; set; }

        public string amount { get; set; }
    }
}