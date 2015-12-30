using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(ReprintFormUpdateValidator))]
    public class ReprintFormUpdate : ReprintForm
    {
        /// <summary>
        /// Dependent Sevis ID
        /// </summary>
        public string dependentSevisID { get; set; }
    }
}