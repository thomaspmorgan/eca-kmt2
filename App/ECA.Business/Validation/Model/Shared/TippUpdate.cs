using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    public class TippUpdate
    {
        public TippUpdate()
        {
            AddTIPP = new AddTIPPUpdate();
            AddSite = new TippSite();
            EditSite = new EditTippSite();
            DeleteSite = new DeleteTippSite();
            AddPhase = new AddPhase();
            EditPhase = new EditPhase();
            DeletePhase = new DeletePhase();
            UpdateSignatureDates = new UpdateSignatureDates();
        }

        public AddTIPPUpdate AddTIPP { get; set; }

        public TippSite AddSite { get; set; }

        public EditTippSite EditSite { get; set; }

        public DeleteTippSite DeleteSite { get; set; }

        public AddPhase AddPhase { get; set; }

        public EditPhase EditPhase { get; set; }

        public DeletePhase DeletePhase { get; set; }

        public UpdateSignatureDates UpdateSignatureDates { get; set; }
    }
}