using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(StudentStatusValidator))]
    public class StudentStatus
    {
        public CancelStudent cancel { get; set; }

        public CompleteStudent complete { get; set; }

        public TerminateStudent terminate { get; set; }

        public string Verify { get; set; }
    }
}
