using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CreateDependent
    {
        /// <summary>
        /// Dependent personal information
        /// </summary>
        public PersonalInfo Dependent { get; set; }
        
        /// <summary>
        /// Dependent record remarks
        /// </summary>
        [MaxLength(500)]
        public string Remarks { get; set; }
        
    }
}
