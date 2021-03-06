﻿
using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;
using ECA.Business.Queries.Models.Admin;
using PhoneNumbers;
using KellermanSoftware.CompareNetObjects;
using System.Collections.Generic;

namespace ECA.Business.Validation.Sevis.Bio
{
    [Validator(typeof(PersonValidator))]
    public class Person : IBiographical, IFormPrintable, IRemarkable, IFluentValidatable, IChangeComparable<Person, PersonChangeDetail>
    {
        /// <summary>
        /// Creates a new person instance.
        /// </summary>
        /// <param name="fullName">The full name of the person.</param>
        /// <param name="birthCity">The birth city.</param>
        /// <param name="birthCountryCode">The birth country code.</param>
        /// <param name="birthDate">The birth date.</param>
        /// <param name="citizenshipCountryCode">The citizenship country code.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="permanentResidenceCountryCode">The permananent residence country code.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="positionCode">The position code.</param>
        /// <param name="programCategoryCode">The program category code.</param>
        /// <param name="subjectField">The subject field.</param>
        /// <param name="mailAddress">The mailing address.</param>
        /// <param name="usAddress">The us address.</param>
        /// <param name="printForm">The print form flag.</param>
        /// <param name="personId">The person id.</param>
        /// <param name="participantId">The participant id.</param>
        public Person(
            FullName fullName,
            string birthCity,
            string birthCountryCode,
            DateTime? birthDate,
            string citizenshipCountryCode,
            string emailAddress,
            string gender,
            string permanentResidenceCountryCode,
            string phoneNumber,
            string remarks,
            string positionCode,
            string programCategoryCode,
            SubjectField subjectField,
            AddressDTO mailAddress,
            AddressDTO usAddress,
            bool printForm,
            int personId,
            int participantId
            )
        {
            this.BirthCity = birthCity;
            this.BirthCountryCode = birthCountryCode;
            this.BirthDate = birthDate;
            this.CitizenshipCountryCode = citizenshipCountryCode;
            this.EmailAddress = emailAddress;
            this.FullName = fullName;
            this.Gender = gender;
            this.PermanentResidenceCountryCode = permanentResidenceCountryCode;
            this.PhoneNumber = phoneNumber;
            this.Remarks = remarks;
            this.MailAddress = mailAddress;
            this.USAddress = usAddress;
            this.PrintForm = printForm;
            this.PositionCode = positionCode;
            this.PersonId = personId;
            this.ParticipantId = participantId;
            this.ProgramCategoryCode = programCategoryCode;
            this.SubjectField = subjectField;
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
        /// Gets or sets the print form flag.
        /// </summary>
        public bool PrintForm { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets the position code.
        /// </summary>
        public string PositionCode { get; private set; }

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
        /// Gets the field of study code.
        /// </summary>
        public SubjectField SubjectField { get; private set; }

        /// <summary>
        /// Gets the citizneship country code.
        /// </summary>
        public string CitizenshipCountryCode { get; private set; }

        /// <summary>
        /// Gets the permamanent residence country code.
        /// </summary>
        public string PermanentResidenceCountryCode { get; private set; }

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
        /// Gets the program category code.
        /// </summary>
        public string ProgramCategoryCode { get; private set; }

        /// <summary>
        /// Returns a US Phone number with the given number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to parse into a US phone number.</param>
        /// <returns>The US phone number.</returns>
        public string GetUSPhoneNumber(string phoneNumber)
        {
            Contract.Requires(phoneNumber != null, "The phone number must not be null.");
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var pn = phoneNumberUtil.Parse(phoneNumber, ECA.Data.PhoneNumber.US_PHONE_NUMBER_REGION_KEY);
            return pn.NationalNumber.ToString();
        }

        /// <summary>
        /// Returns an EVPersonTypeBiographical instance for use in a new exchange visitor's biographical information.
        /// </summary>
        /// <returns>Returns an EVPersonTypeBiographical instance for use in a new exchange visitor's biographical information.</returns>
        public EVPersonTypeBiographical GetEVPersonTypeBiographical()
        {
            Contract.Requires(this.BirthDate.HasValue, "The birth date must have a value.");
            Contract.Requires(this.FullName != null, "The full name should be specified.");
            Contract.Requires(this.BirthCountryCode != null, "The BirthCountryCode should be specified.");
            Contract.Requires(this.CitizenshipCountryCode != null, "The CitizenshipCountryCode should be specified.");
            Contract.Requires(this.PermanentResidenceCountryCode != null, "The PermanentResidenceCountryCode should be specified.");
            Contract.Requires(this.Gender != null, "The Gender should be specified.");
            var instance = new EVPersonTypeBiographical
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                BirthDate = this.BirthDate.Value,
                CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetNameType(),
                Gender = this.Gender.GetEVGenderCodeType(),
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
            };
            if (this.PhoneNumber != null)
            {
                instance.PhoneNumber = GetUSPhoneNumber(this.PhoneNumber);
            }
            return instance;
        }

        /// <summary>
        /// Returns a registered sevis exchange visitor's updated biographical information model to be used
        /// to update an exchange visitor's biography.
        /// </summary>
        /// <returns>A registered sevis exchange visitor's updated biographical information model to be used
        /// to update an exchange visitor's biography.</returns>
        public SEVISEVBatchTypeExchangeVisitorBiographical GetSEVISEVBatchTypeExchangeVisitorBiographical()
        {
            Contract.Requires(this.BirthDate.HasValue, "The birth date must have a value.");
            Contract.Requires(this.FullName != null, "The full name should be specified.");
            Contract.Requires(this.BirthCountryCode != null, "The BirthCountryCode should be specified.");
            Contract.Requires(this.CitizenshipCountryCode != null, "The CitizenshipCountryCode should be specified.");
            Contract.Requires(this.PermanentResidenceCountryCode != null, "The PermanentResidenceCountryCode should be specified.");
            Contract.Requires(this.Gender != null, "The Gender should be specified.");
            Contract.Requires(this.USAddress != null, "The us address should be specified.");
            Contract.Requires(this.PositionCode != null, "The position code must be specified.");
            Func<string, bool> isCodeSpecified = (value) =>
            {
                return !string.IsNullOrWhiteSpace(value);
            };

            var instance = new SEVISEVBatchTypeExchangeVisitorBiographical
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode != null ? this.BirthCountryCode.GetBirthCntryCodeType() : default(BirthCntryCodeType),
                BirthDate = this.BirthDate.HasValue ? this.BirthDate.Value : default(DateTime),
                CitizenshipCountryCode = this.CitizenshipCountryCode != null ? this.CitizenshipCountryCode.GetCountryCodeWithType() : default(CntryCodeWithoutType),
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetNameNullableType(),
                Gender = this.Gender != null ? this.Gender.GetGenderCodeType() : default(GenderCodeType),
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode != null ? this.PermanentResidenceCountryCode.GetCountryCodeWithType() : default(CntryCodeWithoutType),
                printForm = this.PrintForm,
                Remarks = this.Remarks,
                BirthCountryCodeSpecified = isCodeSpecified(this.BirthCountryCode),
                BirthDateSpecified = this.BirthDate.HasValue,
                CitizenshipCountryCodeSpecified = isCodeSpecified(this.CitizenshipCountryCode),
                GenderSpecified = isCodeSpecified(this.Gender),
                PermanentResidenceCountryCodeSpecified = isCodeSpecified(this.PermanentResidenceCountryCode),
                ResidentialAddress = null,
                USAddress = null,
                MailAddress = null
            };
            if (this.PhoneNumber != null)
            {
                instance.PhoneNumber = GetUSPhoneNumber(this.PhoneNumber);
            }
            if (this.PositionCode != null)
            {
                instance.PositionCode = (short)Int32.Parse(this.PositionCode);
                instance.PositionCodeSpecified = true;
            }
            else
            {
                instance.PositionCode = default(short);
                instance.PositionCodeSpecified = false;
            }
            if (this.USAddress != null)
            {
                var address = this.USAddress.GetUSAddress();
                var usAddress = address.GetUSAddressDoctorType();
                instance.USAddress = usAddress;
            }
            if (this.MailAddress != null)
            {
                var address = this.MailAddress.GetUSAddress();
                var usAddress = address.GetUSAddressDoctorType();
                instance.MailAddress = usAddress;
            }
            return instance;
        }

        /// <summary>
        /// Returns true, if this person should be validated, otherwise false.
        /// </summary>
        /// <returns>True, if this person should be validated, otherwise false.</returns>
        public bool ShouldValidate()
        {
            return true;
        }

        /// <summary>
        /// Returns a PersonChangeDetail for this person.
        /// </summary>
        /// <param name="otherChangeComparable">The person to compare.</param>
        /// <returns>The change detail.</returns>
        public PersonChangeDetail GetChangeDetail(Person otherChangeComparable)
        {
            var compareConfig = new ComparisonConfig
            {
                CompareChildren = false,
                MembersToIgnore = new List<string> { nameof(this.PrintForm) }
            };
            var compareLogic = new CompareLogic(compareConfig);
            var result = compareLogic.Compare(this, otherChangeComparable);
            return new PersonChangeDetail(result);
        }
    }
}