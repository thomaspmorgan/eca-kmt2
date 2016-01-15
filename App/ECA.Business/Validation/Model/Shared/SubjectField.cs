﻿using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// Subject or field of study
    /// </summary>
    [Validator(typeof(SubjectFieldValidator))]
    public class SubjectField
    {
        public SubjectField()
        { }

        /// <summary>
        /// Code for subject (format: 12.1234)
        /// </summary>
        public string SubjectFieldCode { get; set; }

        /// <summary>
        /// Foreign degree level (required if intern)
        /// </summary>
        public string ForeignDegreeLevel { get; set; }

        /// <summary>
        /// Foreign field of study
        /// </summary>
        public string ForeignFieldOfStudy { get; set; }

        public string Remarks { get; set; }
    }
}