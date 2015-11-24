
namespace ECA.Business.Validation.Model
{
    public class StudentStatus
    {
        public CancelStudent cancel { get; set; }

        public CompleteProgram complete { get; set; }

        public TerminateStudent terminate { get; set; }

        public string Verify { get; set; }
    }
}
