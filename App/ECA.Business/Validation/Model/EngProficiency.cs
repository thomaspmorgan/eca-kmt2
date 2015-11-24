using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class EngProficiency
    {

        public bool EngRequired { get; set; }

        public bool RequirementsMet { get; set; }

        [MaxLength(500)]
        public string NotRequiredReason { get; set; }

    }
}
