using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    public class UpdateSignatureDates
    {
        public UpdateSignatureDates()
        {
            TippSite = new TippSiteUpdate();
            UpdateParticipantInfo = new ParticipantInfoUpdate();
        }

        public TippSiteUpdate TippSite { get; set; }

        public ParticipantInfoUpdate UpdateParticipantInfo { get; set; }
    }
}