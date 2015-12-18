namespace ECA.Business.Validation.Model.CreateEV
{
    /// <summary>
    /// Validate the exchange visitor’s participation in the program
    /// </summary>
    public class ValidateParticipant
    {
        public ValidateParticipant()
        {
            USAddress = new USAddress();
            TravelInfo = new TravelInfo();
            TippSignatureDates = new SignatureDates();
        }
        
        public USAddress USAddress { get; set; }

        public TravelInfo TravelInfo { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public SignatureDates TippSignatureDates { get; set; }
    }
}