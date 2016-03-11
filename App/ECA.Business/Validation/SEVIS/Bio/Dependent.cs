using ECA.Business.Queries.Models.Admin;
using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// A Dependent instance is used to specify what action will be taken on a sevis registered exchange visitor dependent, such as
    /// adding a new dependenting, deleting a sevis registered dependent, or editing a sevis registered dependent.
    /// </summary>
    public abstract class Dependent : IBiographical, IFormPrintable, IUserDefinable
    {

        public Dependent(
            FullName fullName,
            string birthCity,
            string birthCountryCode,
            string birthCountryReason,
            DateTime? birthDate,
            string citizenshipCountryCode,
            string emailAddress,
            string genderCode,
            string permanentResidenceCountryCode,
            string phoneNumber,
            string positionCode,
            string relationship,
            AddressDTO mailAddress,
            AddressDTO usAddress,
            bool printForm,
            int personId,
            int participantId
            )
        {
            this.BirthCity = birthCity;
            this.BirthCountryCode = birthCountryCode;
            this.BirthCountryReason = birthCountryReason;
            this.BirthDate = birthDate;
            this.CitizenshipCountryCode = citizenshipCountryCode;
            this.EmailAddress = emailAddress;
            this.FullName = fullName;
            this.Gender = genderCode;
            this.PermanentResidenceCountryCode = permanentResidenceCountryCode;
            this.PhoneNumber = phoneNumber;
            this.MailAddress = mailAddress;
            this.USAddress = usAddress;
            this.PrintForm = printForm;
            this.PositionCode = positionCode;
            this.Relationship = relationship;
            SetParticipantId(participantId);
            SetPersonId(personId);
        }

        /// <summary>
        /// Gets or sets the print form flag.
        /// </summary>
        public bool PrintForm { get; set; }
        

        /// <summary>
        /// Gets or sets the position code.
        /// </summary>
        public string PositionCode { get; set; }

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
        public string BirthCountryReason { get; private set; }

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
        /// Gets or sets the user defined a field.
        /// </summary>
        public string UserDefinedA { get; private set; }

        /// <summary>
        /// Gets or sets the user defined b field.
        /// </summary>
        public string UserDefinedB { get; private set; }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Sets the UserDefinedA field to the participant ic.
        /// </summary>
        /// <param name="participantId">The participant id.</param>
        public void SetParticipantId(int participantId)
        {
            this.UserDefinedA = participantId.ToString();
        }

        /// <summary>
        /// Sets the UserDefinedB field to the person id.
        /// </summary>
        /// <param name="personId">The person id.</param>
        public void SetPersonId(int personId)
        {
            this.UserDefinedB = personId.ToString();
        }

        /// <summary>
        /// Returns the person id of this dependent.
        /// </summary>
        /// <returns>The person id of this dependent.</returns>
        public int? GetPersonId()
        {
            return Int32.Parse(this.UserDefinedB);
        }

        /// <summary>
        /// Returns the participant id i.e. the participant id of the person that is participating in the project and who this person
        /// is a dependent of.
        /// </summary>
        /// <returns>The participant id.</returns>
        public int? GetParticipantId()
        {
            return Int32.Parse(this.UserDefinedA);
        }


        /// <summary>
        /// Returns a SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance used when performing
        /// a sevis registered exchange visitor's dependent details.
        /// </summary>
        /// <returns>A SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance.</returns>
        public abstract object GetSevisExhangeVisitorDependentInstance();
    }
}
