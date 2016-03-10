using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// The Other financial info type.
    /// </summary>
    [Validator(typeof(OtherValidator))]
    public class Other
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Returns a OtherFundsTypeOther sevis exchange visitor model.
        /// </summary>
        /// <returns>Returns a OtherFundsTypeOther sevis exchange visitor model.</returns>
        public OtherFundsTypeOther GetOtherFundsTypeOther()
        {
            return new OtherFundsTypeOther
            {
                Amount = this.Amount,
                Name = this.Name
            };
        }
    }
}