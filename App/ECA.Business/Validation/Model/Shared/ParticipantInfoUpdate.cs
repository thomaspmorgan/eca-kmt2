﻿using System;

namespace ECA.Business.Validation.Model.Shared
{
    public class ParticipantInfoUpdate
    {
        public string EmailAddress { get; set; }

        public string FieldOfStudy { get; set; }

        public string YearsOfExperience { get; set; }

        public string TypeOfDegree { get; set; }

        public DateTime DateAwardedOrExpected { get; set; }
    }
}