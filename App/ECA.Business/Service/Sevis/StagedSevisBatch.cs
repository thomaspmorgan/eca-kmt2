using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// A StagedSevisBatch represents exchange visitors that are to be sent to sevis in a batch object.
    /// </summary>
    public class StagedSevisBatch
    {
        private int maxCreate;
        private int maxUpdate;
        private List<ExchangeVisitor> exchangeVisitors;

        /// <summary>
        /// Creates a new default StagedSevisBatch.
        /// <param name="batchId">The id of the batch this staged batch belongs to.</param>
        /// 
        /// </summary>
        public StagedSevisBatch(Guid batchId, string orgId, int maxCreate, int maxUpdate)
        {
            this.BatchId = batchId;
            this.maxCreate = maxCreate;
            this.maxUpdate = maxUpdate;
            this.exchangeVisitors = new List<ExchangeVisitor>();
            this.SevisBatchProcessing = new SevisBatchProcessing
            {
                BatchId = this.BatchId
            };
            this.SEVISBatchCreateUpdateEV = new SEVISBatchCreateUpdateEV();
            this.SEVISBatchCreateUpdateEV.BatchHeader = new BatchHeaderType
            {
                BatchID = this.BatchId.ToString(),
                OrgID = orgId
            };
            this.SEVISBatchCreateUpdateEV.CreateEV = new SEVISEVBatchTypeExchangeVisitor[0];
            this.SEVISBatchCreateUpdateEV.UpdateEV = new SEVISEVBatchTypeExchangeVisitor1[0];
        }

        /// <summary>
        /// Gets the batch id.
        /// </summary>
        public Guid BatchId { get; private set; }

        /// <summary>
        /// Gets or sets the sevis batch processing entity framework model.
        /// </summary>
        public SevisBatchProcessing SevisBatchProcessing { get; private set; }

        /// <summary>
        /// Gets or sets the serializable sevis model that will be sent to sevis as xml.
        /// </summary>
        public SEVISBatchCreateUpdateEV SEVISBatchCreateUpdateEV { get; private set; }
        
        public List<ExchangeVisitor> GetExchangeVisitors()
        {
            return this.exchangeVisitors;
        }        

        /// <summary>
        /// Serializes the SEVISBatchCreateUpdateEV object and saves it to the SevisBatchProcessing object.
        /// </summary>
        public void SerializeSEVISBatchCreateUpdateEV()
        {
            Contract.Requires(this.SEVISBatchCreateUpdateEV != null, "The SEVISBatchCreateUpdateEV property object must not be null.");
            Contract.Requires(this.SevisBatchProcessing != null, "The sevis batch processing property object must not be null.");
            using (var textWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(SEVISBatchCreateUpdateEV));
                serializer.Serialize(textWriter, this.SEVISBatchCreateUpdateEV);
                var xml = textWriter.ToString();
                this.SevisBatchProcessing.SendString = xml;
            }
        }

        public bool ExceededCreateExchangeVisitorCapacity(int maxCapacity)
        {
            return this.SEVISBatchCreateUpdateEV.CreateEV.Count() >= maxCapacity;
        }

        public bool ExceededUpdateExchangeVisitorCapacity(int maxCapacity)
        {
            return this.SEVISBatchCreateUpdateEV.UpdateEV.Count() >= maxCapacity;
        }

        public bool CanAccomodate(ExchangeVisitor exchangeVisitor)
        {
            if (String.IsNullOrWhiteSpace(exchangeVisitor.SevisId))
            {
                var count = this.SEVISBatchCreateUpdateEV.CreateEV.Count();
                var addedItemCount = 1;
                return count + addedItemCount <= this.maxCreate;
            }
            else
            {
                var count = this.SEVISBatchCreateUpdateEV.UpdateEV.Count();
                var addedItemCount = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection().Count();
                return count + addedItemCount <= this.maxUpdate;
            }
        }

        public void AddExchangeVisitor(ExchangeVisitor exchangeVisitor)
        {   
            if (String.IsNullOrWhiteSpace(exchangeVisitor.SevisId))
            {
                var list = this.SEVISBatchCreateUpdateEV.CreateEV.ToList();
                list.Add(exchangeVisitor.GetSEVISBatchTypeExchangeVisitor());
                this.SEVISBatchCreateUpdateEV.CreateEV = list.ToArray();
                if(this.SEVISBatchCreateUpdateEV.CreateEV.Count() > maxCreate)
                {
                    throw new NotSupportedException("This StagedSevisBatch can not accomodate the given exchange visitor.");
                }
            }
            else
            {
                var list = this.SEVISBatchCreateUpdateEV.UpdateEV.ToList();
                list.AddRange(exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection());
                this.SEVISBatchCreateUpdateEV.UpdateEV = list.ToArray();
                if (this.SEVISBatchCreateUpdateEV.UpdateEV.Count() > maxUpdate)
                {
                    throw new NotSupportedException("This StagedSevisBatch can not accomodate the given exchange visitor.");
                }
            }
            this.exchangeVisitors.Add(exchangeVisitor);
        }
    }
}
