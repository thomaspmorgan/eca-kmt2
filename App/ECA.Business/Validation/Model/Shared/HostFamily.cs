using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(HostFamilyValidator))]
    public class HostFamily
    {
        public HostFamily()
        {
            PContact = new PContact();
            SContact = new SContact();
        }

        public PContact PContact { get; set; }

        public SContact SContact { get; set; }
        
        public string Phone { get; set; }

        public string PhoneExt { get; set; }
    }
}