using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    public class AddTIPPUpdate
    {
        public AddTIPPUpdate()
        {
            ParticipantInfo = new ParticipantInfoUpdate();
            TippSite = new TippSite();
        }

        public bool print7002 { get; set; }

        public ParticipantInfoUpdate ParticipantInfo { get; set; }

        public TippSite TippSite { get; set; }


    }
}