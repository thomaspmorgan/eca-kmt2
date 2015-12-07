using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CapGapExtensionValidator))]
    public class CapGapExtension
    {
        public string _capGapExtension;

        public CapGapExtension(string capGapExtension)
        {
            _capGapExtension = capGapExtension;
        }

        public static implicit operator CapGapExtension(string capGapExtension)
        {
            if (capGapExtension == null)
                return null;

            return new CapGapExtension(capGapExtension);
        }
    }
}
