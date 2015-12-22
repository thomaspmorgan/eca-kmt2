using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(PersonalInfoValidator))]
    public class PersonalInfo
    {
        public PersonalInfo()
        {
            usAddress = new USAddress();
            mailAddress = new USAddress();
            residentialAddress = new ResidentialAddress();
        }

        /// <summary>
        /// Dependent Sevis ID
        /// </summary>
        public string dependentSevisID { get; set; }

        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool PrintForm { get; set; }

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
        /// City of birth
        /// </summary>
        public string BirthCity { get; set; }

        /// <summary>
        /// Country of birth
        /// </summary>
        public string BirthCountryCode { get; set; }

        /// <summary>
        /// (required if USA or territory is country of birth)
        /// </summary>
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Country of citizenship
        /// </summary>
        public string CitizenshipCountryCode { get; set; }

        /// <summary>
        /// Country of legal permanent residence
        /// </summary>
        public string PermanentResidenceCountryCode { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Relationship
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// EV phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// EV position code
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// EV physical address
        /// </summary>
        public USAddress usAddress { get; set; }

        /// <summary>
        /// EV mailing address
        /// </summary>
        public USAddress mailAddress { get; set; }
        
        /// <summary>
        /// EV residential address
        /// </summary>
        public ResidentialAddress residentialAddress { get; set; }

        /// <summary>
        /// Commuter flag
        /// </summary>
        public bool Commuter { get; set; }

        /// <summary>
        /// Visa type
        /// </summary>
        public string VisaType { get; set; }

        /// <summary>
        /// EV remarks
        /// </summary>
        public string Remarks { get; set; }
        
    }
}
