
namespace ECA.Business.Validation.Model.Shared
{
    public class SupervisorsUpdate
    {
        public SupervisorsUpdate()
        {
            TippPhase = new TippSupervisorPhaseUpdate();
        }

        public TippSupervisorPhaseUpdate TippPhase { get; set; }
    }
}