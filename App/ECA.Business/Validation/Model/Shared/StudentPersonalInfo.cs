using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Personal information record used for updating a student record
    /// </summary>
    [Validator(typeof(StudentPersonalInfoValidator))]
    public class StudentPersonalInfo
    {
        public bool printForm { get; set; }
        
        public FullName fullName { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public string Gender { get; set; }
        
        public string BirthCountryCode { get; set; }
        
        public string CitizenshipCountryCode { get; set; }
        
        public string Email { get; set; }

        public bool Commuter { get; set; }

        public USAddress usAddress { get; set; }

        public ForeignAddress foreignAddress { get; set; }
        
        public string Remarks { get; set; }        
    }
}
