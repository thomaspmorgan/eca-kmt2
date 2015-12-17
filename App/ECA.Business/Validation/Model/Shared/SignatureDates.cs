using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model
{
    public class SignatureDates
    {
        public SignatureDates()
        {
            TippSite = new TippSite();
            UpdateParticipantInfo = new ParticipantInfo();
        }

        public TippSite TippSite { get; set; }

        public ParticipantInfo UpdateParticipantInfo { get; set; }
    }
}