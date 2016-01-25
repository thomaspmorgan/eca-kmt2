﻿using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(SiteOfActivityExemptUpdateValidator))]
    public class SiteOfActivityExemptUpdate : SiteOfActivityExempt
    {
        public SiteOfActivityExemptUpdate()
        { }

        /// <summary>
        /// Print request indicator
        /// </summary>
        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }
    }
}