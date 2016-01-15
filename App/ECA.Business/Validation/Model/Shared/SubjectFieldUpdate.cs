using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(SubjectFieldUpdateValidator))]
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
