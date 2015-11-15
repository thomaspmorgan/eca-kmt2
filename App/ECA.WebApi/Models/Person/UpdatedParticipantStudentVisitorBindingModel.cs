using System;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing membership
    /// </summary>
    public class UpdatedParticipantStudentVisitorBindingModel
    {
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
            /// Convert binding model to business model 
            /// </summary>
            /// <param name="user">The user updating the membership</param>
            /// <returns>Update membership business model</returns>
            public UpdatedParticipantStudentVisitor ToUpdatedParticipantStudentVisitor(User user)
        {
            return new UpdatedParticipantStudentVisitor(
                updater: user,
                participantId: this.ParticipantId,
                educationLevelId: this.EducationLevelId,
                educationLevelOtherRemarks: this.EducationLevelOtherRemarks,
                primaryMajorId: this.PrimaryMajorId,
                secondaryMajorId: this.SecondaryMajorId,
                minorId: this.MinorId,
                lengthOfStudy: this.LengthOfStudy,
                isEnglishLanguageProficiencyReqd: this.IsEnglishLanguageProficiencyReqd,
                isEnglishLanguageProficiencyMet: this.IsEnglishLanguageProficiencyMet,
                englishLanguageProficiencyNotReqdReason: this.EnglishLanguageProficiencyNotReqdReason,
                tuitionExpense: this.TuitionExpense,
                livingExpense: this.LivingExpense,
                dependentExpense: this.DependentExpense,
                otherExpense: this.OtherExpense,
                expenseRemarks: this.ExpenseRemarks,
                personalFunding: this.PersonalFunding,
                schoolFunding: this.SchoolFunding,
                schoolFundingRemarks: this.SchoolFundingRemarks,
                otherFunding: this.OtherFunding,
                otherFundingRemarks: this.OtherFundingRemarks,
                employmentFunding: this.EmploymentFunding
            );
        }
    }
}