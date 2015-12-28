namespace ECA.Business.Validation.Model.Shared
{
    public class SupervisorsUpdate
    {
        public SupervisorsUpdate()
        {
            TippPhase = new TippPhaseUpdate();
        }

        public TippPhaseUpdate TippPhase { get; set; }

        public ParticipantInfoUpdate UpdateParticipantInfo { get; set; }
    }
}