using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    public class ExchangeVisitorUpdate
    {
        public ExchangeVisitorUpdate()
        {
            Biographical = new BiographicalUpdate();
            Dependent = new UpdatedDependent();
            FinancialInfo = new FinancialInfoUpdate();
            Program = new Program();
            Reprint = new ReprintFormUpdate();
            SiteOfActivity = new SiteOfActivityUpdate();
            TIPP = new TippUpdate();
            Status = new StatusUpdate();
            Validate = new ValidateParticipant();
            Reprint7002 = new Reprint7002();
        }
        
        /// <summary>
        /// Sevis batch ID
        /// </summary>
        public string sevisID { get; set; }

        /// <summary>
        /// Request identifier
        /// </summary>
        public string requestID { get; set; }

        /// <summary>
        /// SEVIS user id
        /// </summary>
        public string userID { get; set; }

        /// <summary>
        /// Status code of student
        /// </summary>
        public string statusCode { get; set; }

        /// <summary>
        /// User defined field A
        /// </summary>
        public string UserDefinedA { get; set; }

        /// <summary>
        /// User defined field B
        /// </summary>
        public string UserDefinedB { get; set; }

        /// <summary>
        /// Biographical information
        /// </summary>
        public BiographicalUpdate Biographical { get; set; }
        
        /// <summary>
        /// Edit dependent
        /// </summary>
        public UpdatedDependent Dependent { get; set; }

        /// <summary>
        /// Update financial info
        /// </summary>
        public FinancialInfoUpdate FinancialInfo { get; set; }

        /// <summary>
        /// Program events
        /// </summary>
        public Program Program { get; set; }

        /// <summary>
        /// Reprint form DS-2019
        /// </summary>
        public ReprintFormUpdate Reprint { get; set; }

        /// <summary>
        /// Site of activity events
        /// </summary>
        public SiteOfActivityUpdate SiteOfActivity { get; set; }

        /// <summary>
        /// T/IPP information
        /// </summary>
        public TippUpdate TIPP { get; set; }

        /// <summary>
        /// EV status change events
        /// </summary>
        public StatusUpdate Status { get; set; }

        /// <summary>
        /// Validate participant
        /// </summary>
        public ValidateParticipant Validate { get; set; }

        /// <summary>
        /// Reprint DS-7002
        /// </summary>
        public Reprint7002 Reprint7002 { get; set; }
    }
}
