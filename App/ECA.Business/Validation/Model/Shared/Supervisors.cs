namespace ECA.Business.Validation.Model.CreateEV
{
    public class Supervisors
    {
        public Supervisors()
        {
            TippPhase = new TippPhase();
        }

        public TippPhase TippPhase { get; set; }
    }
}