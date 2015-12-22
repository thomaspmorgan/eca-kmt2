using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    public class AddUpdatedDependent
    {
        public AddUpdatedDependent()
        {
            FullName = new FullName();
        }

        public FullName FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string BirthCity { get; set; }

        public string BirthCountryCode { get; set; }

        public string CitizenshipCountryCode { get; set; }

        public string PermanentResidenceCountryCode { get; set; }

        public string BirthCountryReason { get; set; }

        public string EmailAddress { get; set; }

        public string Relationship { get; set; }

        public string FormPurpose { get; set; }
    }
}