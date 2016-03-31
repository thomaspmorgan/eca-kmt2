using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// A StagedSevisBatch represents exchange visitors that are to be sent to sevis in a batch object.
    /// </summary>
    public class StagedSevisBatch
    {
        /// <summary>
        /// The default maximum number of records to place in StagedSevisBatch object's create exhange visitor records array.
        /// </summary>
        public const int MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT = 250;

        /// <summary>
        /// The default maximum number of records to place in StagedSevisBatch object's update exhange visitor records array.
        /// </summary>
        public const int MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT = 250;

        private List<ExchangeVisitor> exchangeVisitors;
        
        /// <summary>
        /// Creates a new default StagedSevisBatch.
        /// <param name="batchId">The id of the batch this staged batch belongs to.</param>
        /// 
        /// </summary>
        public StagedSevisBatch(
            Guid batchId, 
            string sevisUserId, 
            string orgId, 
            int maxCreateExchangeVisitorRecordsPerBatch = MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT,
            int maxUpdateExchangeVisitorRecordPerBatch = MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT)
        {
            Contract.Requires(batchId != Guid.Empty, "The batch id must not be the empty guid.");

            this.BatchId = GetBatchId(batchId);
            this.exchangeVisitors = new List<ExchangeVisitor>();
            this.SevisBatchProcessing = new SevisBatchProcessing
            {
                BatchId = this.BatchId
            };
            this.SEVISBatchCreateUpdateEV = new SEVISBatchCreateUpdateEV();
            this.SEVISBatchCreateUpdateEV.userID = sevisUserId;
            this.SEVISBatchCreateUpdateEV.BatchHeader = new BatchHeaderType
            {
                BatchID = this.BatchId.ToString(),
                OrgID = orgId
            };
            this.SEVISBatchCreateUpdateEV.CreateEV = null;
            this.SEVISBatchCreateUpdateEV.UpdateEV = null;
            this.MaxCreateExchangeVisitorRecordsPerBatch = maxCreateExchangeVisitorRecordsPerBatch;
            this.MaxUpdateExchangeVisitorRecordPerBatch = maxUpdateExchangeVisitorRecordPerBatch;
        }

        /// <summary>
        /// Gets the batch id.
        /// </summary>
        public string BatchId { get; private set; }

        /// <summary>
        /// Gets or sets the sevis batch processing entity framework model.
        /// </summary>
        public SevisBatchProcessing SevisBatchProcessing { get; private set; }

        /// <summary>
        /// Gets or sets the serializable sevis model that will be sent to sevis as xml.
        /// </summary>
        public SEVISBatchCreateUpdateEV SEVISBatchCreateUpdateEV { get; private set; }

        /// <summary>
        /// Gets the maximum number of records to place in the CreateEV array.
        /// </summary>
        public int MaxCreateExchangeVisitorRecordsPerBatch { get; private set; }

        /// <summary>
        /// Gets the maximum number of records to place in the UpdateEV array.
        /// </summary>
        public int MaxUpdateExchangeVisitorRecordPerBatch { get; private set; }
        /// <summary>
        /// Gets or sets the IsSaved flag.
        /// </summary>
        public bool IsSaved
        {
            get; set;
        }
        
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

        /// <summary>
        /// Returns true, if the given exchange visitor can be added to this batch; otherwise, false.
        /// </summary>
        /// <param name="exchangeVisitor">The exchange visitor to add.</param>
        /// <returns>True, if the given exchange visitor can be added to this batch; otherwise false.</returns>
        public bool CanAccomodate(ExchangeVisitor exchangeVisitor)
        {
            Contract.Requires(exchangeVisitor != null, "The exchange visitor must not be null.");
            if (String.IsNullOrWhiteSpace(exchangeVisitor.SevisId))
            {
                var count = this.SEVISBatchCreateUpdateEV.CreateEV == null ? 0 : this.SEVISBatchCreateUpdateEV.CreateEV.Count();
                var addedItemCount = 1;
                return count + addedItemCount <= this.MaxCreateExchangeVisitorRecordsPerBatch;
            }
            else
            {
                var count = this.SEVISBatchCreateUpdateEV.UpdateEV == null ? 0 : this.SEVISBatchCreateUpdateEV.UpdateEV.Count();
                var addedItemCount = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection().Count();
                return count + addedItemCount <= this.MaxCreateExchangeVisitorRecordsPerBatch;
            }
        }

        /// <summary>
        /// Adds the given exchange visitor to this batch.  If this batch can not be added an exception is thrown.
        /// </summary>
        /// <param name="exchangeVisitor">The exchange visitor to add.</param>
        public void AddExchangeVisitor(ExchangeVisitor exchangeVisitor)
        {
            Contract.Requires(exchangeVisitor != null, "The exchange visitor must not be null.");
            Contract.Requires(exchangeVisitor.Person != null, "The exchange visitor person must not be null.");
            
            var existingExchangeVisitor = this.exchangeVisitors.Where(x => x.Person.ParticipantId == exchangeVisitor.Person.ParticipantId).FirstOrDefault();
            if (existingExchangeVisitor != null)
            {
                return;
            }
            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            if (String.IsNullOrWhiteSpace(exchangeVisitor.SevisId))
            {
                var list = this.SEVISBatchCreateUpdateEV.CreateEV != null ? this.SEVISBatchCreateUpdateEV.CreateEV.ToList() : new List<SEVISEVBatchTypeExchangeVisitor>();
                list.Add(exchangeVisitor.GetSEVISBatchTypeExchangeVisitor());
                this.SEVISBatchCreateUpdateEV.CreateEV = list.ToArray();
                if(this.SEVISBatchCreateUpdateEV.CreateEV.Count() > this.MaxCreateExchangeVisitorRecordsPerBatch)
                {
                    throw new NotSupportedException(message);
                }
            }
            else
            {
                var list = this.SEVISBatchCreateUpdateEV.UpdateEV != null ? this.SEVISBatchCreateUpdateEV.UpdateEV.ToList() : new List<SEVISEVBatchTypeExchangeVisitor1>();
                list.AddRange(exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection());
                this.SEVISBatchCreateUpdateEV.UpdateEV = list.ToArray();
                if (this.SEVISBatchCreateUpdateEV.UpdateEV.Count() > this.MaxUpdateExchangeVisitorRecordPerBatch)
                {
                    throw new NotSupportedException(message);
                }
            }
            this.exchangeVisitors.Add(exchangeVisitor);
        }

        /// <summary>
        /// Returns the batch id from the given guid.
        /// </summary>
        /// <param name="guid">The batch id guid.</param>
        /// <returns>The guid to convert to a batch id.</returns>
        public string GetBatchId(Guid guid)
        {
            Contract.Requires(guid != Guid.Empty, "The provided guid must not be the empty guid.");
            var maxLength = 14;
            var guidString = guid.ToString();
            guidString = guidString.Replace("-", String.Empty);            
            var index = guidString.Length - maxLength;
            return guidString.Substring(index);
        }
    }
}
