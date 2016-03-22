﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 
namespace ECA.Business.Sevis.Model.TransLog {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("TransactionLog", Namespace="", IsNullable=false)]
    public partial class TransactionLogType {
        
        private TransactionLogTypeBatchHeader batchHeaderField;
        
        private TransactionLogTypeBatchDetail batchDetailField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransactionLogTypeBatchHeader BatchHeader {
            get {
                return this.batchHeaderField;
            }
            set {
                this.batchHeaderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransactionLogTypeBatchDetail BatchDetail {
            get {
                return this.batchDetailField;
            }
            set {
                this.batchDetailField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TransactionLogTypeBatchHeader {
        
        private string batchIDField;
        
        private string orgIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BatchID {
            get {
                return this.batchIDField;
            }
            set {
                this.batchIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string OrgID {
            get {
                return this.orgIDField;
            }
            set {
                this.orgIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EmployerLogType {
        
        private string employerNameField;
        
        private System.DateTime startDateField;
        
        private USAddrDoctorResponseType providedAddressField;
        
        private USAddrDoctorResponseType correctedAddressField;
        
        private string addressResultField;
        
        private string addressMessageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string EmployerName {
            get {
                return this.employerNameField;
            }
            set {
                this.employerNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="date")]
        public System.DateTime StartDate {
            get {
                return this.startDateField;
            }
            set {
                this.startDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public USAddrDoctorResponseType ProvidedAddress {
            get {
                return this.providedAddressField;
            }
            set {
                this.providedAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public USAddrDoctorResponseType CorrectedAddress {
            get {
                return this.correctedAddressField;
            }
            set {
                this.correctedAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string AddressResult {
            get {
                return this.addressResultField;
            }
            set {
                this.addressResultField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string AddressMessage {
            get {
                return this.addressMessageField;
            }
            set {
                this.addressMessageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.ice.gov/xmlschema/sevisbatch/Common")]
    public partial class USAddrDoctorResponseType {
        
        private string address1Field;
        
        private string address2Field;
        
        private string cityField;
        
        private System.Nullable<StateCodeType> stateField;
        
        private bool stateFieldSpecified;
        
        private string postalCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Address1 {
            get {
                return this.address1Field;
            }
            set {
                this.address1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public string Address2 {
            get {
                return this.address2Field;
            }
            set {
                this.address2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public System.Nullable<StateCodeType> State {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StateSpecified {
            get {
                return this.stateFieldSpecified;
            }
            set {
                this.stateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PostalCode {
            get {
                return this.postalCodeField;
            }
            set {
                this.postalCodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.ice.gov/xmlschema/sevisbatch/Table")]
    public enum StateCodeType {
        
        /// <remarks/>
        AK,
        
        /// <remarks/>
        AL,
        
        /// <remarks/>
        AR,
        
        /// <remarks/>
        AS,
        
        /// <remarks/>
        AZ,
        
        /// <remarks/>
        CA,
        
        /// <remarks/>
        CO,
        
        /// <remarks/>
        CT,
        
        /// <remarks/>
        DC,
        
        /// <remarks/>
        DE,
        
        /// <remarks/>
        FL,
        
        /// <remarks/>
        FM,
        
        /// <remarks/>
        GA,
        
        /// <remarks/>
        GU,
        
        /// <remarks/>
        HI,
        
        /// <remarks/>
        IA,
        
        /// <remarks/>
        ID,
        
        /// <remarks/>
        IL,
        
        /// <remarks/>
        IN,
        
        /// <remarks/>
        KS,
        
        /// <remarks/>
        KY,
        
        /// <remarks/>
        LA,
        
        /// <remarks/>
        MA,
        
        /// <remarks/>
        MD,
        
        /// <remarks/>
        ME,
        
        /// <remarks/>
        MH,
        
        /// <remarks/>
        MI,
        
        /// <remarks/>
        MN,
        
        /// <remarks/>
        MO,
        
        /// <remarks/>
        MP,
        
        /// <remarks/>
        MS,
        
        /// <remarks/>
        MT,
        
        /// <remarks/>
        NC,
        
        /// <remarks/>
        ND,
        
        /// <remarks/>
        NE,
        
        /// <remarks/>
        NH,
        
        /// <remarks/>
        NJ,
        
        /// <remarks/>
        NM,
        
        /// <remarks/>
        NV,
        
        /// <remarks/>
        NY,
        
        /// <remarks/>
        OH,
        
        /// <remarks/>
        OK,
        
        /// <remarks/>
        OR,
        
        /// <remarks/>
        PA,
        
        /// <remarks/>
        PR,
        
        /// <remarks/>
        PW,
        
        /// <remarks/>
        RI,
        
        /// <remarks/>
        SC,
        
        /// <remarks/>
        SD,
        
        /// <remarks/>
        TN,
        
        /// <remarks/>
        TX,
        
        /// <remarks/>
        UM,
        
        /// <remarks/>
        UT,
        
        /// <remarks/>
        VA,
        
        /// <remarks/>
        VI,
        
        /// <remarks/>
        VT,
        
        /// <remarks/>
        WA,
        
        /// <remarks/>
        WI,
        
        /// <remarks/>
        WV,
        
        /// <remarks/>
        WY,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TippPhase {
        
        private string phaseIdField;
        
        private string phaseNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PhaseId {
            get {
                return this.phaseIdField;
            }
            set {
                this.phaseIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PhaseName {
            get {
                return this.phaseNameField;
            }
            set {
                this.phaseNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SiteOfActivityLogType {
        
        private USAddrDoctorResponseType providedAddressField;
        
        private USAddrDoctorResponseType correctedAddressField;
        
        private string addressResultField;
        
        private string addressMessageField;
        
        private string siteIdField;
        
        private string siteNameField;
        
        private TippPhase[] phaseField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public USAddrDoctorResponseType ProvidedAddress {
            get {
                return this.providedAddressField;
            }
            set {
                this.providedAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public USAddrDoctorResponseType CorrectedAddress {
            get {
                return this.correctedAddressField;
            }
            set {
                this.correctedAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string AddressResult {
            get {
                return this.addressResultField;
            }
            set {
                this.addressResultField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string AddressMessage {
            get {
                return this.addressMessageField;
            }
            set {
                this.addressMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SiteId {
            get {
                return this.siteIdField;
            }
            set {
                this.siteIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SiteName {
            get {
                return this.siteNameField;
            }
            set {
                this.siteNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Phase", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TippPhase[] Phase {
            get {
                return this.phaseField;
            }
            set {
                this.phaseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ResultType {
        
        private string errorCodeField;
        
        private string errorMessageField;
        
        private bool statusField;
        
        private bool statusFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ErrorCode {
            get {
                return this.errorCodeField;
            }
            set {
                this.errorCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ErrorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool statusSpecified {
            get {
                return this.statusFieldSpecified;
            }
            set {
                this.statusFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BatchDetailType {
        
        private string resultCodeField;
        
        private System.DateTime dateTimeStampField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string resultCode {
            get {
                return this.resultCodeField;
            }
            set {
                this.resultCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime dateTimeStamp {
            get {
                return this.dateTimeStampField;
            }
            set {
                this.dateTimeStampField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TransactionLogTypeBatchDetail {
        
        private TransactionLogTypeBatchDetailUpload uploadField;
        
        private TransactionLogTypeBatchDetailProcess processField;
        
        private TransactionLogTypeBatchDetailDownload downloadField;
        
        private bool statusField;
        
        private string systemField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransactionLogTypeBatchDetailUpload Upload {
            get {
                return this.uploadField;
            }
            set {
                this.uploadField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransactionLogTypeBatchDetailProcess Process {
            get {
                return this.processField;
            }
            set {
                this.processField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransactionLogTypeBatchDetailDownload Download {
            get {
                return this.downloadField;
            }
            set {
                this.downloadField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string system {
            get {
                return this.systemField;
            }
            set {
                this.systemField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TransactionLogTypeBatchDetailUpload : BatchDetailType {
        
        private string fileNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FileName {
            get {
                return this.fileNameField;
            }
            set {
                this.fileNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TransactionLogTypeBatchDetailProcess : BatchDetailType {
        
        private TransactionLogTypeBatchDetailProcessRecordCount recordCountField;
        
        private TransactionLogTypeBatchDetailProcessRecord[] recordField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransactionLogTypeBatchDetailProcessRecordCount RecordCount {
            get {
                return this.recordCountField;
            }
            set {
                this.recordCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Record", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransactionLogTypeBatchDetailProcessRecord[] Record {
            get {
                return this.recordField;
            }
            set {
                this.recordField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TransactionLogTypeBatchDetailProcessRecordCount {
        
        private string successField;
        
        private string failureField;
        
        private string totalField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="integer")]
        public string Success {
            get {
                return this.successField;
            }
            set {
                this.successField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="integer")]
        public string Failure {
            get {
                return this.failureField;
            }
            set {
                this.failureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="integer")]
        public string Total {
            get {
                return this.totalField;
            }
            set {
                this.totalField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TransactionLogTypeBatchDetailProcessRecord {
        
        private string userDefinedAField;
        
        private string userDefinedBField;
        
        private System.DateTime processDateField;
        
        private ResultType resultField;
        
        private SiteOfActivityLogType[] siteOfActivityField;
        
        private TransactionLogTypeBatchDetailProcessRecordDependent[] dependentField;
        
        private EmployerLogType[] employerField;
        
        private USAddrDoctorResponseType physicalProvidedAddressField;
        
        private USAddrDoctorResponseType physicalCorrectedAddressField;
        
        private string physicalAddressResultField;
        
        private string physicalAddressMessageField;
        
        private USAddrDoctorResponseType mailingProvidedAddressField;
        
        private USAddrDoctorResponseType mailingCorrectedAddressField;
        
        private string mailingAddressResultField;
        
        private string mailingAddressMessageField;
        
        private string sevisIDField;
        
        private string userIDField;
        
        private string requestIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UserDefinedA {
            get {
                return this.userDefinedAField;
            }
            set {
                this.userDefinedAField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UserDefinedB {
            get {
                return this.userDefinedBField;
            }
            set {
                this.userDefinedBField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime ProcessDate {
            get {
                return this.processDateField;
            }
            set {
                this.processDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ResultType Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SiteOfActivity", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SiteOfActivityLogType[] SiteOfActivity {
            get {
                return this.siteOfActivityField;
            }
            set {
                this.siteOfActivityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Dependent", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransactionLogTypeBatchDetailProcessRecordDependent[] Dependent {
            get {
                return this.dependentField;
            }
            set {
                this.dependentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Employer", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public EmployerLogType[] Employer {
            get {
                return this.employerField;
            }
            set {
                this.employerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public USAddrDoctorResponseType PhysicalProvidedAddress {
            get {
                return this.physicalProvidedAddressField;
            }
            set {
                this.physicalProvidedAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public USAddrDoctorResponseType PhysicalCorrectedAddress {
            get {
                return this.physicalCorrectedAddressField;
            }
            set {
                this.physicalCorrectedAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PhysicalAddressResult {
            get {
                return this.physicalAddressResultField;
            }
            set {
                this.physicalAddressResultField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PhysicalAddressMessage {
            get {
                return this.physicalAddressMessageField;
            }
            set {
                this.physicalAddressMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public USAddrDoctorResponseType MailingProvidedAddress {
            get {
                return this.mailingProvidedAddressField;
            }
            set {
                this.mailingProvidedAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public USAddrDoctorResponseType MailingCorrectedAddress {
            get {
                return this.mailingCorrectedAddressField;
            }
            set {
                this.mailingCorrectedAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MailingAddressResult {
            get {
                return this.mailingAddressResultField;
            }
            set {
                this.mailingAddressResultField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MailingAddressMessage {
            get {
                return this.mailingAddressMessageField;
            }
            set {
                this.mailingAddressMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sevisID {
            get {
                return this.sevisIDField;
            }
            set {
                this.sevisIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string userID {
            get {
                return this.userIDField;
            }
            set {
                this.userIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string requestID {
            get {
                return this.requestIDField;
            }
            set {
                this.requestIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TransactionLogTypeBatchDetailProcessRecordDependent {
        
        private string userDefinedAField;
        
        private string userDefinedBField;
        
        private string dependentSevisIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UserDefinedA {
            get {
                return this.userDefinedAField;
            }
            set {
                this.userDefinedAField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UserDefinedB {
            get {
                return this.userDefinedBField;
            }
            set {
                this.userDefinedBField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dependentSevisID {
            get {
                return this.dependentSevisIDField;
            }
            set {
                this.dependentSevisIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TransactionLogTypeBatchDetailDownload {
        
        private string resultCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string resultCode {
            get {
                return this.resultCodeField;
            }
            set {
                this.resultCodeField = value;
            }
        }
    }
}
