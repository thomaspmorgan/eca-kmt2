using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// US government organization funding
    /// </summary>
    [Validator(typeof(USGovtValidator))]
    public class USGovt
    {
        /// <summary>
        /// US government organization 1
        /// </summary>
        public string Org1 { get; set; }

        /// <summary>
        /// Other US government organization 1
        /// </summary>
        public string OtherName1 { get; set; }

        /// <summary>
        /// US government organization 1 funding amount
        /// </summary>
        public string Amount1 { get; set; }

        /// <summary>
        /// US government organization 2
        /// </summary>
        public string Org2 { get; set; }

        /// <summary>
        /// Other US government organization 2
        /// </summary>
        public string OtherName2 { get; set; }

        /// <summary>
        /// US government organization 2 funding amount
        /// </summary>
        public string Amount2 { get; set; }
    }
}