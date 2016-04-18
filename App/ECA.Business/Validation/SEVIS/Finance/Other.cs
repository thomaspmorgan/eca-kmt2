using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using Newtonsoft.Json;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// The Other financial info type.
    /// </summary>
    [Validator(typeof(OtherValidator))]
    public class Other
    {
        /// <summary>
        /// Creates a new Other financial info object.
        /// </summary>
        /// <param name="name">The name of the sponsor.</param>
        /// <param name="amount">The funding amount.</param>
        [JsonConstructor]
        public Other(string name, string amount)
        {
            this.Name = name;
            this.Amount = amount;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public string Amount { get; private set; }

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

        /// <summary>
        /// Returns a OtherFundsNullableTypeOther sevis exchange visitor model.
        /// </summary>
        /// <returns>Returns a OtherFundsNullableTypeOther sevis exchange visitor model.</returns>
        public OtherFundsNullableTypeOther GetOtherFundsNullableTypeInternational()
        {
            return new OtherFundsNullableTypeOther
            {
                Amount = this.Amount,
                Name = this.Name
            };
        }

        /// <summary>
        /// Returns the total funding.
        /// </summary>
        /// <returns>The total funding.</returns>
        public decimal GetTotalFunding()
        {
            decimal total = 0.0m;
            decimal amount;
            if(!string.IsNullOrWhiteSpace(this.Amount) && decimal.TryParse(this.Amount, out amount))
            {
                total += amount;
            }
            return total;
        }
    }
}