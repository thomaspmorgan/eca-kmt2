using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// Subject or field of study
    /// </summary>
    [Validator(typeof(SubjectFieldValidator))]
    public class SubjectField : IRemarkable
    {
        public SubjectField(string subjectFieldCode, string foreignDegreeLevel, string foreignFieldOfStudy, string remarks)
        {
            this.SubjectFieldCode = subjectFieldCode;
            this.ForeignDegreeLevel = foreignDegreeLevel;
            this.ForeignFieldOfStudy = foreignFieldOfStudy;
            this.Remarks = remarks;
        }

        /// <summary>
        /// Gets or sets the subject field code for example: 12.1234.
        /// </summary>
        public string SubjectFieldCode { get; set; }

        /// <summary>
        /// Gets or sets the foreign degree level.
        /// </summary>
        public string ForeignDegreeLevel { get; set; }

        /// <summary>
        /// Gets or sets the foreign field of study.
        /// </summary>
        public string ForeignFieldOfStudy { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Returns the EVPersonTypeSubjectField sevis model instance.
        /// </summary>
        /// <returns>The EVPersonTypeSubjectField sevis model instance.</returns>
        public EVPersonTypeSubjectField GetEVPersonTypeSubjectField()
        {
            return new EVPersonTypeSubjectField
            {
                Remarks = this.Remarks,
                SubjectFieldCode = this.SubjectFieldCode.GetProgSubjectCodeType()
            };
        }

        /// <summary>
        /// Returns the SEVISEVBatchTypeExchangeVisitorProgramEditSubject sevis model instance.
        /// </summary>
        /// <returns>The SEVISEVBatchTypeExchangeVisitorProgramEditSubject sevis model instance.</returns>
        public SEVISEVBatchTypeExchangeVisitorProgramEditSubject GetSEVISEVBatchTypeExchangeVisitorProgramEditSubject()
        {
            return new SEVISEVBatchTypeExchangeVisitorProgramEditSubject
            {
                printForm = true,
                Remarks = this.Remarks,
                SubjectFieldRemarks = this.Remarks,
                SubjectFieldCode = this.SubjectFieldCode.GetProgSubjectCodeType()
            };
        }
    }
}