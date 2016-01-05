using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model
{
    public class TippPhaseDates
    {
        public TippPhaseDates()
        {
            TippPhase = new TippPhase();
        }

        public TippPhase TippPhase { get; set; }
    }
}