using ECA.Business.Validation.Model;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ECA.Business.Validation
{
    /// <summary>
    /// SEVIS batch create or update exchange visitors
    /// </summary>
    [Serializable]
    public class SEVISBatchCreateUpdateEV
    {
        public SEVISBatchCreateUpdateEV()
        {
            //BatchHeader = new BatchHeader();
            CreateEV = new List<CreateExchVisitor>();
            UpdateEV = new List<UpdateExchVisitor>();
        }

        /// <summary>
        /// Sevis batch record
        /// </summary>
        [XmlAttribute(AttributeName = "userID")]
        public string userID { get; set; }

        /// <summary>
        /// Sevis batch header
        /// </summary>
        //public BatchHeader BatchHeader { get; set; }

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
