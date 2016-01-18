using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// Program official and supervisor signature information
    /// </summary>
    public class TippSiteUpdate : TippSite
    {
        public TippSiteUpdate()
        {
            ProgramOfficial = new ProgramOfficial();
            Supervisors = new TippSupervisorsUpdate();
        }
        
        /// <summary>
        /// Program official signature
        /// </summary>
        public ProgramOfficial ProgramOfficial { get; set; }
        
        /// <summary>
        /// Supervisor signature
        /// </summary>
        public TippSupervisorsUpdate Supervisors { get; set; }
    }
}