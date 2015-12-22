using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Validation.Model.CreateEV
{
    /// <summary>
    /// EV Status Change Events
    /// </summary>
    public class Status
    {
        public Status()
        {

        }

        public CorrectInfraction CorrectInfraction { get; set; }
        
        public EndProgram End { get; set; }

        public InvalidStatus Invalid { get; set; }

        public NoShowStatus NoShow { get; set; }

        public TerminateVisitor Terminate { get; set; }
    }
}