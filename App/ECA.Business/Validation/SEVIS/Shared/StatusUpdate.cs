using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    public class StatusUpdate
    {
        public StatusUpdate()
        {
            CorrectInfraction = new CorrectInfraction();
            End = new EndProgram();
            Invalid = new InvalidStatus();
            NoShow = new NoShowStatus();
            Terminate = new TerminateVisitor();
        }

        public CorrectInfraction CorrectInfraction { get; set; }

        public EndProgram End { get; set; }

        public InvalidStatus Invalid { get; set; }

        public NoShowStatus NoShow { get; set; }

        public TerminateVisitor Terminate { get; set; }        
    }
}