using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(SiteOfActivityAddOnTravelValidator))]
    public class SiteOfActivityAddOnTravel
    {
        public SiteOfActivityAddOnTravel()
        { }

        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool printForm { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
    }
}