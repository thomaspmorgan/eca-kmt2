using ECA.Business.Queries.Models.Admin;
using ECA.Business.Sevis.Model;
using Newtonsoft.Json;
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
    public abstract class Dependent : IBiographical, IFormPrintable
    {
        public Dependent(
            FullName fullName,
            string birthCity,
            string birthCountryCode,
            string birthCountryReason,
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
            this.Gender = gender;
            this.PermanentResidenceCountryCode = permanentResidenceCountryCode;
            this.PhoneNumber = phoneNumber;
            this.MailAddress = mailAddress;
            this.USAddress = usAddress;
            this.PrintForm = printForm;
            this.Relationship = relationship;
            this.PersonId = personId;
            this.ParticipantId = participantId;
        }

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
        /// Gets or sets the relationship.
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Returns a SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance used when performing
        /// a sevis registered exchange visitor's dependent details.
        /// </summary>
        /// <returns>A SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance.</returns>
        public abstract object GetSevisExhangeVisitorDependentInstance();
    }
}
