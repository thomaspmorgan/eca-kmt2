
namespace ECA.Business.Validation.Model.Shared
{
    public class AmendProgramUpdate : AmendProgram
    {
        public AmendProgramUpdate()
        {

        }

        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool PrintForm { get; set; }
    }
}
