using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// The common namespace prefix.
        /// </summary>
        public const string COMMON_NAMESPACE_PREFIX = "common";

        /// <summary>
        /// The sevis table namespace prefix.
        /// </summary>
        public const string TABLE_NAMESPACE_PREFIX = "table";

        /// <summary>
        /// The exchange visitor namesapce prefix.
        /// </summary>
        public const string EXCHANGE_VISITOR_NAMESPACE_PREFIX = "noNamespaceSchemaLocation";

        /// <summary>
        /// The xsd namespace prefix.
        /// </summary>
        public const string XSD_NAMESPACE_PREFIX = "xsd";

        /// <summary>
        /// The xsi namespace prefix.
        /// </summary>
        public const string XSI_NAMESPACE_PREFIX = "xsi";

        /// <summary>
        /// The xsd schema url.
        /// </summary>
        public const string XSD_NAMESPACE_URL = "http://www.w3.org/2001/XMLSchema";

        /// <summary>
        /// The xsi namespace url.
        /// </summary>
        public const string XSI_NAMESPACE_URL = "http://www.w3.org/2001/XMLSchema-instance";

        /// <summary>
        /// The common namespace url.
        /// </summary>
        public const string COMMON_NAMESPACE_URL = "http://www.ice.gov/xmlschema/sevisbatch/alpha/Common.xsd";

        /// <summary>
        /// The table namespace url.
        /// </summary>
        public const string TABLE_NAMESPACE_URL = "http://www.ice.gov/xmlschema/sevisbatch/alpha/SEVISTable.xsd";

        /// <summary>
        /// The exchange visitor namespace url.
        /// </summary>
        public const string EXCHANGE_VISITOR_NAMESPACE_URL = "http://www.ice.gov/xmlschema/sevisbatch/alpha/Create-UpdateExchangeVisitor.xsd";

        private List<ExchangeVisitor> exchangeVisitors;

        /// <summary>
        /// Creates a new default StagedSevisBatch.
        /// <param name="batchId">The id of the batch this staged batch belongs to.</param>
        /// <param name="maxCreateExchangeVisitorRecordsPerBatch">The maximum number of create records this batch will contain.</param>
        /// <param name="maxUpdateExchangeVisitorRecordPerBatch">The maximum number of update records this batch will contain.</param>
        /// <param name="sevisOrgId">The sevis org, or program, id to submit this batch with.</param>
        /// <param name="sevisUsername">The sevis username all exchange visitors will be submitted with.</param>
        /// </summary>
        public StagedSevisBatch(
            Guid batchId,
            string sevisUsername,
            string sevisOrgId,
            int maxCreateExchangeVisitorRecordsPerBatch = MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT,
            int maxUpdateExchangeVisitorRecordPerBatch = MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT)
        {
            Contract.Requires(batchId != Guid.Empty, "The batch id must not be the empty guid.");

            this.BatchId = GetBatchId(batchId);
            this.SevisUsername = sevisUsername;
            this.SevisOrgId = sevisOrgId;
            this.exchangeVisitors = new List<ExchangeVisitor>();
            this.SevisBatchProcessing = new SevisBatchProcessing
            {
                BatchId = this.BatchId,
                SevisOrgId = this.SevisOrgId,
                SevisUsername = this.SevisUsername,
                UploadTries = 0,
                UploadCooldown = null
            };
            this.SEVISBatchCreateUpdateEV = new SEVISBatchCreateUpdateEV();
            this.SEVISBatchCreateUpdateEV.userID = sevisUsername;
            this.SEVISBatchCreateUpdateEV.BatchHeader = new BatchHeaderType
            {
                BatchID = this.BatchId.ToString(),
                OrgID = this.SevisOrgId
            };
            this.SEVISBatchCreateUpdateEV.CreateEV = null;
            this.SEVISBatchCreateUpdateEV.UpdateEV = null;
            this.MaxCreateExchangeVisitorRecordsPerBatch = maxCreateExchangeVisitorRecordsPerBatch;
            this.MaxUpdateExchangeVisitorRecordPerBatch = maxUpdateExchangeVisitorRecordPerBatch;
        }

        /// <summary>
        /// Gets the sevis username the exchange visitors will be submitted with.
        /// </summary>
        public string SevisUsername { get; private set; }

        /// <summary>
        /// Gets the sevis org or program id this batch will be submitted with.
        /// </summary>
        public string SevisOrgId { get; private set; }

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
        /// Returns the namespaces to add to the sevis exchange visitor document.
        /// </summary>
        /// <returns>The exchange visitor sevis xml document namespaces.</returns>
        public XmlSerializerNamespaces GetExchangeVisitorNamespaces()
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(COMMON_NAMESPACE_PREFIX, COMMON_NAMESPACE_URL);
            namespaces.Add(TABLE_NAMESPACE_PREFIX, TABLE_NAMESPACE_URL);
            namespaces.Add(EXCHANGE_VISITOR_NAMESPACE_PREFIX, EXCHANGE_VISITOR_NAMESPACE_URL);
            namespaces.Add(XSD_NAMESPACE_PREFIX, XSD_NAMESPACE_URL);
            namespaces.Add(XSI_NAMESPACE_PREFIX, XSI_NAMESPACE_URL);
            return namespaces;
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
                serializer.Serialize(textWriter, this.SEVISBatchCreateUpdateEV, GetExchangeVisitorNamespaces());
                var xml = textWriter.ToString();
                this.SevisBatchProcessing.SendString = xml;
            }
        }

        /// <summary>
        /// Returns true, if the given exchange visitor can be added to this batch; otherwise, false.
        /// </summary>
        /// <param name="exchangeVisitor">The exchange visitor to add.</param>
        /// <returns>True, if the given exchange visitor can be added to this batch; otherwise false.</returns>
        public bool CanAccomodate(ExchangeVisitor exchangeVisitor, string sevisUsername, string sevisOrgId)
        {
            Contract.Requires(exchangeVisitor != null, "The exchange visitor must not be null.");
            if (this.SevisUsername != sevisUsername|| this.SevisOrgId != sevisOrgId)
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(exchangeVisitor.SevisId))
            {
                var count = this.SEVISBatchCreateUpdateEV.CreateEV == null ? 0 : this.SEVISBatchCreateUpdateEV.CreateEV.Count();
                var addedItemCount = 1;
                return count + addedItemCount <= this.MaxCreateExchangeVisitorRecordsPerBatch;
            }
            else
            {
                var count = this.SEVISBatchCreateUpdateEV.UpdateEV == null ? 0 : this.SEVISBatchCreateUpdateEV.UpdateEV.Count();
                var addedItemCount = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUsername).Count();
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
                list.Add(exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(this.SevisUsername));
                this.SEVISBatchCreateUpdateEV.CreateEV = list.ToArray();
                if (this.SEVISBatchCreateUpdateEV.CreateEV.Count() > this.MaxCreateExchangeVisitorRecordsPerBatch)
                {
                    throw new NotSupportedException(message);
                }
            }
            else
            {
                var list = this.SEVISBatchCreateUpdateEV.UpdateEV != null ? this.SEVISBatchCreateUpdateEV.UpdateEV.ToList() : new List<SEVISEVBatchTypeExchangeVisitor1>();
                list.AddRange(exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(this.SevisUsername));
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
