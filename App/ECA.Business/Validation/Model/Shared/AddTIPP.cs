namespace ECA.Business.Validation.Model.CreateEV
{
    public class AddTIPP
    {
        public AddTIPP()
        {
            tippExemptProgram = new TippExemptProgram();
            participantInfo = new ParticipantInfo();
            tippSite = new TippSite();
        }

        public bool print7002 { get; set; }

        public TippExemptProgram tippExemptProgram { get; set; }

        public ParticipantInfo participantInfo { get; set; }

        public TippSite tippSite { get; set; }
    }
}