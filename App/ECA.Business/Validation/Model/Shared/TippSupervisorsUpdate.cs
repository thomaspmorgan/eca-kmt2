namespace ECA.Business.Validation.Model.Shared
{
    public class TippSupervisorsUpdate
    {
        public TippSupervisorsUpdate()
        {
            TippPhase = new TippSupervisorPhaseUpdate();
        }

        public TippSupervisorPhaseUpdate TippPhase { get; set; }
    }
}