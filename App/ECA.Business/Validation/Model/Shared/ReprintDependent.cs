using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class ReprintDependent
    {
        [MaxLength(11)]
        public string dependentSevisID { get; set; }

        public bool PrintForm { get; set; }

    }
}
