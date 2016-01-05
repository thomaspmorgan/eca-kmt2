
namespace ECA.Business.Validation.Model.Shared
{
    public class SupervisorsUpdate
    {
        public SupervisorsUpdate()
        {
            TippPhase = new TippPhaseUpdate();
            UpdateParticipantInfo = new ParticipantInfoUpdate();
        }

        public TippPhaseUpdate TippPhase { get; set; }

        public ParticipantInfoUpdate UpdateParticipantInfo { get; set; }
    }
}