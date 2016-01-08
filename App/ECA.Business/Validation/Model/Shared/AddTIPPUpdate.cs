using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// T/IPP site with phases
    /// </summary>
    [Validator(typeof(AddTIPPUpdateValidator))]
    public class AddTIPPUpdate
    {
        public AddTIPPUpdate()
        {
            ParticipantInfo = new ParticipantInfoUpdate();
            TippSite = new TippSiteUpdate();
        }

        public bool print7002 { get; set; }

        public ParticipantInfoUpdate ParticipantInfo { get; set; }

        public TippSiteUpdate TippSite { get; set; }
    }
}