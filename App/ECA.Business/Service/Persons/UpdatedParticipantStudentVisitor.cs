using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An UpdatedParticipantPerson is used by a business layer client to update a person that is a project participant.
    /// </summary>
    public class UpdatedParticipantStudentVisitor : IAuditable
    {
        /// <summary>
        /// A class to update a Participant Persons SEVIS info
        /// </summary>
        /// <param name="updater"></param>
        /// <param name="participantId"></param>
        /// <param name="educationLevelId"></param>
        /// <param name="educationLevelOtherRemarks"></param>
        /// <param name="primaryMajorId"></param>
        /// <param name="secondaryMajorId"></param>
        /// <param name="minorId"></param>
        /// <param name="lengthOfStudy"></param>
        /// <param name="isEnglishLanguageProficiencyReqd"></param>
        /// <param name="isEnglishLanguageProficiencyMet"></param>
        /// <param name="englishLanguageProficiencyNotReqdReason"></param>
        /// <param name="tuitionExpense"></param>
        /// <param name="livingExpense"></param>
        /// <param name="dependentExpense"></param>
        /// <param name="otherExpense"></param>
        /// <param name="expenseRemarks"></param>
        /// <param name="personalFunding"></param>
        /// <param name="schoolFunding"></param>
        /// <param name="schoolFundingRemarks"></param>
        /// <param name="otherFunding"></param>
        /// <param name="otherFundingRemarks"></param>
        /// <param name="employmentFunding"></param>
        public UpdatedParticipantStudentVisitor(
            User updater, 
            int participantId, 
            int? educationLevelId,
            string educationLevelOtherRemarks,
            int? primaryMajorId,
            int? secondaryMajorId,
            int? minorId,
            int? lengthOfStudy,
            string studyProject,
            bool isEnglishLanguageProficiencyReqd,
            bool isEnglishLanguageProficiencyMet,
            string englishLanguageProficiencyNotReqdReason,
            decimal? tuitionExpense,
            decimal? livingExpense,
            decimal? dependentExpense,
            decimal? otherExpense,
            string expenseRemarks,
            decimal? personalFunding,
            decimal? schoolFunding,
            string schoolFundingRemarks,
            decimal? otherFunding,
            string otherFundingRemarks,
            decimal? employmentFunding
            )
        {
            this.Audit = new Update(updater);
            this.ParticipantId = participantId;
            this.EducationLevelId = educationLevelId;
            this.EducationLevelOtherRemarks = educationLevelOtherRemarks;
            this.PrimaryMajorId = primaryMajorId;
            this.SecondaryMajorId = secondaryMajorId;
            this.MinorId = minorId;
            this.LengthOfStudy = lengthOfStudy;
            this.StudyProject = studyProject;
            this.IsEnglishLanguageProficiencyReqd = isEnglishLanguageProficiencyReqd;
            this.IsEnglishLanguageProficiencyMet = isEnglishLanguageProficiencyMet;
            this.EnglishLanguageProficiencyNotReqdReason = englishLanguageProficiencyNotReqdReason;
            this.TuitionExpense = tuitionExpense;
            this.LivingExpense = livingExpense;
            this.DependentExpense = dependentExpense;
            this.OtherExpense = otherExpense;
            this.ExpenseRemarks = expenseRemarks;
            this.PersonalFunding = personalFunding;
            this.SchoolFunding = schoolFunding;
            this.SchoolFundingRemarks = schoolFundingRemarks;
            this.OtherFunding = otherFunding;
            this.OtherFundingRemarks = otherFundingRemarks;
            this.EmploymentFunding = employmentFunding;
        }

        /// <summary>
        /// Gets or sets the students id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the student's education Level id
        /// </summary>
        public int? EducationLevelId { get; set; }

        /// <summary>
        /// Gets or sets the student's education Level
        /// </summary>
        public string EducationLevelOtherRemarks { get; set; }

        /// <summary>
        /// Gets or sets the student's PrimaryMajor Id
        /// </summary>
        public int? PrimaryMajorId { get; set; }

        /// <summary>
        /// Gets or sets the student's SecondaryMajor Id
        /// </summary>
        public int? SecondaryMajorId { get; set; }

        /// <summary>
        /// Gets or sets the student's Minor Id
        /// </summary>
        public int? MinorId { get; set; }

        /// <summary>
        /// Gets or sets the student's length of study in months
        /// </summary>
        public int? LengthOfStudy { get; set; }

        /// <summary>
        /// Gets or sets the student's study project
        /// </summary>
        public string StudyProject { get; set; }

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

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
