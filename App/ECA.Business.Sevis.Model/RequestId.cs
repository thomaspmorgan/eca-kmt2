using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ECA.Business.Sevis.Model
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
        SubjectField,
        
        /// <summary>
        /// The validate request type.  This is used when validating a participant in the sevis batch api.
        /// </summary>
        Validate
    }

    public enum RequestActionType
    {
        /// <summary>
        /// The request is to create a sevis record.
        /// </summary>
        Create = 1,

        /// <summary>
        /// The request is to update a sevis record.
        /// </summary>
        Update
    }

    /// <summary>
    /// A RequestId is used to encode information about the individual requests made in a sevis batch, including the participant id or person dependent id,
    /// and the request type.  According to the api documentation a request id
    /// may be up to 20 characters long.  An Int32 max number is 10 characters long, leaving 10 characters for additional information. 
    /// 
    /// The string format will be [RequestIdType]-[RequestActionTypeId]-[ParticipantId|DependentId] where request type id is the enum integer value, [RequestEntityTypeId] 
    /// the request Id will be up to 16 characters long.
    /// </summary>
    public class RequestId
    {
        private static Regex REQUEST_ID_REGEX = new Regex(@"^\d+[-]\d+[-]\d+$");

        private const string SPLIT_CHAR = "-";

        private const string REQUEST_ID_FORMAT_STRING = "{0}-{1}-{2}";

        /// <summary>
        /// Creates a new request id with the id of the object and request id and request action types.
        /// </summary>
        /// <param name="id">The id of the participant or dependent.</param>
        /// <param name="requestIdType">The request id type.</param>
        /// <param name="actionType">The request action type.</param>
        public RequestId(int id, RequestIdType requestIdType, RequestActionType actionType)
        {
            this.Id = id;
            this.RequestActionType = actionType;
            this.RequestIdType = requestIdType;
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
        }

        private void Parse(string requestId)
        {
            Contract.Requires(requestId != null, "The requestId must not be null.");
            var splitStrings = requestId.Split(new string[] { SPLIT_CHAR }, StringSplitOptions.RemoveEmptyEntries);
            Contract.Assert(splitStrings.Length == 3, "The array should have 3 items in it.");
            this.RequestIdType = (RequestIdType)Enum.Parse(typeof(RequestIdType), splitStrings[0]);
            this.RequestActionType = (RequestActionType)Enum.Parse(typeof(RequestActionType), splitStrings[1]);
            this.Id = Int32.Parse(splitStrings[2]);
        }

        /// <summary>
        /// Gets a flag specifiying whether this request id contains a Participant Id.
        /// </summary>
        public bool IsParticipantId
        {
            get
            {
                return !this.IsPersonDependentId;
            }
        }

        /// <summary>
        /// Gets a flag specifying whether this request id contains a Person Depdendent Id.
        /// </summary>
        public bool IsPersonDependentId
        {
            get
            {
                return this.RequestIdType == RequestIdType.Dependent;
            }
        }

        /// <summary>
        /// Gets the request id type.
        /// </summary>
        public RequestIdType RequestIdType { get; private set; }

        /// <summary>
        /// Gets the request action type.
        /// </summary>
        public RequestActionType RequestActionType { get; private set; }

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
            return String.Format(REQUEST_ID_FORMAT_STRING, (int)this.RequestIdType, (int)this.RequestActionType, this.Id);
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
