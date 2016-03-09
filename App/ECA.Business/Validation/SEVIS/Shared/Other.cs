using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(OtherValidator))]
    public class Other
    {
        public Other()
        {

        }

        public string Name { get; set; }

        public string Amount { get; set; }

        public OtherFundsTypeOther GetOtherFundsTypeOther()
        {
            return new OtherFundsTypeOther
            {

            };
        }
    }
}