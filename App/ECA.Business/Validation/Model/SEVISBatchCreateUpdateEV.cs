using ECA.Business.Validation.Model;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;

namespace ECA.Business.Validation
{
    /// <summary>
    /// SEVIS batch create or update exchange visitors
    /// </summary>
    [Validator(typeof(SEVISBatchCreateUpdateEVValidator))]
    [Serializable]
    public class SEVISBatchCreateUpdateEV
    {
        public SEVISBatchCreateUpdateEV()
        {
            BatchHeader = new BatchHeader();
            CreateEV = new List<CreateExchVisitor>();
            UpdateEV = new List<UpdateExchVisitor>();
        }

        /// <summary>
        /// Sevis batch record
        /// </summary>
        public string userID { get; set; }

        /// <summary>
        /// Sevis batch header
        /// </summary>
        public BatchHeader BatchHeader { get; set; }

        /// <summary>
        /// Create an exchange visitor record (250 max)
        /// </summary>
        public List<CreateExchVisitor> CreateEV { get; set; }

        /// <summary>
        /// Update an exchange visitor record (250 max)
        /// </summary>
        public List<UpdateExchVisitor> UpdateEV { get; set; }
    }
}
