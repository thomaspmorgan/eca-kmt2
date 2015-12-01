using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ECA.Data
{
    /// <summary>
    /// A person participant on a project
    /// </summary>
    public class ParticipantStudentVisitor : IHistorical
    {

        /// <summary>
        /// Gets the max length of a Study Project
        /// </summary>
        private const int REMARKS_MAX_LENGTH = 255;

        /// <summary>
        /// constructor to initialize history for a ParticipantStudentVisitor
        /// </summary>
        public ParticipantStudentVisitor()
        {
            this.History = new History();
        }

        /// <summary>
        /// The key, and foreign key to the participant
        /// </summary>
        [Key]
        public int ParticipantId { get; set; }
        
        /// <summary>
        /// The educational level ID of the student
        /// </summary>
        public int? EducationLevelId {get; set;}

        /// <summary>
        /// The educational level of the student
        /// </summary
        public EducationLevel EducationLevel { get; set; }

        /// <summary>
        /// The Education Level Remarks if "Other"
        /// </summary>
        [MaxLength(REMARKS_MAX_LENGTH)]
        public string EducationLevelOtherRemarks { get; set; }

        /// <summary>
        /// Primary Major Id
        /// </summary>
        public int? PrimaryMajorId { get; set; }

        /// <summary>
        /// Primary Major
        /// </summary>
        [ForeignKey("PrimaryMajorId")]
        public FieldOfStudy PrimaryMajor { get; set; }

        /// <summary>
        /// Secondary Major Id
        /// </summary>
        public int? SecondaryMajorId { get; set; }

        /// <summary>
        /// Secondary Major
        /// </summary>
        [ForeignKey("SecondaryMajorId")]
        public FieldOfStudy SecondaryMajor { get; set; }

        /// <summary>
        /// Minor Id
        /// </summary>
        public int? MinorId { get; set; }

        /// <summary>
        /// Minor
        /// </summary>
        [ForeignKey("MinorId")]
        public FieldOfStudy Minor { get; set; }

        /// <summary>
        /// The length of Study in Months
        /// </summary>
        public int? LengthOfStudy { get; set; }

        /// <summary>
        /// does the participant need English Language Proficiency?
        /// </summary>
        public bool IsEnglishProficiencyReqd { get; set; }

        /// <summary>
        /// has the participant met English Language Proficiency?
        /// </summary>
        public bool IsEnglishProficiencyMet{ get; set; }

        /// <summary>
        /// the reason English language proficiency is not required
        /// </summary>
        [MaxLength(REMARKS_MAX_LENGTH)]
        public string EnglishProficiencyNotReqdReason { get; set; }

        /// <summary>
        /// the tuition expense
        /// </summary>
        public decimal? TuitionExpense { get; set; }

        /// <summary>
        /// The living expense
        /// </summary>
        public decimal? LivingExpense { get; set; }

        /// <summary>
        /// Dependent Expense
        /// </summary>
        public decimal? DependentExpense { get; set; }

        /// <summary>
        /// Other Expense
        /// </summary>
        public decimal? OtherExpense { get; set; }

        /// <summary>
        /// Funding from the visiting participant's government
        /// </summary>
        [MaxLength(REMARKS_MAX_LENGTH)]
        public string ExpenseRemarks { get; set; }

        /// <summary>
        /// Funding from the student
        /// </summary>
        public decimal? PersonalFunding { get; set; }

        /// <summary>
        /// Funding from the school
        /// </summary>
        public decimal? SchoolFunding { get; set; }

        /// <summary>
        /// Remarks for funding from the school
        /// </summary>
        [MaxLength(REMARKS_MAX_LENGTH)]
        public string SchoolFundingRemarks { get; set; }

        /// <summary>
        /// Funding from other source
        /// </summary>
        public decimal? OtherFunding { get; set; }

        /// <summary>
        /// Remarks for funding from another source
        /// </summary>
        public string OtherFundingRemarks { get; set; }

        /// <summary>
        /// Funding from student employment
        /// </summary>
        public decimal? EmploymentFunding { get; set; }

        //Relationships

        /// <summary>
        /// The associated participant record
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// create/update time and user
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets the participant person.
        /// </summary>
        [ForeignKey("ParticipantId")]
        public virtual ParticipantPerson ParticipantPerson { get; set; }

    }
}
