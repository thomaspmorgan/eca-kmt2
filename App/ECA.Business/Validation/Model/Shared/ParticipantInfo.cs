using System;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(ParticipantInfoValidator))]
    public class ParticipantInfo
    {
        public ParticipantInfo()
        { }

        public bool IsIWT { get; set; }

        public string EmailAddress { get; set; }

        public string FieldOfStudy { get; set; }

        public string YearsOfExperience { get; set; }

        public string TypeOfDegree { get; set; }

        public DateTime DateAwardedOrExpected { get; set; }
    }
}