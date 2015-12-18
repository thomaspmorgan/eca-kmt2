using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(AddTippValidator))]
    public class AddTIPP
    {
        public AddTIPP()
        {
            TippExemptProgram = new TippExemptProgram();
            ParticipantInfo = new ParticipantInfo();
            TippSite = new TippSite();
        }

        public bool print7002 { get; set; }

        public TippExemptProgram TippExemptProgram { get; set; }

        public ParticipantInfo ParticipantInfo { get; set; }

        public TippSite TippSite { get; set; }
    }
}