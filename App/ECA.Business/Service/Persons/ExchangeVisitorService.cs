using ECA.Business.Queries.Persons;
using System.Data.Entity;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Validation.Model.Shared;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Admin;

namespace ECA.Business.Service.Persons
{
    public class ExchangeVisitorService : EcaService, IExchangeVisitorService
    {
        /// <summary>
        /// The default exchange visitor occupation category code.
        /// </summary>
        public const string EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE = "99";

        /// <summary>
        /// The max length of the subject field remarks.
        /// </summary>
        public const int SUBJECT_FIELD_REMARKS_MAX_LENGTH = 500;

        /// <summary>
        /// The address 1 of the state dept site of activity.
        /// </summary>
        public const string SITE_OF_ACTIVITY_STATE_DEPT_ADDRESS_1 = "2200 C Street, NW";

        /// <summary>
        /// The city of the state dept site of activity.
        /// </summary>
        public const string SITE_OF_ACTIVITY_STATE_DEPT_CITY = "Washington";

        /// <summary>
        /// The state of the state dept site of activity.
        /// </summary>
        public const string SITE_OF_ACTIVITY_STATE_DEPT_STATE = "DC";

        /// <summary>
        /// The postal code of the state dept site of activity.
        /// </summary>
        public const string SITE_OF_ACTIVITY_STATE_DEPT_POSTAL_CODE = "20522";

        /// <summary>
        /// The reprint form update reason code.
        /// </summary>
        public const string REPRINT_FORM_UPDATE_REASON_CODE = "05";

        /// <summary>
        /// The name of the state dept site of activity.
        /// </summary>
        public const string SITE_OF_ACTIVITY_STATE_DEPT_NAME = "US Department of State";

        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private readonly Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;
        private readonly Action<Participant> throwIfParticipantIsNotAPerson;
        private readonly Action<Participant, int> throwIfMoreThanOneCountryOfCitizenship;
        private readonly Action<Person, int> throwIfPersonDoesNotHavePlaceOfBirth;
        private readonly Action<int, int> throwIfLocationIsNotACity;

