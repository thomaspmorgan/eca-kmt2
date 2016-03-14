using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Queries.Persons;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly Action<Participant, Project> throwIfProjectIsNotExchangeVisitorType;

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
            throwIfProjectIsNotExchangeVisitorType = (participant, project) =>
            {
                if (project.VisitorTypeId != VisitorType.ExchangeVisitor.Id)
                {
                    throw new NotSupportedException(String.Format("The participant with id [{0}] belongs to a project with id [{1}] that is not an exchange visitor project.", participant.ParticipantId, project.ProjectId));
                }
            };
        }

        #region Get Exchange Visitor

        /// <summary>
        /// Returns the exchange visitor for the participant with the given id.
        /// </summary>
        /// <param name="user">The user requesting the exchange visitor.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The exchange visitor model representing the participant.</returns>
        public ExchangeVisitor GetExchangeVisitor(User user, int projectId, int participantId)
        {
            var participant = Context.Participants.Find(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            throwIfParticipantIsNotAPerson(participant);

            var participantPerson = Context.ParticipantPersons.Find(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var participantExchangeVisitor = CreateGetParticipantExchangeVisitorByParticipantIdQuery(participantId).FirstOrDefault();

            var project = Context.Projects.Find(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));
            throwIfProjectIsNotExchangeVisitorType(participant, project);

            var biographyDto = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.Context, participantId).FirstOrDefault();
            Contract.Assert(biographyDto != null, "The biography should not be null.");

            var subjectField = ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(this.Context, participantId).FirstOrDefault();
            var siteOfActivityAddress = GetStateDepartmentCStreetAddress();
            var person = GetPerson(biography: biographyDto, participantExchangeVisitor: participantExchangeVisitor, subjectFieldDTO: subjectField, siteOfActivityAddress: siteOfActivityAddress);
            var financialInfo = GetFinancialInfo(participantExchangeVisitor);
            var dependents = ExchangeVisitorQueries.CreateGetParticipantDependentsBiographicalQuery(this.Context, participantId).ToList();

            return GetExchangeVisitor(
                user: user,
                person: person,
                financialInfo: financialInfo,
                participantPerson: participantPerson,
                occupationCategoryCode: EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE,
                dependents: dependents,
                siteOfActivity: siteOfActivityAddress);
        }

        /// <summary>
        /// Returns the exchange visitor for the participant with the given id.
        /// </summary>
        /// <param name="user">The user requesting the exchange visitor.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The exchange visitor model representing the participant.</returns>
        public async Task<ExchangeVisitor> GetExchangeVisitorAsync(User user, int projectId, int participantId)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            throwIfParticipantIsNotAPerson(participant);

            var participantPerson = await Context.ParticipantPersons.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var participantExchangeVisitor = await CreateGetParticipantExchangeVisitorByParticipantIdQuery(participantId).FirstOrDefaultAsync();

            var project = await Context.Projects.FindAsync(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));
            throwIfProjectIsNotExchangeVisitorType(participant, project);

            var biographyDto = await ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            Contract.Assert(biographyDto != null, "The biography should not be null.");

            var subjectField = await ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            var siteOfActivityAddress = GetStateDepartmentCStreetAddress();
            var person = GetPerson(biography: biographyDto, participantExchangeVisitor: participantExchangeVisitor, subjectFieldDTO: subjectField, siteOfActivityAddress: siteOfActivityAddress);
            var financialInfo = await GetFinancialInfoAsync(participantExchangeVisitor);
            var dependents = await ExchangeVisitorQueries.CreateGetParticipantDependentsBiographicalQuery(this.Context, participantId).ToListAsync();

            return GetExchangeVisitor(
                user: user,
                person: person,
                financialInfo: financialInfo,
                participantPerson: participantPerson,
                occupationCategoryCode: EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE,
                dependents: dependents,
                siteOfActivity: siteOfActivityAddress);
        }

        /// <summary>
        /// Returns the person model for an exchange visitor.
        /// </summary>
        /// <param name="biography">The biography of the participant.</param>
        /// <param name="participantExchangeVisitor">The participant exchange visitor record.  This record should have the program category pre-loaded.</param>
        /// <param name="subjectFieldDTO">The field of study of the participant.</param>
        /// <param name="siteOfActivityAddress">the site of activity of the exchange visitor, i.e. C Street state dept.</param>
        /// <returns>The person model for the exchange visitor.</returns>
        public ECA.Business.Validation.Sevis.Bio.Person GetPerson(
            BiographicalDTO biography,
            ParticipantExchangeVisitor participantExchangeVisitor,
            SubjectFieldDTO subjectFieldDTO,
            AddressDTO siteOfActivityAddress)
        {
            Contract.Requires(biography != null, "The biography should not be null.");
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            Contract.Requires(siteOfActivityAddress != null, "The site of activity address must not be null.");
            SubjectField subjectField = null;
            FullName fullName = null;
            if (subjectFieldDTO != null)
            {
                subjectField = subjectFieldDTO.GetSubjectField();
            }
            if (biography.FullName != null)
            {
                fullName = biography.FullName.GetFullName();
            }
            var instance = new ECA.Business.Validation.Sevis.Bio.Person(
                fullName: fullName,
                birthCity: biography.BirthCity,
                birthCountryCode: biography.BirthCountryCode,
                birthCountryReason: biography.BirthCountryReason,
                birthDate: biography.BirthDate,
                citizenshipCountryCode: biography.CitizenshipCountryCode,
                emailAddress: biography.EmailAddress,
                genderCode: biography.Gender,
                permanentResidenceCountryCode: biography.PermanentResidenceCountryCode,
                phoneNumber: biography.PhoneNumber,
                remarks: null,
                positionCode: biography.PositionCode,
                programCategoryCode: participantExchangeVisitor.ProgramCategory != null ? participantExchangeVisitor.ProgramCategory.ProgramCategoryCode : null,
                mailAddress: biography.MailAddress,
                participantId: participantExchangeVisitor.ParticipantId,
                usAddress: siteOfActivityAddress,
                personId: biography.PersonId,
                printForm: true,
                subjectField: subjectField
                );
            return instance;
        }

        /// <summary>
        /// Returns the exchange visitor model given the required exchange vistor information.
        /// </summary>
        /// <param name="user">The user requesting the exchange visitor.</param>
        /// <param name="person">The exchange visitor person model.</param>
        /// <param name="financialInfo">The financial information.</param>
        /// <param name="participantPerson">The participant person record for the participant.</param>
        /// <param name="occupationCategoryCode">The occupation category code.</param>
        /// <param name="dependents">The dependents for the exchange visitor.</param>
        /// <param name="siteOfActivity">The site of activity, i.e. C Street state dept.</param>
        /// <returns>The exchange visitor.</returns>
        public ExchangeVisitor GetExchangeVisitor(
            User user, 
            Validation.Sevis.Bio.Person person, 
            FinancialInfo financialInfo, 
            ParticipantPerson participantPerson, 
            string occupationCategoryCode, 
            IEnumerable<DependentBiographicalDTO> dependents, 
            AddressDTO siteOfActivity)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(person != null, "The person must not be null.");
            Contract.Requires(financialInfo != null, "The financial info must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            Contract.Requires(siteOfActivity != null, "The site of activity must not be null.");

            var exchangeVisitorDependents = new List<Dependent>();
            if (dependents != null)
            {
                foreach(var dependent in dependents)
                {
                    exchangeVisitorDependents.Add(dependent.GetDependent(siteOfActivity, null));
                }
            }

            return new ExchangeVisitor(
                user: user,
                sevisId: participantPerson.SevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: participantPerson.EndDate.HasValue ? participantPerson.EndDate.Value.DateTime : default(DateTime),
                programStartDate: participantPerson.StartDate.HasValue ? participantPerson.StartDate.Value.DateTime : default(DateTime),
                dependents: exchangeVisitorDependents,
                siteOfActivity: siteOfActivity);
        }
        #endregion


        #region State Dept Address

        /// <summary>
        /// Returns a USAddress instance of the US State Department C Street location.
        /// </summary>
        /// <returns>A USAddress instance of the US State Department C Street location.</returns>
        public AddressDTO GetStateDepartmentCStreetAddress()
        {
            return new AddressDTO
            {
                Street1 = SITE_OF_ACTIVITY_STATE_DEPT_ADDRESS_1,
                Street2 = null,
                Street3 = null,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME,
                City = SITE_OF_ACTIVITY_STATE_DEPT_CITY,
                Division = SITE_OF_ACTIVITY_STATE_DEPT_STATE,
                PostalCode = SITE_OF_ACTIVITY_STATE_DEPT_POSTAL_CODE,
                LocationName = SITE_OF_ACTIVITY_STATE_DEPT_NAME
            };
        }
        #endregion


        #region Financial Info

        /// <summary>
        /// Returns the financial info of the participant.
        /// </summary>
        /// <param name="participantExchangeVisitor">The participant.</param>
        /// <returns>The financial info for the exchange visitor.</returns>
        public FinancialInfo GetFinancialInfo(ParticipantExchangeVisitor participantExchangeVisitor)
        {
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            var internationFundingDTO = ExchangeVisitorQueries.CreateGetInternationalFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefault();
            var usFundingDTO = ExchangeVisitorQueries.CreateGetUSFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefault();
            return GetFinancialInfo(participantExchangeVisitor, internationFundingDTO, usFundingDTO);
        }

        /// <summary>
        /// Returns the financial info of the participant.
        /// </summary>
        /// <param name="participantExchangeVisitor">The participant.</param>
        /// <returns>The financial info for the exchange visitor.</returns>
        public async Task<FinancialInfo> GetFinancialInfoAsync(ParticipantExchangeVisitor participantExchangeVisitor)
        {
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            var internationFundingDTO = await ExchangeVisitorQueries.CreateGetInternationalFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefaultAsync();
            var usFundingDTO = await ExchangeVisitorQueries.CreateGetUSFundingQuery(this.Context, participantExchangeVisitor.ParticipantId).FirstOrDefaultAsync();
            return GetFinancialInfo(participantExchangeVisitor, internationFundingDTO, usFundingDTO);
        }

        /// <summary>
        /// Returns sevis participant financial information.
        /// </summary>
        /// <param name="participantExchangeVisitor">The exchange visitor.</param>
        /// <param name="orgFunding">The international organization funding.</param>
        /// <param name="usFunding">The US government funding for the participant.</param>
        /// <returns>The financial info object.</returns>
        public FinancialInfo GetFinancialInfo(ParticipantExchangeVisitor participantExchangeVisitor, ExchangeVisitorFundingDTO orgFunding, ExchangeVisitorFundingDTO usFunding)
        {
            Contract.Requires(participantExchangeVisitor != null, "The participant exchange visitor must not be null.");
            Func<decimal?, string> getFundingAsWholeDollarString = (value) =>
            {
                if (value.HasValue && value.Value > 0.0m)
                {
                    return ((int)value.Value).ToString();
                }
                else
                {
                    return null;
                }
            };

            var receivedUsGovtFunds = false;
            if ((participantExchangeVisitor.FundingGovtAgency1.HasValue && participantExchangeVisitor.FundingGovtAgency1.Value > 0.0m)
                || (participantExchangeVisitor.FundingGovtAgency2.HasValue && participantExchangeVisitor.FundingGovtAgency2.Value > 0.0m))
            {
                receivedUsGovtFunds = true;
            }
            var programSponsorFunds = getFundingAsWholeDollarString(participantExchangeVisitor.FundingSponsor);
            Other other = null;
            if (participantExchangeVisitor.FundingOther.HasValue && participantExchangeVisitor.FundingOther.Value > 0.0m)
            {
                other = new Other(
                    name: participantExchangeVisitor.OtherName,
                    amount: getFundingAsWholeDollarString(participantExchangeVisitor.FundingOther));
            }

            var otherFunds = new OtherFunds(
                evGovt: getFundingAsWholeDollarString(participantExchangeVisitor.FundingVisGovt),
                binationalCommission: getFundingAsWholeDollarString(participantExchangeVisitor.FundingVisBNC),
                personal: getFundingAsWholeDollarString(participantExchangeVisitor.FundingPersonal),
                usGovt: usFunding != null ? usFunding.GetUSGovt() : null,
                international: orgFunding != null ? orgFunding.GetInternational() : null,
                other: other);

            var financialInfo = new FinancialInfo(
                printForm: true,
                receivedUSGovtFunds: receivedUsGovtFunds,
                programSponsorFunds: programSponsorFunds,
                otherFunds: otherFunds);

            return financialInfo;
        }
        #endregion

        private IQueryable<ParticipantExchangeVisitor> CreateGetParticipantExchangeVisitorByParticipantIdQuery(int participantId)
        {
            return Context.ParticipantExchangeVisitors
                .Include(x => x.Position)
                .Include(x => x.ProgramCategory)
                .Where(x => x.ParticipantId == participantId);
        }
    }
}
