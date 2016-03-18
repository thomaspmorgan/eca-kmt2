using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.Bio;
using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// A ParticipantSevisKey is used to normalize the setting and retrieval of
    /// a PersonId and ParticipantId from either an exchange visitor person or an exchange visitor
    /// dependent.  Use the class methods to set user defined fields on sevis model instances
    /// and use the user defined field constructor to recover the key on a sevis api response.
    /// </summary>
    public class ParticipantSevisKey
    {
        private const string USER_DEFINED_B_FIELD_PREFIX = "B";

        private static Regex PARTICIPANT_ID_REGEX = new Regex(@"^[0-9]{1,10}$");

        private static Regex PERSON_ID_REGEX = new Regex(@"^[B]{1}[0-9]{1,13}$");

        /// <summary>
        /// Creates a new ParticipantSevisKey with the given dependent and intializes the 
        /// ParticipantId and the PersonId.
        /// </summary>
        /// <param name="dependent"></param>
        public ParticipantSevisKey(Dependent dependent)
            : this(dependent.ParticipantId, dependent.PersonId)
        {
            Contract.Requires(dependent != null, "The dependent must not be null.");
        }

        /// <summary>
        /// Creates a new ParticipantSevisKey with the given person and initializes
        /// the ParticpantId and the Person Id.
        /// </summary>
        /// <param name="person">The person to intialize this key instance with.</param>
        public ParticipantSevisKey(Person person)
            : this(person.ParticipantId, person.PersonId)
        {
            Contract.Requires(person != null, "The person must not be null.");
        }

        /// <summary>
        /// Creates a new participant sevis key instance with the given user defined fields.  The fields
        /// should have been previously set by another instance of a ParticipantSevisKey as they are
        /// specially formatted.
        /// </summary>
        /// <param name="userDefinedAField">The formatted user defined field a.</param>
        /// <param name="userDefinedBField">The formatted user defined field b.</param>
        public ParticipantSevisKey(string userDefinedAField, string userDefinedBField)
        {
            Contract.Requires(userDefinedAField != null, "The user defined a field must be defined.");
            Contract.Requires(userDefinedBField != null, "The user defined b field must be defined.");
            if (!PARTICIPANT_ID_REGEX.IsMatch(userDefinedAField))
            {
                throw new NotSupportedException("The user defined a field which represents the participant id is not valid.");
            }
            if (!PERSON_ID_REGEX.IsMatch(userDefinedBField))
            {
                throw new NotSupportedException("The user defined b field which represents the person id is not valid.");
            }
            this.ParticipantId = Int32.Parse(userDefinedAField);
            this.PersonId = Int32.Parse(userDefinedBField.Substring(1));
        }

        private ParticipantSevisKey(int participantId, int personId)
        {
            this.ParticipantId = participantId;
            this.PersonId = personId;
        }

        /// <summary>
        /// Gets the participant id.
        /// </summary>
        public int ParticipantId { get; private set; }

        /// <summary>
        /// Gets the person id.
        /// </summary>
        public int PersonId { get; private set; }

        /// <summary>
        /// Sets the user defined fields on the given sevis dependent model.
        /// </summary>
        /// <param name="dependent">The dependent to set the user defined fields on.</param>
        public void SetUserDefinedFields(EVPersonTypeDependent dependent)
        {
            Contract.Requires(dependent != null, "The dependent must not be null.");
            dependent.UserDefinedA = GetParticipantIdAsString(this.ParticipantId);
            dependent.UserDefinedB = GetPersonIdAsString(this.PersonId);
        }

        /// <summary>
        /// Sets the user defined fields on the given sevis person model.
        /// </summary>
        /// <param name="person">The person to the set the user defined fields on.</param>
        public void SetUserDefinedFields(EVPersonType person)
        {
            Contract.Requires(person != null, "The person must not be null.");
            person.UserDefinedA = GetParticipantIdAsString(this.ParticipantId);
            person.UserDefinedB = GetPersonIdAsString(this.PersonId);
        }

        private string GetParticipantIdAsString(int participantId)
        {
            return participantId.ToString();
        }

        private string GetPersonIdAsString(int personId)
        {
            return String.Format("{0}{1}", USER_DEFINED_B_FIELD_PREFIX, personId);
        }
    }
}