        public ExchangeVisitorService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfModelDoesNotExist = (id, instance, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model of type [{0}] with id [{1}] was not found.", type.Name, id));
                }
            };
            throwSecurityViolationIfParticipantDoesNotBelongToProject = (userId, projectId, participant) =>
            {
                if (participant != null && participant.ProjectId != projectId)
                {
                    throw new BusinessSecurityException(
                        String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        userId,
                        participant.ParticipantId,
                        projectId));
                }
            };
            throwIfParticipantIsNotAPerson = (participant) =>
            {
                if (!participant.PersonId.HasValue)
                {
                    throw new NotSupportedException(String.Format("The participant with id [0] is not a person participant.", participant.ParticipantId));
                }
            };
            throwIfMoreThanOneCountryOfCitizenship = (participant, numberOfCitizenships) =>
            {
                if (numberOfCitizenships > 1)
                {
                    throw new NotSupportedException(String.Format("The participant with id [0] has more than one country of citizenship.", participant.ParticipantId));
                }
            };
            throwIfPersonDoesNotHavePlaceOfBirth = (person, participantId) =>
            {
                if (!person.PlaceOfBirthId.HasValue)
                {
                    throw new NotSupportedException(String.Format("The participant with id [{0}] does not have a place of birth.", participantId));
                }
            };
            throwIfLocationIsNotACity = (locationTypeId, participantId) =>
            {
                if(locationTypeId != LocationType.City.Id)
                {
                    throw new NotSupportedException(String.Format("The participant with id [{0}] does not have a place of birth that is a city.", participantId));
                }
            };
        }

        #region Get Update Exchange Visitor
        public UpdateExchVisitor GetUpdateExchangeVisitor(User user, int personId)
        {
            //do the lookup here for the person's participant info based on latest participant.
            throw new NotImplementedException();
        }

        public UpdateExchVisitor GetUpdateExchangeVisitorAsync(User user, int personId)
        {
            //do the lookup here for the person's participant info based on latest participant.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the update exchange visitor for the participant with the given participant and project ids.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The participant id.</param>
        /// <returns>The update exchange visitor.</returns>
        public UpdateExchVisitor GetUpdateExchangeVisitor(User user, int projectId, int participantId)
        {
            var participant = Context.Participants.Find(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            throwIfParticipantIsNotAPerson(participant);

            var participantPerson = Context.ParticipantPersons.Find(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var participantExchangeVisitor = CreateGetParticipantExchangeVisitorByParticipantIdQuery(participantId).FirstOrDefault();
            throwIfModelDoesNotExist(participantId, participantExchangeVisitor, typeof(ParticipantExchangeVisitor));

            //need to check for multiple countries of citizen...
            var numberOfCitizenships = CreateGetNumberOfCitizenshipsQuery(participantId).Count();
            throwIfMoreThanOneCountryOfCitizenship(participant, numberOfCitizenships);

            var person = Context.People.Find(participant.PersonId.Value);
            throwIfPersonDoesNotHavePlaceOfBirth(person, participantId);
            var cityOfBirth = Context.Locations.Find(person.PlaceOfBirthId.Value);
            throwIfLocationIsNotACity(cityOfBirth.LocationTypeId, participantId);

            var project = Context.Projects.Find(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));

            var exchangeVisitorUpdate = GetExchangeVisitorUpdate(participant, user, participantPerson);
            SetBiographyUpdate(participant, participantPerson, exchangeVisitorUpdate);
            SetFinancialInfoUpdate(exchangeVisitorUpdate, participantExchangeVisitor);

            var updateVisitor = new UpdateExchVisitor
            {
                ExchangeVisitor = exchangeVisitorUpdate
            };
            return updateVisitor;
        }

        /// <summary>
        /// Returns the update exchange visitor for the participant with the given participant and project ids.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The participant id.</param>
        /// <returns>The update exchange visitor.</returns>
        public async Task<UpdateExchVisitor> GetUpdateExchangeVisitorAsync(User user, int projectId, int participantId)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            throwIfParticipantIsNotAPerson(participant);

            var participantPerson = await Context.ParticipantPersons.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var participantExchangeVisitor = await CreateGetParticipantExchangeVisitorByParticipantIdQuery(participantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(participantId, participantExchangeVisitor, typeof(ParticipantExchangeVisitor));

            //need to check for multiple countries of citizen...
            var numberOfCitizenships = await CreateGetNumberOfCitizenshipsQuery(participantId).CountAsync();
            throwIfMoreThanOneCountryOfCitizenship(participant, numberOfCitizenships);

            var person = await Context.People.FindAsync(participant.PersonId.Value);
            throwIfPersonDoesNotHavePlaceOfBirth(person, participantId);
            var cityOfBirth = await Context.Locations.FindAsync(person.PlaceOfBirthId.Value);
            throwIfLocationIsNotACity(cityOfBirth.LocationTypeId, participantId);

            var project = await Context.Projects.FindAsync(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));

            var exchangeVisitorUpdate = GetExchangeVisitorUpdate(participant, user, participantPerson);
            await SetBiographyUpdateAsync(participant, participantPerson, exchangeVisitorUpdate);
            await SetFinancialInfoUpdateAsync(exchangeVisitorUpdate, participantExchangeVisitor);
            var updateVisitor = new UpdateExchVisitor
            {
                ExchangeVisitor = exchangeVisitorUpdate
            };
            return updateVisitor;
        }

        public ExchangeVisitorUpdate GetExchangeVisitorUpdate(Participant participant, User user, ParticipantPerson participantPerson)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            var instance = new ExchangeVisitorUpdate();
            if (String.IsNullOrWhiteSpace(participantPerson.SevisId))
            {
                throw new NotSupportedException(String.Format("The participant with id [{0}] does not have a sevis id.  The update can not take place.", participant.ParticipantId));
            }
            instance.sevisID = participantPerson.SevisId;
            instance.requestID = participant.ParticipantId.ToString();
            instance.userID = user.Id.ToString();
            instance.Reprint = new ReprintFormUpdate
            {
                printForm = true,
                Reason = REPRINT_FORM_UPDATE_REASON_CODE
            };
            instance.Reprint7002 = new Reprint7002
            {
                print7002 = false
            };
            
            return instance;
        }

        #endregion



        #region Get Create Exchange Visitor

        public CreateExchVisitor GetCreateExchangeVisitor(User user, int personId)
        {
            //do the lookup here for the person's participant info based on latest participant.
            throw new NotImplementedException();
        }

        public Task<CreateExchVisitor> GetCreateExchangeVisitorAsync(User user, int personId)
        {
            //do the lookup here for the person's participant info based on latest participant.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the CreateExchVisitor object to validate and send to sevis to create a new exchange visitor.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The participant id.</param>
        public CreateExchVisitor GetCreateExchangeVisitor(User user, int projectId, int participantId)
        {
            var participant = Context.Participants.Find(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            throwIfParticipantIsNotAPerson(participant);

            var participantPerson = Context.ParticipantPersons.Find(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var participantExchangeVisitor = CreateGetParticipantExchangeVisitorByParticipantIdQuery(participantId).FirstOrDefault();
            throwIfModelDoesNotExist(participantId, participantExchangeVisitor, typeof(ParticipantExchangeVisitor));

            //need to check for multiple countries of citizen...
            var numberOfCitizenships = CreateGetNumberOfCitizenshipsQuery(participantId).Count();
            throwIfMoreThanOneCountryOfCitizenship(participant, numberOfCitizenships);

            var person = Context.People.Find(participant.PersonId.Value);
            throwIfPersonDoesNotHavePlaceOfBirth(person, participantId);
            var cityOfBirth = Context.Locations.Find(person.PlaceOfBirthId.Value);
            throwIfLocationIsNotACity(cityOfBirth.LocationTypeId, participantId);

            var project = Context.Projects.Find(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));

            var visitor = GetCreateExchangeVisitor(
                participant: participant,
                user: user,
                project: project,
                visitor: participantExchangeVisitor);

            SetBiography(participant, visitor);
            SetSubjectField(participant, visitor);
            SetMailingAddress(participant, visitor, participantPerson);
            SetUSAddress(participant, visitor, participantPerson);
            SetFinancialInfo(visitor, participantExchangeVisitor);
            SetAddSiteOfActivity(visitor);
            return new CreateExchVisitor
            {
                ExchangeVisitor = visitor
            };
        }

        /// <summary>
        /// Returns the CreateExchVisitor object to validate and send to sevis to create a new exchange visitor.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The participant id.</param>
        /// <returns>The CreateExchVisitor instance</returns>
        public async Task<CreateExchVisitor> GetCreateExchangeVisitorAsync(User user, int projectId, int participantId)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            throwIfParticipantIsNotAPerson(participant);

            var participantPerson = await Context.ParticipantPersons.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var participantExchangeVisitor = await CreateGetParticipantExchangeVisitorByParticipantIdQuery(participantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(participantId, participantExchangeVisitor, typeof(ParticipantExchangeVisitor));

            //need to check for multiple countries of citizen...
            var numberOfCitizenships = await CreateGetNumberOfCitizenshipsQuery(participantId).CountAsync();
            throwIfMoreThanOneCountryOfCitizenship(participant, numberOfCitizenships);
            
            var person = await Context.People.FindAsync(participant.PersonId.Value);
            throwIfPersonDoesNotHavePlaceOfBirth(person, participantId);
            var cityOfBirth = await Context.Locations.FindAsync(person.PlaceOfBirthId.Value);
            throwIfLocationIsNotACity(cityOfBirth.LocationTypeId, participantId);

            var project = await Context.Projects.FindAsync(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));

            var visitor = GetCreateExchangeVisitor(
                participant: participant,
                user: user,
                project: project,
                visitor: participantExchangeVisitor);

            await SetBiographyAsync(participant, visitor);
            await SetSubjectFieldAsync(participant, visitor);
            await SetMailingAddressAsync(participant, visitor, participantPerson);
            await SetUSAddressAsync(participant, visitor, participantPerson);
            await SetFinancialInfoAsync(visitor, participantExchangeVisitor);
            SetAddSiteOfActivity(visitor);
            return new CreateExchVisitor
            {
                ExchangeVisitor = visitor
            };
        }

        public ExchangeVisitor GetCreateExchangeVisitor(Participant participant, User user, Project project, ParticipantExchangeVisitor visitor)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(project != null, "The project must not be null.");
            Contract.Requires(visitor != null, "The visitor must not be null.");
            var instance = new ExchangeVisitor();
            instance.requestID = participant.ParticipantId.ToString();
            instance.userID = user.Id.ToString();
            instance.PrgStartDate = project.StartDate.UtcDateTime;
            instance.PrgEndDate = project.EndDate.HasValue ? project.EndDate.Value.UtcDateTime : default(DateTime?);
            instance.OccupationCategoryCode = EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            if (visitor.Position != null)
            {
                instance.PositionCode = visitor.Position.PositionCode;
            }
            if (visitor.ProgramCategory != null)
            {
                instance.CategoryCode = visitor.ProgramCategory.ProgramCategoryCode;
            }
            return instance;
        }
        #endregion

        #region Site Of Activity

        /// <summary>
        /// Sets the site of activity on the exchange visitor.
        /// </summary>
        /// <param name="visitor">The exchange visitor.</param>
        public void SetAddSiteOfActivity(ExchangeVisitor visitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            visitor.AddSiteOfActivity = new AddSiteOfActivity
            {
                SiteOfActivitySOA = GetStateDepartmentSiteOfActivity(),
                SiteOfActivityExempt = new SiteOfActivityExempt
                {
                    Remarks = String.Empty
                }
            };
        }

        /// <summary>
        /// Returns the state of department site of activity.
        /// </summary>
        /// <returns>The state department site of activity.</returns>
        public SiteOfActivitySOA GetStateDepartmentSiteOfActivity()
        {
            return new SiteOfActivitySOA
            {
                Address1 = SITE_OF_ACTIVITY_STATE_DEPT_ADDRESS_1,
                City = SITE_OF_ACTIVITY_STATE_DEPT_CITY,
                State = SITE_OF_ACTIVITY_STATE_DEPT_STATE,
                PostalCode = SITE_OF_ACTIVITY_STATE_DEPT_POSTAL_CODE,
                SiteName = SITE_OF_ACTIVITY_STATE_DEPT_NAME,
                PrimarySite = true,
                Remarks = String.Empty
            };
        }
        #endregion

        #region Financial Info

        /// <summary>
        /// Sets the financial info on the exchange visitor.
        /// </summary>
        /// <param name="visitor">The exchange visitor.</param>
        /// <param name="participantExchangeVisitor">The participant exchange visitor.</param>
        public void SetFinancialInfo(ExchangeVisitor visitor, ParticipantExchangeVisitor participantExchangeVisitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            var internationalFunding = ExchangeVisitorQueries.CreateGetInternationalFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefault();
            var usGovernmentFunding = ExchangeVisitorQueries.CreateGetUSFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefault();
            SetFinancialInfo(visitor, participantExchangeVisitor, internationalFunding, usGovernmentFunding);
        }

        /// <summary>
        /// Sets the financial info on the exchange visitor.
        /// </summary>
        /// <param name="visitor">The exchange visitor.</param>
        /// <param name="participantExchangeVisitor">The participant exchange visitor.</param>
        /// <returns>The task.</returns>
        public async Task SetFinancialInfoAsync(ExchangeVisitor visitor, ParticipantExchangeVisitor participantExchangeVisitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            var internationalFunding = await ExchangeVisitorQueries.CreateGetInternationalFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefaultAsync();
            var usGovernmentFunding = await ExchangeVisitorQueries.CreateGetUSFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefaultAsync();
            SetFinancialInfo(visitor, participantExchangeVisitor, internationalFunding, usGovernmentFunding);
        }

        /// <summary>
        /// Sets the financial info update information for the exchange visitor update.
        /// </summary>
        /// <param name="visitor">The exchange visitor update.</param>
        /// <param name="participantExchangeVisitor">The participant exchange visitor.</param>
        public void SetFinancialInfoUpdate(ExchangeVisitorUpdate visitor, ParticipantExchangeVisitor participantExchangeVisitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            var internationalFunding = ExchangeVisitorQueries.CreateGetInternationalFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefault();
            var usGovernmentFunding = ExchangeVisitorQueries.CreateGetUSFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefault();
            SetFinancialInfoUpdate(visitor, participantExchangeVisitor, internationalFunding, usGovernmentFunding);
        }

        /// <summary>
        /// Sets the financial info update information for the exchange visitor update.
        /// </summary>
        /// <param name="visitor">The exchange visitor update.</param>
        /// <param name="participantExchangeVisitor">The participant exchange visitor.</param>
        /// <returns>The task.</returns>
        public async Task SetFinancialInfoUpdateAsync(ExchangeVisitorUpdate visitor, ParticipantExchangeVisitor participantExchangeVisitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            var internationalFunding = await ExchangeVisitorQueries.CreateGetInternationalFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefaultAsync();
            var usGovernmentFunding = await ExchangeVisitorQueries.CreateGetUSFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefaultAsync();
            SetFinancialInfoUpdate(visitor, participantExchangeVisitor, internationalFunding, usGovernmentFunding);
        }

        private void SetFinancialInfoUpdate(ExchangeVisitorUpdate visitor, ParticipantExchangeVisitor participantExchangeVisitor, International orgFunding, USGovt usFunding)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            var financialInfoUpdate = GetFinancialInfo(participantExchangeVisitor, orgFunding, usFunding).GetFinancialInfoUpdate();
            visitor.FinancialInfo = financialInfoUpdate;
        }

        private void SetFinancialInfo(ExchangeVisitor visitor, ParticipantExchangeVisitor participantExchangeVisitor, International orgFunding, USGovt usFunding)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            visitor.FinancialInfo = GetFinancialInfo(participantExchangeVisitor, orgFunding, usFunding);
        }

        /// <summary>
        /// Returns sevis participant financial information.
        /// </summary>
        /// <param name="participantExchangeVisitor">The exchange visitor.</param>
        /// <param name="orgFunding">The international organization funding.</param>
        /// <param name="usFunding">The US government funding for the participant.</param>
        /// <returns>The financial info object.</returns>
        public FinancialInfo GetFinancialInfo(ParticipantExchangeVisitor participantExchangeVisitor, International orgFunding, USGovt usFunding)
        {
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            Func<decimal?, string> getFundingAsWholeDollarString = (value) =>
            {
                if (value.HasValue)
                {
                    return ((int)value.Value).ToString();
                }
                else
                {
                    return null;
                }
            };
            var financialInfo = new FinancialInfo();
            if (participantExchangeVisitor.FundingGovtAgency1 > 0.0m || participantExchangeVisitor.FundingGovtAgency2 > 0.0m)
            {
                financialInfo.ReceivedUSGovtFunds = true;
            }
            else
            {
                financialInfo.ReceivedUSGovtFunds = false;
            }
            financialInfo.ProgramSponsorFunds = getFundingAsWholeDollarString(participantExchangeVisitor.FundingSponsor);
            financialInfo.OtherFunds = new OtherFunds
            {
                International = orgFunding,
                USGovt = usFunding,
                EVGovt = getFundingAsWholeDollarString(participantExchangeVisitor.FundingVisGovt),
                BinationalCommission = getFundingAsWholeDollarString(participantExchangeVisitor.FundingVisBNC),
                Personal = getFundingAsWholeDollarString(participantExchangeVisitor.FundingPersonal)
            };
            return financialInfo;
        }
        #endregion

        #region US Address

        /// <summary>
        /// Sets the US address on the exchange visitor.  The us address is based on the host institution.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="visitor">The exchange visitor.</param>
        /// <param name="participantPerson">The participant person.</param>
        /// <returns>The task.</returns>
        public async Task SetUSAddressAsync(Participant participant, ExchangeVisitor visitor, ParticipantPerson participantPerson)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            USAddress usAddress = null;
            if (participantPerson.HostInstitutionAddressId.HasValue)
            {
                usAddress = await ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HostInstitutionAddressId.Value).FirstOrDefaultAsync();
            }
            SetUSAddress(visitor, usAddress);
        }

        /// <summary>
        /// Sets the US address on the exchange visitor.  The us address is based on the host institution.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="visitor">The exchange visitor.</param>
        /// <param name="participantPerson">The participant person.</param>
        public void SetUSAddress(Participant participant, ExchangeVisitor visitor, ParticipantPerson participantPerson)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            USAddress usAddress = null;
            if (participantPerson.HostInstitutionAddressId.HasValue)
            {
                usAddress = ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HostInstitutionAddressId.Value).FirstOrDefault();
            }
            SetUSAddress(visitor, usAddress);
        }

        private void SetUSAddress(ExchangeVisitor visitor, USAddress usAddress)
        {
            if (usAddress != null)
            {
                visitor.USAddress = usAddress;
            }
            else
            {
                visitor.USAddress = null;
            }
        }

        #endregion

        #region Mailing Address

        /// <summary>
        /// Sets the mailing address on the exchange visitor.  The mailing address is based on the home instutition address.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="visitor">The visitor.</param>
        /// <param name="participantPerson">The participant person.</param>
        /// <returns>The task.</returns>
        public async Task SetMailingAddressAsync(Participant participant, ExchangeVisitor visitor, ParticipantPerson participantPerson)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            USAddress usAddress = null;
            if (participantPerson.HomeInstitutionAddressId.HasValue)
            {
                usAddress = await ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HomeInstitutionAddressId.Value).FirstOrDefaultAsync();
            }
            SetMailingAddress(visitor, usAddress);
        }

        /// <summary>
        /// Sets the mailing address on the exchange visitor.  The mailing address is based on the home instutition address.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="visitor">The visitor.</param>
        /// <param name="participantPerson">The participant person.</param>
        public void SetMailingAddress(Participant participant, ExchangeVisitor visitor, ParticipantPerson participantPerson)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            USAddress usAddress = null;
            if (participantPerson.HomeInstitutionAddressId.HasValue)
            {
                usAddress = ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HomeInstitutionAddressId.Value).FirstOrDefault();
            }
            SetMailingAddress(visitor, usAddress);
        }

        private void SetMailingAddress(ExchangeVisitor visitor, USAddress mailingAddress)
        {
            if (mailingAddress != null)
            {
                visitor.MailAddress = mailingAddress;
            }
            else
            {
                visitor.MailAddress = null;
            }
        }
        #endregion

        #region Biography

        /// <summary>
        /// Sets the biographical information on the exchange visitor.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="visitor">The exchange visitor.</param>
        public async Task SetBiographyAsync(Participant participant, ExchangeVisitor visitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            var biography = await ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.Context, participant.ParticipantId).FirstOrDefaultAsync();
            SetBiography(participant, visitor, biography);
        }

        /// <summary>
        /// Sets the biographical information on the exchange visitor.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="visitor">The exchange visitor.</param>
        public void SetBiography(Participant participant, ExchangeVisitor visitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            var biography = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.Context, participant.ParticipantId).FirstOrDefault();
            SetBiography(participant, visitor, biography);
        }

        private void SetBiography(Participant participant, ExchangeVisitor visitor, BiographicalDTO biography)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            if (biography == null)
            {
                throw new NotSupportedException(String.Format("The participant with id [{0}] must have biographical information.", participant.ParticipantId));
            }
            visitor.Biographical = biography.GetBiographical();
        }

        /// <summary>
        /// Sets the update biography model on the exchange visitor update.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="participantPerson">The participant person.</param>
        /// <param name="visitor">The exchange visitor update.</param>
        /// <returns>The task.</returns>
        public async Task SetBiographyUpdateAsync(Participant participant, ParticipantPerson participantPerson, ExchangeVisitorUpdate visitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            USAddress usAddress = null; //host
            USAddress mailingAddress = null; //home

            if (participantPerson.HostInstitutionAddressId.HasValue)
            {
                usAddress = await ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HostInstitutionAddressId.Value).FirstOrDefaultAsync();
            }
            if (participantPerson.HomeInstitutionAddressId.HasValue)
            {
                mailingAddress = await ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HomeInstitutionAddressId.Value).FirstOrDefaultAsync();
            }
            var biography = await ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.Context, participant.ParticipantId).FirstOrDefaultAsync();
            SetBiographyUpdate(
                participant: participant, 
                visitor: visitor, 
                biography: biography, 
                mailingAddress: mailingAddress, 
                usAddress: usAddress);
        }

        /// <summary>
        /// Sets the update biography model on the exchange visitor update.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="participantPerson">The participant person.</param>
        /// <param name="visitor">The exchange visitor update.</param>
        public void SetBiographyUpdate(Participant participant, ParticipantPerson participantPerson, ExchangeVisitorUpdate visitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            USAddress usAddress = null; //host
            USAddress mailingAddress = null; //home

            if (participantPerson.HostInstitutionAddressId.HasValue)
            {
                usAddress = ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HostInstitutionAddressId.Value).FirstOrDefault();
            }
            if (participantPerson.HomeInstitutionAddressId.HasValue)
            {
                mailingAddress = ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HomeInstitutionAddressId.Value).FirstOrDefault();
            }
            var biography = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.Context, participant.ParticipantId).FirstOrDefault();
            SetBiographyUpdate(
                participant: participant,
                visitor: visitor,
                biography: biography,
                mailingAddress: mailingAddress,
                usAddress: usAddress);
        }

        private void SetBiographyUpdate(Participant participant, ExchangeVisitorUpdate visitor, BiographicalDTO biography, USAddress mailingAddress, USAddress usAddress)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            if (biography == null)
            {
                throw new NotSupportedException(String.Format("The participant with id [{0}] must have biographical information.", participant.ParticipantId));
            }
            visitor.Biographical = biography.GetBiographicalUpdate(mailingAddress, usAddress);
        }

        #endregion

        #region Subject Field

        /// <summary>
        /// Sets the SevisSubject field on the exchange visitor.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="visitor">The visitor to set the subject field.</param>
        /// <returns>The task.</returns>
        public async Task SetSubjectFieldAsync(Participant participant, ExchangeVisitor visitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            var fieldOfStudy = await ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(this.Context, participant.ParticipantId).FirstOrDefaultAsync();
            SetSubjectField(participant, visitor, fieldOfStudy);
        }

        /// <summary>
        /// Sets the SevisSubject field on the exchange visitor.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="visitor">The visitor to set the subject field.</param>
        public void SetSubjectField(Participant participant, ExchangeVisitor visitor)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            var fieldOfStudy = ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(this.Context, participant.ParticipantId).FirstOrDefault();
            SetSubjectField(participant, visitor, fieldOfStudy);
        }

        private void SetSubjectField(Participant participant, ExchangeVisitor visitor, SubjectField subjectField)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            if (subjectField != null && subjectField.Remarks != null && subjectField.Remarks.Length > SUBJECT_FIELD_REMARKS_MAX_LENGTH)
            {
                subjectField.Remarks = subjectField.Remarks.Substring(0, SUBJECT_FIELD_REMARKS_MAX_LENGTH);
            }
            visitor.SubjectField = subjectField;
        }
        #endregion

        private IQueryable<ParticipantExchangeVisitor> CreateGetParticipantExchangeVisitorByParticipantIdQuery(int participantId)
        {
            return Context.ParticipantExchangeVisitors
                .Include(x => x.Position)
                .Include(x => x.ProgramCategory)
                .Where(x => x.ParticipantId == participantId);
        }

        private IQueryable<int> CreateGetNumberOfCitizenshipsQuery(int participantId)
        {
            return Context.Participants.Where(p => p.ParticipantId == participantId)
                .Select(x => x.Person)
                .SelectMany(x => x.CountriesOfCitizenship)
                .Select(x => x.LocationId);
        }
    }
}
