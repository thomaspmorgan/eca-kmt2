using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    public class SiteOfActivityExemptUpdate : SiteOfActivityExempt
    {
        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool printForm { get; set; }
    }
}