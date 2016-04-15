using ECA.Business.Sevis.Model.TransLog;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.Finance;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// A RequestIdType details what the sevis batch request is related to.
    /// </summary>
    public enum RequestIdType
    {
        /// <summary>
        /// The participant request type.  This is used for biographical data requests.
        /// </summary>
        Participant = 1,

        /// <summary>
        /// The dependent request type.  This is used for all requests relating to dependents only.
        /// </summary>
        Dependent,

        /// <summary>
        /// The financial info request type.  This is used when updating financial info for a participant already registered in sevis.
        /// </summary>
        FinancialInfo,

        /// <summary>
        /// The subject field request type.  This is used when updating subject field info for a participant already registered in sevis.
        /// </summary>
        SubjectField
    }    

    /// <summary>
    /// A RequestId is used to encode information about the individual requests made in a sevis batch, including the participant id or person dependent id,
    /// and the request type.  The format of the string will be [RequestIdType][P|D][ParticipantId|PersonDependentId]
    /// </summary>
    public class RequestId
    {
        private static Regex REQUEST_ID_REGEX = new Regex(@"^\d+[P|D]\d+$");

        private const string REQUEST_ID_FORMAT_STRING = "{0}{1}{2}";

        private const string PARTICIPANT_CHAR = "P";

        private const string DEPENDENT_CHAR = "D";

        private string requestId;

        /// <summary>
        /// Creates a new RequestId with the given person.
        /// </summary>
        /// <param name="person">The person.</param>
        public RequestId(Bio.Person person)
            : this(person.ParticipantId, PARTICIPANT_CHAR, RequestIdType.Participant)
        {
            Contract.Requires(person != null, "The person must not be null.");
        }

        /// <summary>
        /// Creates a new RequestId with the given dependent.
        /// </summary>
        /// <param name="dependent">The dependent.</param>
        public RequestId(Dependent dependent)
            : this(dependent.PersonId, DEPENDENT_CHAR, RequestIdType.Dependent)
        {
            Contract.Requires(dependent != null, "The dependent must not be null.");
        }

        /// <summary>
        /// Creates a new Request Id with the given person and financial info.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="financialInfo"></param>
        public RequestId(Bio.Person person, FinancialInfo financialInfo)
            : this(person.ParticipantId, PARTICIPANT_CHAR, RequestIdType.FinancialInfo)
        {
            Contract.Requires(person != null, "The person must not be null.");
            Contract.Requires(financialInfo != null, "The financial info must not be null.");
        }

        /// <summary>
        /// Creates a new RequestId instance with the given person and subject field.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="subjectField">The subject field.</param>
        public RequestId(Bio.Person person, SubjectField subjectField)
            : this(person.ParticipantId, PARTICIPANT_CHAR, RequestIdType.SubjectField)
        {
            Contract.Requires(person != null, "The person must not be null.");
            Contract.Requires(subjectField != null, "The subject field must not be null.");
        }

        /// <summary>
        /// Creates a new RequestId instance from the given process record.
        /// </summary>
        /// <param name="record">The process record.</param>
        public RequestId(TransactionLogTypeBatchDetailProcessRecord record)
            :this(record.requestID)
        {
            Contract.Requires(record != null, "The record must not be null.");
        }

        /// <summary>
        /// Creates a new request id with the given string and parses it.
        /// </summary>
        /// <param name="requestId">The request id string.  This value must be created with another RequestId instance.</param>
        public RequestId(string requestId)
        {
            Contract.Requires(requestId != null, "The request id must not be null.");
            if (!REQUEST_ID_REGEX.IsMatch(requestId))
            {
                throw new NotSupportedException("The request id string is not a valid request id.");
            }
            Parse(requestId);
            this.requestId = requestId;
        }

        private RequestId(int objectId, string objectTypeChar, RequestIdType requestIdType)
        {
            this.RequestIdType = requestIdType;
            this.Id = objectId;
            this.requestId = String.Format(REQUEST_ID_FORMAT_STRING, (int)requestIdType, objectTypeChar, objectId);
        }

        private void Parse(string requestId)
        {
            var splitStrings = requestId.Split(new string[] { PARTICIPANT_CHAR, DEPENDENT_CHAR }, StringSplitOptions.RemoveEmptyEntries);
            this.RequestIdType = (RequestIdType)Enum.Parse(typeof(RequestIdType), splitStrings[0]);
            this.Id = Int32.Parse(splitStrings[1]);
        }

        /// <summary>
        /// Gets a flag specifiying whether this request id contains a Participant Id.
        /// </summary>
        public bool IsParticipantId
        {
            get
            {
                return this.requestId.Contains(PARTICIPANT_CHAR);
            }
        }

        /// <summary>
        /// Gets a flag specifying whether this request id contains a Person Depdendent Id.
        /// </summary>
        public bool IsPersonDependentId
        {
            get
            {
                return this.requestId.Contains(DEPENDENT_CHAR);
            }
        }

        /// <summary>
        /// Gets the request id type.
        /// </summary>
        public RequestIdType RequestIdType { get; private set; }

        /// <summary>
        /// Gets the id of either the participant or the person dependent.  The id will be a person dependent
        /// id if the request id type is a dependent.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Returns a string of this id.  Use this string when creating batch records.
        /// </summary>
        /// <returns>A string of this id.</returns>
        public override string ToString()
        {
            return this.requestId;
        }

        /// <summary>
        /// Tests the equality of this instance to the given instance.
        /// </summary>
        /// <param name="obj">The instance to test.</param>
        /// <returns>True, if this instance equals the given instance.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var otherType = obj as RequestId;
            if (otherType == null)
            {
                return false;
            }
            return this.ToString() == otherType.ToString();
        }

        /// <summary>
        /// Returns a hash code of this request id.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
