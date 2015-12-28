using ECA.Business.Validation.Model.CreateEV;
using System;

namespace ECA.Business.Validation.Model.Shared
{
    public class TippSiteUpdate
    {
        public TippSiteUpdate()
        {
            ProgramOfficial = new ProgramOfficial();
            Supervisors = new TippSupervisorsUpdate();
        }

        public string SiteId { get; set; }

        public ProgramOfficial ProgramOfficial { get; set; }

        public DateTime EvSignatureDate { get; set; }
        
        public TippSupervisorsUpdate Supervisors { get; set; }
    }
}