using ECA.Business.Queries.Models.Persons;
using ECA.Business.Sevis.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using FluentValidation.Results;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using System.Linq;
using System.Diagnostics.Contracts;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Service;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;

namespace ECA.Business.Validation.Sevis
{
    public class ExchangeVisitor
    {


        public ExchangeVisitor(
            User user,
            Bio.Person person,
            FinancialInfo financialInfo,
            string occupationCategoryCode,
            DateTime programEndDate,
            DateTime programStartDate,
            AddressDTO siteOfActivity,
            IEnumerable<Dependent> dependents
            )
        {
            this.User = user;
            this.Person = person;
            this.FinancialInfo = financialInfo;
            this.OccupationCategoryCode = occupationCategoryCode;
            this.ProgramEndDate = programEndDate;
            this.ProgramStartDate = programStartDate;
            this.Dependents = dependents ?? new List<Dependent>();
            this.SiteOfActivity = siteOfActivity;

        }

        /// <summary>
        /// Gets the site of activity, i.e. State Dept at the C Street address.
        /// </summary>
        public AddressDTO SiteOfActivity { get; private set; }

        /// <summary>
        /// Gets the person.
        /// </summary>
        public Bio.Person Person { get; private set; }

        /// <summary>
        /// Gets the financial info.
        /// </summary>
        public FinancialInfo FinancialInfo { get; private set; }

        /// <summary>
        /// Gets the occupation category code.
        /// </summary>
        public string OccupationCategoryCode { get; private set; }

        /// <summary>
        /// Gets the program end date.
        /// </summary>
        public DateTime ProgramEndDate { get; private set; }

