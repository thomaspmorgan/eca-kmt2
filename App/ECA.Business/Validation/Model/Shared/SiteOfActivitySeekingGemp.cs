using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(SiteOfActivitySeekingGempValidator))]
    public class SiteOfActivitySeekingGemp
    {
        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool printForm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remarks { get; set; }
    }
}