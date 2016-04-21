using ECA.Business.Queries.Models.Admin;
using ECA.Business.Sevis.Model;
using System;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// A Dependent instance is used to specify what action will be taken on a sevis registered exchange visitor dependent, such as
    /// adding a new dependenting, deleting a sevis registered dependent, or editing a sevis registered dependent.
    /// </summary>
    public abstract class Dependent : IBiographical, IFormPrintable, IFluentValidatable
    {
        public Dependent(
            FullName fullName,
            string birthCity,
            string birthCountryCode,
            string birthCountryReasonCode,
            DateTime? birthDate,
            string citizenshipCountryCode,
            string emailAddress,
            string gender,
            string permanentResidenceCountryCode,
            string phoneNumber,
            string relationship,
            AddressDTO mailAddress,
            AddressDTO usAddress,
            bool printForm,
            int personId,
            int participantId,
            bool isTravelingWithParticipant,
            bool isDeleted
            )
        {
            this.BirthCity = birthCity;
            this.BirthCountryCode = birthCountryCode;
            this.BirthCountryReasonCode = birthCountryReasonCode;
            this.BirthDate = birthDate;
            this.CitizenshipCountryCode = citizenshipCountryCode;
            this.EmailAddress = emailAddress;
            this.FullName = fullName;
            this.Gender = gender;
            this.PermanentResidenceCountryCode = permanentResidenceCountryCode;
            this.PhoneNumber = phoneNumber;
            this.MailAddress = mailAddress;
            this.USAddress = usAddress;
            this.PrintForm = printForm;
            this.Relationship = relationship;
            this.PersonId = personId;
            this.ParticipantId = participantId;
            this.IsTravelingWithParticipant = isTravelingWithParticipant;
            this.IsDeleted = isDeleted;
        }

        /// <summary>
        /// Gets whether the dependent has been deleted in the KMT system.
        /// </summary>
        public bool IsDeleted { get; private set; }

        /// <summary>
        /// Gets whether or not the dependent will be traveling with the participant.
        /// </summary>
        public bool IsTravelingWithParticipant { get; private set; }

        /// <summary>
        /// Gets the participant id i.e. the id of the participant this person is a dependent of.
        /// </summary>
        public int ParticipantId { get; private set; }

        /// <summary>
        /// Gets the person id of this dependent.
        /// </summary>
        public int PersonId { get; private set; }

        /// <summary>
        /// Gets or sets the print form flag.
        /// </summary>
        public bool PrintForm { get; set; }        
        
        /// <summary>
        /// Gets the full name.
        /// </summary>
        public FullName FullName { get; private set; }

        /// <summary>
        /// Gets the birth date.
        /// </summary>
        public DateTime? BirthDate { get; private set; }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        public string Gender { get; private set; }

        /// <summary>
        /// Gets the birth city.
        /// </summary>
        public string BirthCity { get; private set; }

        /// <summary>
        /// Gets the birth country code.
        /// </summary>
        public string BirthCountryCode { get; private set; }

        /// <summary>
        /// Gets the citizneship country code.
        /// </summary>
        public string CitizenshipCountryCode { get; private set; }

        /// <summary>
        /// Gets the permamanent residence country code.
        /// </summary>
        public string PermanentResidenceCountryCode { get; private set; }

        /// <summary>
        /// Gets the birth country reason.
        /// </summary>
        public string BirthCountryReasonCode { get; private set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        public string EmailAddress { get; private set; }

        /// <summary>
        /// Gets the phone number.
        /// </summary>
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// Gets the mailing address.
        /// </summary>
        public AddressDTO MailAddress { get; private set; }

        /// <summary>
        /// Gets the us address.
        /// </summary>
        public AddressDTO USAddress { get; private set; }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Returns true, if the relationship has a value and its the dependent code time 01.
        /// </summary>
        /// <returns>True, if the relationship has a value and its the dependent code time 01.</returns>
        public bool IsSpousalDependent()
        {
            return !String.IsNullOrWhiteSpace(this.Relationship) && this.Relationship.GetDependentCodeType() == DependentCodeType.Item01;
        }

        /// <summary>
        /// Returns true, if the relationship has a value and its the dependent code time 02.
        /// </summary>
        /// <returns>True, if the relationship has a value and its the dependent code time 02.</returns>
        public bool IsChildDependent()
        {
            return !String.IsNullOrWhiteSpace(this.Relationship) && this.Relationship.GetDependentCodeType() == DependentCodeType.Item02;
        }

        /// <summary>
        /// Returns the age of this dependent, or -1 if the birthdate is null.
        /// </summary>
        /// <returns>The age of the dependent, or -1 if the birthdate is null.</returns>
        public int GetAge()
        {
            if (this.BirthDate.HasValue)
            {
                var dob = this.BirthDate.Value;
                var utcNow = DateTime.UtcNow;
                var birthDate = new DateTime(dob.Year, dob.Month, dob.Day, 0, 0, 0, DateTimeKind.Utc);
                var today = utcNow.Date;

                var age = today.Year - birthDate.Year;
                if (today.DayOfYear < birthDate.DayOfYear)
                {
                    age--;
                }
                return age;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Returns a SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance used when performing
        /// a sevis registered exchange visitor's dependent details.
        /// </summary>
        /// <returns>A SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance.</returns>
        public abstract object GetSevisExhangeVisitorDependentInstance();

        /// <summary>
        /// Returns true, if the dependent should be validated, otherwise false.
        /// </summary>
        /// <returns>True, if this dependent should be validated, otherwise false.</returns>
        public bool ShouldValidate()
        {
            return !IgnoreDependentValidation();
        }

        /// <summary>
        /// Returns true, if this dependent should be ignored in validation.
        /// </summary>
        /// <returns>True, if this dependent should be ignored in validation.</returns>
        public abstract bool IgnoreDependentValidation();

        /// <summary>
        /// Returns the request id of this dependent.
        /// </summary>
        /// <returns>The request id.</returns>
        public abstract RequestId GetRequestId();
    }
}
