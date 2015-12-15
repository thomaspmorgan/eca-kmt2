using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(PersonalInfoValidator))]
    public class PersonalInfo
    {
        /// <summary>
        /// Full name of person
        /// </summary>
        public FullName fullName { get; set; }
        
        /// <summary>
        /// Student date of birth.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gender code.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Country of birth
        /// </summary>
        public string BirthCountryCode { get; set; }

        /// <summary>
        /// Country of citizenship
        /// </summary>
        public string CitizenshipCountryCode { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Commuter flag
        /// </summary>
        public bool Commuter { get; set; }

        /// <summary>
        /// Visa type
        /// </summary>
        public string VisaType { get; set; }

        #region Exchange Visitor only

        /// <summary>
        /// City of birth (max 50)
        /// </summary>
        public string BirthCity { get; set; }

        /// <summary>
        /// Country of legal permanent residence
        /// </summary>
        public string PermanentResidenceCountryCode { get; set; }

        /// <summary>
        /// (required if USA or territory is country of birth)
        /// </summary>
        public string BirthCountryReason { get; set; }
        
        #endregion
        
    }
}
