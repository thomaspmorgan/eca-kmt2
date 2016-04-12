using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ECA.Business.Sevis.Model
{
    /// <summary>
    /// The DispositionCode class is used to hold sevis api disposition codes.
    /// </summary>
    public class DispositionCode
    {
        private DispositionCode(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }

        /// <summary>
        /// Gets the code value.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Gets the error code description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The sevis api success code.
        /// </summary>
        public const string SUCCESS_CODE = "S0000";

        /// <summary>
        /// The sevis api general upload and download failure code.
        /// </summary>
        public const string GENERAL_UPLOAD_DOWNLOAD_FAILURE_CODE = "S0001";

        /// <summary>
        /// The sevis api invalid org info code.
        /// </summary>
        public const string INVALID_ORGANIZATION_INFO_CODE = "S0002";

        /// <summary>
        /// The invalid user id code.
        /// </summary>
        public const string INVALID_USER_ID_CODE = "S0003";

        /// <summary>
        /// The duplicate batch id code.
        /// </summary>
        public const string DUPLICATE_BATCH_ID_CODE = "S0010";

        /// <summary>
        /// The document name invalid code.
        /// </summary>
        public const string DOCUMENT_NAME_INVALID_CODE = "S0011";

        /// <summary>
        /// The xml is not well formed error code.
        /// </summary>
        public const string XML_NOT_WELL_FORMED_ERROR_CODE = "S0012";

        /// <summary>
        /// The xml sent does not comply with the sevis xsd schema.
        /// </summary>
        public const string XML_DOES_NOT_COMPLY_WITH_SEVIS_SCHEMA_ERROR_CODE = "S0013";

        /// <summary>
        /// The business rule violations error code.
        /// </summary>
        public const string BUSINESS_RULE_VIOLATIONS_ERROR_CODE = "S0020";

        /// <summary>
        /// The download unavailable error code.
        /// </summary>
        public const string DOWNLOAD_UNAVAILABLE_BATCH_JOB_NOT_YET_PROCESSED = "S0030";

        /// <summary>
        /// The batch not submitted error code.
        /// </summary>
        public const string DOWNLOAD_INVALID_BATCH_JOB_NOT_YET_SUBMITTED = "S0031";

        /// <summary>
        /// Gets the success disposition code.  Used when the sevis batch was processed successfully.  Used
        /// for upload, download, and processing statuses.
        /// </summary>
        public static DispositionCode Success
        {
            get
            {
                return new DispositionCode(SUCCESS_CODE, "Success");
            }
        }

        /// <summary>
        /// Gets the general upload download failure disposition code.  An upload and download code.
        /// </summary>
        public static DispositionCode GeneralUploadDownloadFailure
        {
            get
            {
                return new DispositionCode(GENERAL_UPLOAD_DOWNLOAD_FAILURE_CODE, "General upload/download failure");
            }
        }

        /// <summary>
        /// Gets the invalid organization information code.  An upload and download code.
        /// </summary>
        public static DispositionCode InvalidOrganizationInformation
        {
            get
            {
                return new DispositionCode(INVALID_ORGANIZATION_INFO_CODE, "Invalid organization information");
            }
        }

        /// <summary>
        /// Gets the invalid user id disposition code.  An upload and download code.
        /// </summary>
        public static DispositionCode InvalidUserId
        {
            get
            {
                return new DispositionCode(INVALID_USER_ID_CODE, "Invalid User Id");
            }
        }

        /// <summary>
        /// Gets the duplicate batch id disposition code.  An upload only code.
        /// </summary>
        public static DispositionCode DuplicateBatchId
        {
            get
            {
                return new DispositionCode(DUPLICATE_BATCH_ID_CODE, "Duplicate Batch ID");
            }
        }

        /// <summary>
        /// Gets the document name invalid disposition code.  An upload only code.
        /// </summary>
        public static DispositionCode DocumentNameInvalid
        {
            get
            {
                return new DispositionCode(DOCUMENT_NAME_INVALID_CODE, "Duplicate name invalid");
            }
        }

        /// <summary>
        /// Gets the malformed xml dispostion code. An upload only code.
        /// </summary>
        public static DispositionCode MalformedXml
        {
            get
            {
                return new DispositionCode(XML_NOT_WELL_FORMED_ERROR_CODE, "XML is not well-formed or does not agree with SEVIS-specific POSTing requirements.");
            }
        }

        /// <summary>
        /// Gets the invalid xml disposition code.  An upload only code.
        /// </summary>
        public static DispositionCode InvalidXml
        {
            get
            {
                return new DispositionCode(XML_DOES_NOT_COMPLY_WITH_SEVIS_SCHEMA_ERROR_CODE, "XML does not comply with SEVIS schema.");
            }
        }

        /// <summary>
        /// Gets the business rule violations disposition code.  A process only code.
        /// </summary>
        public static DispositionCode BusinessRuleViolations
        {
            get
            {
                return new DispositionCode(BUSINESS_RULE_VIOLATIONS_ERROR_CODE, "One or more records failed processing due to business rule violations.");
            }
        }

        /// <summary>
        /// Gets the batch not yet process disposition code.  A download only code.
        /// </summary>
        public static DispositionCode BatchNotYetProcessed
        {
            get
            {
                return new DispositionCode(DOWNLOAD_UNAVAILABLE_BATCH_JOB_NOT_YET_PROCESSED, "Download unavailable – batch job not yet processed.");
            }
        }

        /// <summary>
        /// Gets the batch never submitted disposition code.  A download only code.
        /// </summary>
        public static DispositionCode BatchNeverSubmitted
        {
            get
            {
                return new DispositionCode(DOWNLOAD_INVALID_BATCH_JOB_NOT_YET_SUBMITTED, "Download invalid – batch job never submitted.");
            }
        }

        /// <summary>
        /// Returns a string of this comparison type.
        /// </summary>
        /// <returns>A string of this comparison type.</returns>
        public override string ToString()
        {
            return this.Code;
        }

        /// <summary>
        /// Returns true if the given object equals this object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if the given object equals this object.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var otherType = obj as DispositionCode;
            if (otherType == null)
            {
                return false;
            }
            return this.Code == otherType.Code;

        }

        /// <summary>
        /// Returns a hash of this object.
        /// </summary>
        /// <returns>A hash of this object.</returns>
        public override int GetHashCode()
        {
            return this.Code.GetHashCode();
        }

        /// <summary>
        /// Returns true if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>True if the given instances are equal.</returns>
        public static bool operator ==(DispositionCode a, DispositionCode b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Code == b.Code;
        }

        /// <summary>
        /// Returns false if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>False if the given instances are equal.</returns>
        public static bool operator !=(DispositionCode a, DispositionCode b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a DispositionCode whose values is equal to the given string.
        /// </summary>
        /// <param name="code">The disposition code.</param>
        /// <returns>The DispositionCode type.</returns>
        public static DispositionCode ToDispositionCode(string code)
        {
            Contract.Requires(code != null, "The code must not be null.");
            var dictionary = new Dictionary<string, DispositionCode>();

            Action<DispositionCode> addToDictionary = (dispositionCode) =>
            {
                dictionary.Add(dispositionCode.Code, dispositionCode);
            };
            addToDictionary(DispositionCode.BatchNeverSubmitted);
            addToDictionary(DispositionCode.BatchNotYetProcessed);
            addToDictionary(DispositionCode.BusinessRuleViolations);
            addToDictionary(DispositionCode.DocumentNameInvalid);
            addToDictionary(DispositionCode.DuplicateBatchId);
            addToDictionary(DispositionCode.GeneralUploadDownloadFailure);
            addToDictionary(DispositionCode.InvalidOrganizationInformation);
            addToDictionary(DispositionCode.InvalidUserId);
            addToDictionary(DispositionCode.InvalidXml);
            addToDictionary(DispositionCode.MalformedXml);
            addToDictionary(DispositionCode.Success);

            var c = code.ToUpper().Trim();
            Contract.Assert(dictionary.ContainsKey(c), String.Format("The disposition code [{0}] is not recognized.", code));
            return dictionary[c];
        }
    }
}
