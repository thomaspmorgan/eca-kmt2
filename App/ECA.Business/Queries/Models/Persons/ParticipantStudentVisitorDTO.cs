using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A ParticipantStudentVisitorDTO is used to represent a participant that is a Student Visitor in the ECA system and their associated related information.
    /// </summary>
    public class ParticipantStudentVisitorDTO
    {
        public ParticipantStudentVisitorDTO()
        {

        }
        /// <summary>
        /// Gets or sets the students id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Getst or sets the student visitor's project Id
        /// </summary>
        public int ProjectId { get; set; }


        /// <summary>
        /// Gets or sets the student's IssueReason id
        /// </summary>
        public int IssueReasonId { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Issue Reason
        /// </summary>
        public string IssueReason { get; set; }

        /// <summary>
        /// Gets or sets the student's education Level id
        /// </summary>
        public int EducationLevelId { get; set; }

        /// <summary>
        /// Gets or sets the student's education Level
        /// </summary>
        public string EducationLevel { get; set; }

        /// <summary>
        /// Gets or sets the student's education Level
        /// </summary>
        public string EducationLevelOtherRemarks { get; set; }

        /// <summary>
        /// Gets or sets the student's PrimaryMajor Id
        /// </summary>
        public int PrimaryMajorId { get; set; }

        /// <summary>
        /// Gets or sets the student's PrimaryMajor
        /// </summary>
        public string PrimaryMajor { get; set; }

        /// <summary>
        /// Gets or sets the student's SecondaryMajor Id
        /// </summary>
        public int? SecondaryMajorId { get; set; }

        /// <summary>
        /// Gets or sets the student's SecondaryMajor
        /// </summary>
        public string SecondaryMajor { get; set; }

        /// <summary>
        /// Gets or sets the student's Minor Id
        /// </summary>
        public int? MinorId { get; set; }

        /// <summary>
        /// Gets or sets the student's Minor
        /// </summary>
        public string Minor { get; set; }

        /// <summary>
        /// Gets or sets the student's length of study in months
        /// </summary>
        public int? LengthOfStudy { get; set; }

        /// <summary>
        /// Does the student require English language proficiency
        /// </summary>
        public bool IsEnglishLanguageProficiencyReqd { get; set; }

        /// <summary>
        /// Does the student meet English language proficiency
        /// </summary>
        public bool IsEnglishLanguageProficiencyMet { get; set; }

        /// <summary>
        /// Gets or sets a string for why English Language proficiency is not required.
        /// </summary>
        public string EnglishLanguageProficiencyNotReqdReason { get; set; }

        /// <summary>
        /// Get or sets the Tuition expense
        /// </summary>
        public decimal? TuitionExpense { get; set; }

        /// <summary>
        /// Get or sets the Living expense
        /// </summary>
        public decimal? LivingExpense { get; set; }

        /// <summary>
        /// Get or sets the Dependent expense
        /// </summary>
        public decimal? DependentExpense { get; set; }

        /// <summary>
        /// Get or sets the Other expense
        /// </summary>
        public decimal? OtherExpense { get; set; }

        /// <summary>
        /// Get or sets the expense remarks
        /// </summary>
        public string ExpenseRemarks { get; set; }

        /// <summary>
        /// Get or sets the Personal funding
        /// </summary>
        public decimal? PersonalFunding { get; set; }

        /// <summary>
        /// Get or sets the School Funding
        /// </summary>
        public decimal? SchoolFunding { get; set; }

        /// <summary>
        /// Get or sets the School funding remarks
        /// </summary>
        public string SchoolFundingRemarks { get; set; }

        /// <summary>
        /// Get or sets the Other funding
        /// </summary>
        public decimal? OtherFunding { get; set; }

        /// <summary>
        /// Get or sets the Other funding remarks
        /// </summary>
        public string OtherFundingRemarks { get; set; }

        /// <summary>
        /// Get or sets the Employment funding
        /// </summary>
        public decimal? EmploymentFunding { get; set; }

    }
}
