using System;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class ParticipantInfo
    {
        public bool IsIWT { get; set; }

        public string EmailAddress { get; set; }

        public string FieldOfStudy { get; set; }

        public string YearsOfExperience { get; set; }

        public string TypeOfDegree { get; set; }

        public DateTime DateAwardedOrExpected { get; set; }
    }
}