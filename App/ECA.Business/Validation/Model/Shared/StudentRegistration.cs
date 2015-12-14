using FluentValidation.Attributes;
using System;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(StudentRegistrationValidator))]
    public class StudentRegistration
    {
        public bool printForm { get; set; }

        public bool LastSession { get; set; }
        
        public DateTime CurrentSessionEndDate { get; set; }
        
        public DateTime NextSessionStartDate { get; set; }

        public bool Commuter { get; set; }

        public USAddress usAddress { get; set; }

        public ForeignAddress foreignAddress { get; set; }

        public TravelInfo travelInfo { get; set; }
        
        public string Remarks { get; set; }
    }
}
