using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EditDependentValidator))]
    public class EditDependent
    {
        public EditDependent()
        {
            FullName = new FullName();
        }

        /// <summary>
        /// Dependent Sevis ID
        /// </summary>
        public string dependentSevisID { get; set; }

        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool printForm { get; set; }

        /// <summary>
        /// Full name of person
        /// </summary>
        public FullName FullName { get; set; }

        /// <summary>
        /// Student date of birth.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gender code.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// City of birth
        /// </summary>
        public string BirthCity { get; set; }

        /// <summary>
        /// Country of birth
        /// </summary>
        public string BirthCountryCode { get; set; }

        /// <summary>
        /// Country of citizenship
        /// </summary>
        public string CitizenshipCountryCode { get; set; }

        /// <summary>
        /// Country of legal permanent residence
        /// </summary>
        public string PermanentResidenceCountryCode { get; set; }

        /// <summary>
        /// (required if USA or territory is country of birth)
        /// </summary>
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Relationship
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }        
    }
}
