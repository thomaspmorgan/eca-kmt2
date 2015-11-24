using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class EducationalInfo
    {
        public EduLevel eduLevel { get; set; }

        [MaxLength(7)]
        public string PrimaryMajor { get; set; }

        [MaxLength(7)]
        public string SecondMajor { get; set; }

        [MaxLength(7)]
        public string Minor { get; set; }

        [MaxLength(2)]
        public string LengthOfStudy { get; set; }

        public DateTime PrgStartDate { get; set; }

        public DateTime PrgEndDate { get; set; }

        public EngProficiency engProficiency { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }
    }
}