        /// <summary>
        /// Gets the program state date.
        /// </summary>
        public DateTime ProgramStartDate { get; private set; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Gets the dependents.
        /// </summary>
        public IEnumerable<Dependent> Dependents { get; private set; }


        public void SetAddSiteOfActivity()
        {
            throw new NotImplementedException();
            //will fall into the items of the sevis class
        }

        /// <summary>
        /// Returns the AddTipp model to send with the exchange visitor.
        /// </summary>
        /// <returns>The AddTIPP instance.</returns>
        public AddTIPP GetAddTipp()
        {
            return new AddTIPP
            {
                Items = new List<object>().ToArray(),
                print7002 = false
            };
        }

        /// <summary>
        /// Returns the SOA (site of activity) instance for this exchange visitor's site of activity.
        /// </summary>
        /// <returns>The SOA i.e. site of activity for this exchange visitor.</returns>
        public SOA GetSOA()
        {
            Contract.Requires(this.SiteOfActivity != null, "The site of activity must not be null.");
            Contract.Requires(this.SiteOfActivity.Street1 != null, "The site of activity street 1 must not be null.");
            Contract.Requires(this.SiteOfActivity.Street2 != null, "The site of activity street 2 must not be null.");
            Contract.Requires(this.SiteOfActivity.City != null, "The site of activity city must not be null.");
            Contract.Requires(this.SiteOfActivity.Division != null, "The site of activity division must not be null.");
            Contract.Requires(this.SiteOfActivity.Country == LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME, "The site of activity country must be the united states.");
            Contract.Requires(this.SiteOfActivity.LocationName != null, "The site of activity location name must be defined.");
            Contract.Requires(this.SiteOfActivity.PostalCode != null, "The site of activity postal code must be defined.");

            var soa = new SOA
            {
                Address1 = this.SiteOfActivity.Street1,
                Address2 = this.SiteOfActivity.Street2,
                City = this.SiteOfActivity.City,
                Explanation = null,
                ExplanationCode = null,
                ExplanationCodeSpecified = false,
                PostalCode = this.SiteOfActivity.PostalCode,
                PrimarySite = true,
                Remarks = null,
                State = this.SiteOfActivity.Division.GetStateCodeType(),
                StateSpecified = true,
                SiteName = this.SiteOfActivity.LocationName
            };
            return soa;
        }

        /// <summary>
        /// Returns the AddSiteOfActivity instance for this exchange visitor.
        /// </summary>
        /// <returns>The AddSiteOfActivity instance.</returns>
        public AddSiteOfActivity GetAddSiteOfActivity()
        {
            var sitesOfActivity = new List<AbstractSiteOfActivity>();
            sitesOfActivity.Add(GetSOA());
            return new AddSiteOfActivity
            {
                SiteOfActivity = sitesOfActivity.ToArray()
            };
        }

        public ValidationResult Validate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Sevis model to create a new exchange visitor.
        /// </summary>
        /// <returns>The sevis model to create a new exchange visitor.</returns>
        public SEVISEVBatchTypeExchangeVisitor GetSEVISBatchTypeExchangeVisitor()
        {
            Contract.Requires(this.Person != null, "The person must not be null.");
            Contract.Requires(this.Person.FullName != null, "The person full name must not be null.");
            Contract.Requires(this.Person.ProgramCategoryCode != null, "The program category code must not be null.");
            Contract.Requires(this.FinancialInfo != null, "The financial info code must not be null.");
            Contract.Requires(this.OccupationCategoryCode != null, "The occupation category code must not be null.");

            var instance = new SEVISEVBatchTypeExchangeVisitor();
            instance.Biographical = this.Person.GetEVPersonTypeBiographical();
            instance.CategoryCode = this.Person.ProgramCategoryCode.GetEVCategoryCodeType();
            instance.FinancialInfo = this.FinancialInfo.GetEVPersonTypeFinancialInfo();
            instance.Items = new List<object> { GetAddTipp(), GetAddSiteOfActivity() }.ToArray();
            if (this.Person.MailAddress != null)
            {
                var address = this.Person.MailAddress.GetUSAddress();
                var addressDoctor = address.GetUSAddressDoctorType();
                instance.MailAddress = addressDoctor;
            }
            if (this.Person.USAddress != null)
            {
                var address = this.Person.USAddress.GetUSAddress();
                var addressDoctor = address.GetUSAddressDoctorType();
                instance.USAddress = addressDoctor;
            }
            instance.ResidentialAddress = null;
            instance.OccupationCategoryCode = this.OccupationCategoryCode.GetEVOccupationCategoryCodeType();
            instance.OccupationCategoryCodeSpecified = true;
            instance.PositionCode = (short)Int32.Parse(this.Person.PositionCode);
            instance.PrgEndDate = this.ProgramEndDate;
            instance.PrgStartDate = this.ProgramStartDate;
            instance.printForm = true;
            instance.requestID = this.Person.ParticipantId.ToString();
            
            SetDependents(instance);
            return instance;
        }

        private void SetDependents(SEVISEVBatchTypeExchangeVisitor instance)
        {
            var dependentsList = this.Dependents ?? new List<AddedDependent>();
            var addedDependents = new List<EVPersonTypeDependent>();
            foreach (var dependent in dependentsList)
            {
                if (dependent.GetType() != typeof(AddedDependent))
                {
                    throw new NotSupportedException("The dependent must be an added dependent.");
                }
                var addedDependent = (AddedDependent)dependent;
                addedDependents.Add(addedDependent.GetEVPersonTypeDependent());
            }
            instance.CreateDependent = addedDependents.ToArray();
        }

        /// <summary>
        /// Returns all update sevis batch objects.  For example, if a participant has been sent to sevis and
        /// a name has changed, a dependent has been added, and another dependent has been updated.  This collection
        /// will contain all update sevis exchange visitor objects to perform those updates.
        /// </summary>
        /// <returns>All update sevis batch objects.</returns>
        public IEnumerable<SEVISEVBatchTypeExchangeVisitor1> GetSEVISEVBatchTypeExchangeVisitor1Collection()
        {
            return Enumerable.Empty<SEVISEVBatchTypeExchangeVisitor1>();
        }

        /// <summary>
        /// Sets the program start date from the given participant person.
        /// </summary>
        /// <param name="participantPerson">The participant person.</param>
        //public void SetStartDate(ParticipantPerson participantPerson)
        //{
        //    this.ProgramStartDate = participantPerson.StartDate.HasValue ? participantPerson.StartDate.Value.UtcDateTime : default(DateTime);
        //}

        /// <summary>
        /// Sets the program end date from the given participant person.
        /// </summary>
        /// <param name="participantPerson">The participant person.</param>
        //public void SetEndDate(ParticipantPerson participantPerson)
        //{
        //    this.ProgramEndDate = participantPerson.EndDate.HasValue ? participantPerson.EndDate.Value.UtcDateTime : default(DateTime);
        //}



        /// <summary>
        /// Sets the person to the person that is created by the given biography.
        /// </summary>
        /// <param name="biography"></param>
        //public void SetPerson(Bio.Person person)
        //{
        //    if (person != null)
        //    {
        //        this.Person = person;
        //    }
        //    else
        //    {
        //        this.Person = null;
        //    }
        //}


        /// <summary>
        /// Sets the subject field with the given field of study.
        /// </summary>
        /// <param name="fieldOfStudy"></param>
        //public void SetSubjectField(FieldOfStudy fieldOfStudy)
        //{
        //    if (fieldOfStudy != null)
        //    {
        //        this.SubjectField = fieldOfStudy.FieldOfStudyCode;
        //    }
        //    else
        //    {
        //        this.SubjectField = null;
        //    }
        //}

        /// <summary>
        /// Sets the us address from the given address dto.
        /// </summary>
        /// <param name="address">The address.</param>
        //public void SetUSAddress(AddressDTO address)
        //{
        //    if (address != null)
        //    {
        //        var usAddress = address.GetUSAddress();
        //        this.USAddress = usAddress;
        //    }
        //    else
        //    {
        //        this.USAddress = null;
        //    }
        //}

        /// <summary>
        /// Sets the mailing address from the given address.
        /// </summary>
        /// <param name="address">The mailing address.</param>
        //public void SetMailAddress(AddressDTO address)
        //{
        //    if (address != null)
        //    {
        //        var usAddress = address.GetUSAddress();
        //        this.MailAddress = usAddress;
        //    }
        //    else
        //    {
        //        this.MailAddress = null;
        //    }
        //} 

        /// <summary>
        /// Sets the dependents of this exchange visitor.
        /// </summary>
        /// <param name="dependents">The exchange visitor's dependents.</param>
        //public void SetDependents(IEnumerable<Dependent> dependents)
        //{
        //    if(dependents != null)
        //    {
        //        this.Dependents = dependents;
        //    }
        //    else
        //    {
        //        this.Dependents = new List<Dependent>();
        //    }
        //}

        /// <summary>
        /// Sets the financial info with the given instance.
        /// </summary>
        /// <param name="financialInfo">The financial info.</param>
        //public void SetFinancialInfo(FinancialInfo financialInfo)
        //{
        //    if (financialInfo != null)
        //    {
        //        this.FinancialInfo = financialInfo;
        //    }
        //    else
        //    {
        //        this.FinancialInfo = null;
        //    }
        //}
    }

}
