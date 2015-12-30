
namespace ECA.Business.Validation.Model.Shared
{
    public class TippPhaseDatesUpdate
    {
        public TippPhaseDatesUpdate()
        {
            TippPhase = new TippPhaseUpdate();
        }

        public TippPhaseUpdate TippPhase { get; set; }
    }
}