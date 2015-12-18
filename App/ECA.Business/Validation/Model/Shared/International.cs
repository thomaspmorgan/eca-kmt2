using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(InternationalValidator))]
    public class International
    {
        public string org1 { get; set; }

        public string otherName1 { get; set; }

        public string amount1 { get; set; }

        public string org2 { get; set; }

        public string otherName2 { get; set; }

        public string amount2 { get; set; }
    }
}