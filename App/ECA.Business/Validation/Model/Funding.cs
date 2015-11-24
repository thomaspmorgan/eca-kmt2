using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class Funding
    {
        [MaxLength(8)]
        public int Personal { get; set; }

        public School School { get; set; }

        public FundingOther Other { get; set; }

        [MaxLength(8)]
        public int Employment { get; set; }
        
    }
}
