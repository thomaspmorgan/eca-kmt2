using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(HostFamilyValidator))]
    public class HostFamily
    {
        public HostFamily()
        {
            pContact = new PContact();
            sContact = new SContact();
        }

        public PContact pContact { get; set; }

        public SContact sContact { get; set; }
        
        public string phone { get; set; }

        public string phoneExt { get; set; }
    }
}