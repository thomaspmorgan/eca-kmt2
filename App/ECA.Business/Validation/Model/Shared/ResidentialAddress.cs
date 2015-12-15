namespace ECA.Business.Validation.Model.CreateEV
{
    public class ResidentialAddress
    {
        public ResidentialAddress()
        {
            hostFamily = new HostFamily();
            boardingSchool = new BoardingSchool();
        }

        public string residentialType { get; set; }

        public HostFamily hostFamily { get; set; }

        public BoardingSchool boardingSchool { get; set; }

        public LCCoordinator lcCoordinator { get; set; }        
    }
}