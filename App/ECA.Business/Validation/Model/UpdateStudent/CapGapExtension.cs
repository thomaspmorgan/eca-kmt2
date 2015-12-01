using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CapGapExtension
    {
        [StringLength(2)]
        private string _capGapExtension;

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
