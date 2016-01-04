using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    public class SubjectFieldUpdate : SubjectField
    {
        public SubjectFieldUpdate()
        { }

        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool printForm { get; set; }

        /// <summary>
        /// Subject update field code description
        /// </summary>
        public string SubjectFieldRemarks { get; set; }
    }
}
