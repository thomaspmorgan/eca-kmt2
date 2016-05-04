using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.Finance;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// The ExchangeVisitor model is used to send created and updated exchange visitor information to sevis.  This class can be used to validate exchange visitor information
    /// as well as convert directly to the sevis xsd schema exchange visitor objects.
    /// </summary>
    public class ExchangeVisitor : ISevisIdentifable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="user">The user requesting the exchange visitor.</param>
        /// <param name="sevisId">The sevis id of the exchange visitor, or null, if none has been provided yet.</param>
        /// <param name="person">The person object representing the biographical information of the exchange visitor.</param>
        /// <param name="financialInfo">The financial info, detailing financial information about the exchange visitor.</param>
        /// <param name="occupationCategoryCode">The occupation category code.</param>
        /// <param name="programEndDate">The end date of the participant.</param>
        /// <param name="programStartDate">The start date of the participant.</param>
        /// <param name="siteOfActivity">The exchange visitor site of activity.</param>
        /// <param name="dependents">The dependents of the exchange visitor.</param>
        public ExchangeVisitor(
            string sevisId,
            Bio.Person person,
            FinancialInfo financialInfo,
            string occupationCategoryCode,
            DateTime programEndDate,
            DateTime programStartDate,
            AddressDTO siteOfActivity,
            IEnumerable<Dependent> dependents
            )
        {
            this.Person = person;
            this.FinancialInfo = financialInfo;
            this.OccupationCategoryCode = occupationCategoryCode;
            this.ProgramEndDate = programEndDate;
            this.ProgramStartDate = programStartDate;
            this.Dependents = dependents ?? new List<Dependent>();
            this.SiteOfActivity = siteOfActivity;
            this.SevisId = sevisId;
        }

        /// <summary>
        /// Gets or sets the sevis id.
        /// </summary>
        public string SevisId { get; private set; }

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
        /// Gets the dependents.
        /// </summary>
        public IEnumerable<Dependent> Dependents { get; private set; }

        /// <summary>
        /// Serializes this exchange visitor to json.
        /// </summary>
        /// <returns>The json object representing this exchange visitor.</returns>
        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(this, GetSerializerSettings());
            return json;
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
                State = this.SiteOfActivity.DivisionIso.GetStateCodeType(),
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

        /// <summary>
        /// Performs validation on this exchange visitor and returns the validation result.
        /// </summary>
        /// <returns>The validation result.</returns>
        public ValidationResult Validate(AbstractValidator<ExchangeVisitor> validator)
        {
            Contract.Requires(validator != null, "The validator must not be null.");
            return validator.Validate(this);
        }

        /// <summary>
        /// Returns the Sevis model to create a new exchange visitor.
        /// </summary>
        /// <param name="sevisUsername">The sevis username the exchange visitor is being sent under.</param>
        /// <returns>The sevis model to create a new exchange visitor.</returns>
        public SEVISEVBatchTypeExchangeVisitor GetSEVISBatchTypeExchangeVisitor(string sevisUsername)
        {
            Contract.Requires(this.Person != null, "The person must not be null.");
            Contract.Requires(this.Person.FullName != null, "The person full name must not be null.");
            Contract.Requires(this.Person.ProgramCategoryCode != null, "The program category code must not be null.");
            Contract.Requires(this.FinancialInfo != null, "The financial info code must not be null.");
            Contract.Requires(this.OccupationCategoryCode != null, "The occupation category code must not be null.");
            Contract.Requires(sevisUsername != null, "The sevis username must not be null.");

            var instance = new SEVISEVBatchTypeExchangeVisitor();
            instance.userID = sevisUsername;
            instance.Biographical = this.Person.GetEVPersonTypeBiographical();
            instance.CategoryCode = this.Person.ProgramCategoryCode.GetEVCategoryCodeType();
            instance.FinancialInfo = this.FinancialInfo.GetEVPersonTypeFinancialInfo();
            instance.Items = new List<object> { GetAddSiteOfActivity() }.ToArray();
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

            if (this.OccupationCategoryCode != null)
            {
                instance.OccupationCategoryCode = this.OccupationCategoryCode.GetEVOccupationCategoryCodeType();
                instance.OccupationCategoryCodeSpecified = true;
            }
            else
            {
                instance.OccupationCategoryCodeSpecified = false;
            }
            instance.ResidentialAddress = null;
            instance.PositionCode = (short)Int32.Parse(this.Person.PositionCode);
            instance.PrgEndDate = this.ProgramEndDate;
            instance.PrgStartDate = this.ProgramStartDate;
            instance.printForm = true;
            instance.SubjectField = this.Person.SubjectField.GetEVPersonTypeSubjectField();
            instance.SetRequestId(this.Person.ParticipantId);
            SetDependents(instance);

            var key = new ParticipantSevisKey(this.Person);
            key.SetUserDefinedFields(instance);
            return instance;
        }

        private void SetDependents(SEVISEVBatchTypeExchangeVisitor instance)
        {
            var dependentsList = this.Dependents ?? new List<AddedDependent>();
            var addedDependents = new List<EVPersonTypeDependent>();
            foreach (var dependent in dependentsList.Where(x => !x.IsDeleted).ToList())
            {
                if (dependent.GetType() != typeof(AddedDependent))
                {
                    throw new NotSupportedException("The dependent must be an added dependent.");
                }
                var addedDependent = (AddedDependent)dependent;
                addedDependents.Add(addedDependent.GetEVPersonTypeDependent());
            }
            if (addedDependents.Count > 0)
            {
                instance.CreateDependent = addedDependents.ToArray();
            }
            else
            {
                instance.CreateDependent = null;
            }
        }

        /// <summary>
        /// Returns all update sevis batch objects.  For example, if a participant has been sent to sevis and
        /// a name has changed, a dependent has been added, and another dependent has been updated.  This collection
        /// will contain all update sevis exchange visitor objects to perform those updates.
        /// </summary>
        /// <param name="sevisUsername">The sevis username the exchange visitor is being sent with.</param>
        /// <returns>All update sevis batch objects.</returns>
        public IEnumerable<SEVISEVBatchTypeExchangeVisitor1> GetSEVISEVBatchTypeExchangeVisitor1Collection(string sevisUsername)
        {
            Contract.Requires(sevisUsername != null, "The sevis username must not be null.");
            var visitors = new List<SEVISEVBatchTypeExchangeVisitor1>();
            Func<object, RequestId, SEVISEVBatchTypeExchangeVisitor1> createUpdateExchangeVisitor = (item, requestId) =>
            {
                return new SEVISEVBatchTypeExchangeVisitor1
                {
                    Item = item,
                    requestID = requestId.ToString(),
                    sevisID = this.SevisId,
                    statusCodeSpecified = false,
                    userID = sevisUsername
                };
            };
            visitors.Add(createUpdateExchangeVisitor(this.Person.GetSEVISEVBatchTypeExchangeVisitorBiographical(), new RequestId(this.Person.ParticipantId, RequestIdType.Participant, RequestActionType.Update)));
            visitors.Add(createUpdateExchangeVisitor(this.FinancialInfo.GetSEVISEVBatchTypeExchangeVisitorFinancialInfo(), new RequestId(this.Person.ParticipantId, RequestIdType.FinancialInfo, RequestActionType.Update)));
            foreach (var dependent in this.Dependents)
            {
                var modifiedDependent = new ModifiedParticipantDependent(dependent);
                visitors.Add(createUpdateExchangeVisitor(modifiedDependent.GetSEVISEVBatchTypeExchangeVisitorDependent(), dependent.GetRequestId()));
            }

            var exchangeVisitorProgram = new SEVISEVBatchTypeExchangeVisitorProgram();
            exchangeVisitorProgram.Item = this.Person.SubjectField.GetSEVISEVBatchTypeExchangeVisitorProgramEditSubject();
            visitors.Add(createUpdateExchangeVisitor(exchangeVisitorProgram, new RequestId(this.Person.ParticipantId, RequestIdType.SubjectField, RequestActionType.Update)));

            return visitors;
        }

        /// <summary>
        /// Returns the exchange visitor validation message to send to sevis.
        /// </summary>
        /// <returns>The exchange visitor validation message to send to sevis.</returns>
        public SEVISEVBatchTypeExchangeVisitor1 GetSEVISEVBatchTypeExchangeVisitorValidate(string sevisUsername)
        {
            var item = new SEVISEVBatchTypeExchangeVisitorValidate
            {
                EmailAddress = this.Person.EmailAddress,
            };
            if (this.Person.PhoneNumber != null)
            {
                item.PhoneNumber = this.Person.GetUSPhoneNumber(this.Person.PhoneNumber);
            }
            if (this.Person.MailAddress != null)
            {
                var address = this.Person.MailAddress.GetUSAddress();
                var addressDoctor = address.GetUSAddressDoctorType();
                item.USAddress = addressDoctor;
            }
            return new SEVISEVBatchTypeExchangeVisitor1
            {
                Item = item,
                sevisID = this.SevisId,
                requestID = new RequestId(this.Person.ParticipantId, RequestIdType.Validate, RequestActionType.Update).ToString(),
                statusCodeSpecified = false,
                userID = sevisUsername
            };
        }

        /// <summary>
        /// Returns an exchange visitor instance with the given json.
        /// </summary>
        /// <param name="json">The json object representing an exchange visitor.</param>
        /// <returns>The ExchangeVisitor instance.</returns>
        public static ExchangeVisitor GetExchangeVisitor(string json)
        {
            Contract.Requires(json != null, "The json string must not be null.");
            return JsonConvert.DeserializeObject<ExchangeVisitor>(json, GetSerializerSettings());
        }

        /// <summary>
        /// Returns the serializer settings for the json serializer.
        /// </summary>
        /// <returns>The settings for the json serializer.</returns>
        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
        }
    }
}
