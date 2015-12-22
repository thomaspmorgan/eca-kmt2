using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    public class SiteOfActivityUpdate
    {
        public SiteOfActivitySOA AddSOA { get; set; }

        public SiteOfActivityExemptUpdate AddEXEMPT { get; set; }

        public SiteOfActivitySeekingGemp AddSEEKINGEMP { get; set; }

        public SiteOfActivityAddOnTravel AddONTRAVEL { get; set; }

        public DeleteSiteOfActivity Delete { get; set; }

        public EditSiteOfActivity Edit { get; set; }
        
    }
}