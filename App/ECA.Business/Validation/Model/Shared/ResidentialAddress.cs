﻿using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(ResidentialAddressValidator))]
    public class ResidentialAddress
    {
        public ResidentialAddress()
        {
            HostFamily = new HostFamily();
            BoardingSchool = new BoardingSchool();
            LCCoordinator = new LCCoordinator();
        }

        public string ResidentialType { get; set; }

        public HostFamily HostFamily { get; set; }

        public BoardingSchool BoardingSchool { get; set; }

        public LCCoordinator LCCoordinator { get; set; }        
    }
}