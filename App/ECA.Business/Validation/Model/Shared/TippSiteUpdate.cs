using ECA.Business.Validation.Model.CreateEV;
using System;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// Program official and supervisor signature information
    /// </summary>
    public class TippSiteUpdate
    {
        public TippSiteUpdate()
        {
            ProgramOfficial = new ProgramOfficial();
            Supervisors = new TippSupervisorsUpdate();
        }

        public string SiteId { get; set; }

        /// <summary>
        /// Program official signature
        /// </summary>
        public ProgramOfficial ProgramOfficial { get; set; }

        /// <summary>
        /// Signature date by EV
        /// </summary>
        public DateTime EvSignatureDate { get; set; }
        
        /// <summary>
        /// Supervisor signature
        /// </summary>
        public TippSupervisorsUpdate Supervisors { get; set; }
    }
}