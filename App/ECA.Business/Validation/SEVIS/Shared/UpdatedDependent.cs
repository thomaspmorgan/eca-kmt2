using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Edit dependent
    /// </summary>
    [Validator(typeof(UpdatedDependentValidator))]
    public class UpdatedDependent
    {
        public UpdatedDependent()
        {
            Add = new AddUpdatedDependent();
            Delete = new DeleteDependent();
            Edit = new EditDependent();
            EndStatus = new EndDependentStatus();
            Reprint = new ReprintForm();
            Terminate = new TerminateDependent();
        }

        /// <summary>
        /// User defined field A
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string UserDefinedA { get; set; }

        /// <summary>
        /// User defined field B
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string UserDefinedB { get; set; }

        /// <summary>
        /// Add depedent
        /// </summary>
        public AddUpdatedDependent Add { get; set; }

        /// <summary>
        /// Delete dependent
        /// </summary>
        public DeleteDependent Delete { get; set; }

        /// <summary>
        /// Dependent personal information
        /// </summary>
        public EditDependent Edit { get; set; }
        
        /// <summary>
        /// End the status for a dependent
        /// </summary>
        public EndDependentStatus EndStatus { get; set; }

        /// <summary>
        /// Reprint dependent DS-2019
        /// </summary>
        public ReprintForm Reprint { get; set; }

        /// <summary>
        /// Terminate dependent
        /// </summary>
        public TerminateDependent Terminate { get; set; }
    }
}
