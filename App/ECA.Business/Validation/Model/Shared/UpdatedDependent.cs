using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class UpdatedDependent
    {
        [MaxLength(10)]
        public string UserDefinedA { get; set; }

        [MaxLength(14)]
        public string UserDefinedB { get; set; }
        
        public AddDependent addDependent { get; set; }

        public CancelDependent cancelDependent { get; set; }

        public EditDependent editDependent { get; set; }

        public ReactivateDependent reactivateDependent { get; set; }

        public ReprintDependent reprintDependent { get; set; }
        
    }
}
