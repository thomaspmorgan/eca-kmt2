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

        #region Get Update Exchange Visitor

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

            var project = Context.Projects.Find(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));
            throwIfProjectIsNotExchangeVisitorType(participant, project);

            var exchangeVisitorUpdate = GetExchangeVisitorUpdate(participant, user, participantPerson);
            SetBiographyUpdate(participant, participantPerson, exchangeVisitorUpdate);
            SetFinancialInfoUpdate(exchangeVisitorUpdate, participantExchangeVisitor);
            SetUSAddress(participant, exchangeVisitorUpdate, participantPerson);
            SetMailingAddress(participant, exchangeVisitorUpdate, participantPerson);
            SetDependents(participant, exchangeVisitorUpdate);
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

            var project = await Context.Projects.FindAsync(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));
            throwIfProjectIsNotExchangeVisitorType(participant, project);

            var exchangeVisitorUpdate = GetExchangeVisitorUpdate(participant, user, participantPerson);
            await SetBiographyUpdateAsync(participant, participantPerson, exchangeVisitorUpdate);
            await SetFinancialInfoUpdateAsync(exchangeVisitorUpdate, participantExchangeVisitor);
            await SetUSAddressAsync(participant, exchangeVisitorUpdate, participantPerson);
            await SetMailingAddressAsync(participant, exchangeVisitorUpdate, participantPerson);
            await SetDependentsAsync(participant, exchangeVisitorUpdate);
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

            var project = Context.Projects.Find(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));
            throwIfProjectIsNotExchangeVisitorType(participant, project);

            var visitor = GetCreateExchangeVisitor(
                participant: participant,
                user: user,
                participantPerson: participantPerson,
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

            var project = await Context.Projects.FindAsync(participant.ProjectId);
            throwIfModelDoesNotExist(participant.ProjectId, project, typeof(Project));
            throwIfProjectIsNotExchangeVisitorType(participant, project);

            var visitor = GetCreateExchangeVisitor(
                participant: participant,
                user: user,
                participantPerson: participantPerson,
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

        public ExchangeVisitor GetCreateExchangeVisitor(Participant participant, User user, ParticipantPerson participantPerson,  ParticipantExchangeVisitor visitor)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            var instance = new ExchangeVisitor();
            instance.requestID = participant.ParticipantId.ToString();
            instance.userID = user.Id.ToString();
            instance.PrgStartDate = participantPerson.StartDate.HasValue ? participantPerson.StartDate.Value.UtcDateTime : default(DateTime);
            instance.PrgEndDate = participantPerson.EndDate.HasValue ? participantPerson.EndDate.Value.UtcDateTime : default(DateTime);

            instance.OccupationCategoryCode = EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            if (visitor != null && visitor.Position != null)
            {
                instance.PositionCode = visitor.Position.PositionCode;
            }
            if (visitor != null && visitor.ProgramCategory != null)
            {
                instance.CategoryCode = visitor.ProgramCategory.ProgramCategoryCode;
            }
            SetResidentialAddress(instance);
            SetTIPP(instance);
            return instance;
        }
        #endregion

        #region TIPP

        /// <summary>
        /// Sets TIPP information on the exchange visitor and the dependents.
        /// </summary>
        /// <param name="visitor">The exchange visitor.</param>
        public void SetTIPP(ExchangeVisitor visitor)
        {
            visitor.AddTIPP = new EcaAddTIPP();
            foreach(var dependent in visitor.CreateDependent)
            {
                dependent.AddTIPP = new EcaAddTIPP();
            }
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
                if (value.HasValue && value.Value > 0.0m)
                {
                    return ((int)value.Value).ToString();
                }
                else
                {
                    return null;
                }
            };
            var financialInfo = new FinancialInfo();
            if ((participantExchangeVisitor.FundingGovtAgency1.HasValue && participantExchangeVisitor.FundingGovtAgency1.Value > 0.0m)
                || (participantExchangeVisitor.FundingGovtAgency2.HasValue && participantExchangeVisitor.FundingGovtAgency2.Value > 0.0m))
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
                International = orgFunding != null ? orgFunding : null,
                USGovt = usFunding != null ? usFunding : null,
                EVGovt = getFundingAsWholeDollarString(participantExchangeVisitor.FundingVisGovt),
                BinationalCommission = getFundingAsWholeDollarString(participantExchangeVisitor.FundingVisBNC),
                Personal = getFundingAsWholeDollarString(participantExchangeVisitor.FundingPersonal),
            };
            if (participantExchangeVisitor.FundingOther.HasValue && participantExchangeVisitor.FundingOther.Value > 0.0m)
            {
                financialInfo.OtherFunds.Other = new Other
                {
                    Name = participantExchangeVisitor.OtherName,
                    Amount = getFundingAsWholeDollarString(participantExchangeVisitor.FundingOther)
                };
            }
            else
            {
                financialInfo.OtherFunds.Other = null;
            }
            //these two null checks are to set the otherfunds.International and otherfund.USGovt to null
            //if the amount1(s) are null since, the structures are optional and we don't want validation to run
            //on optional structures.
            if (orgFunding != null && String.IsNullOrWhiteSpace(orgFunding.Amount1))
            {
                if (!String.IsNullOrWhiteSpace(orgFunding.Amount2))
                {
                    throw new NotSupportedException("The International Funding Amount1 must have a value if Amount2 has a value.");
                }
                financialInfo.OtherFunds.International = null;
            }
            if (usFunding != null && String.IsNullOrWhiteSpace(usFunding.Amount1))
            {
                if (!String.IsNullOrWhiteSpace(usFunding.Amount2))
                {
                    throw new NotSupportedException("The US Government Funding Amount1 must have a value if Amount2 has a value.");
                }
                financialInfo.OtherFunds.USGovt = null;
            }
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
        public async Task SetUSAddressAsync(Participant participant, ExchangeVisitorUpdate visitor, ParticipantPerson participantPerson)
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
        public void SetUSAddress(Participant participant, ExchangeVisitorUpdate visitor, ParticipantPerson participantPerson)
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

        private void SetUSAddress(ExchangeVisitorUpdate visitor, USAddress usAddress)
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
        public async Task SetMailingAddressAsync(Participant participant, ExchangeVisitorUpdate visitor, ParticipantPerson participantPerson)
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
        public void SetMailingAddress(Participant participant, ExchangeVisitorUpdate visitor, ParticipantPerson participantPerson)
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

        private void SetMailingAddress(ExchangeVisitorUpdate visitor, USAddress mailingAddress)
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
            //USAddress usAddress = null; //host
            //USAddress mailingAddress = null; //home

            //if (participantPerson.HostInstitutionAddressId.HasValue)
            //{
            //    usAddress = await ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HostInstitutionAddressId.Value).FirstOrDefaultAsync();
            //}
            //if (participantPerson.HomeInstitutionAddressId.HasValue)
            //{
            //    mailingAddress = await ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HomeInstitutionAddressId.Value).FirstOrDefaultAsync();
            //}
            var biography = await ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.Context, participant.ParticipantId).FirstOrDefaultAsync();
            SetBiographyUpdate(
                participant: participant,
                visitor: visitor,
                biography: biography);
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
            //USAddress usAddress = null; //host
            //USAddress mailingAddress = null; //home

            //if (participantPerson.HostInstitutionAddressId.HasValue)
            //{
            //    usAddress = ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HostInstitutionAddressId.Value).FirstOrDefault();
            //}
            //if (participantPerson.HomeInstitutionAddressId.HasValue)
            //{
            //    mailingAddress = ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(this.Context, participantPerson.HomeInstitutionAddressId.Value).FirstOrDefault();
            //}
            var biography = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.Context, participant.ParticipantId).FirstOrDefault();
            SetBiographyUpdate(
                participant: participant,
                visitor: visitor,
                biography: biography);
        }

        private void SetBiographyUpdate(Participant participant, ExchangeVisitorUpdate visitor, BiographicalDTO biography)
        {
            Contract.Requires(visitor != null, "The visitor must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            if (biography == null)
            {
                throw new NotSupportedException(String.Format("The participant with id [{0}] must have biographical information.", participant.ParticipantId));
            }
            visitor.Biographical = biography.GetBiographicalUpdate();
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

        #region Dependents
        /// <summary>
        /// Sets the dependents of the participant to the exchange visitor.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="exchangeVisitor">The exchange visitor.</param>
        public void SetDependents(Participant participant, ExchangeVisitor exchangeVisitor)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(exchangeVisitor != null, "The exchange visitor must not be null.");
            exchangeVisitor.CreateDependent = new List<CreateDependent>();
        }

        /// <summary>
        /// Sets the dependents of the participant to the exchange visitor.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="exchangeVisitor">The exchange visitor.</param>
        public Task SetDependentsAsync(Participant participant, ExchangeVisitor exchangeVisitor)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(exchangeVisitor != null, "The exchange visitor must not be null.");
            SetDependents(participant, exchangeVisitor);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Sets the dependents of the participant to the exchange visitor.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="exchangeVisitor">The exchange visitor.</param>
        public void SetDependents(Participant participant, ExchangeVisitorUpdate exchangeVisitor)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(exchangeVisitor != null, "The exchange visitor must not be null.");
            exchangeVisitor.Dependent = null;
        }

        /// <summary>
        /// Sets the dependents of the participant to the exchange visitor.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="exchangeVisitor">The exchange visitor.</param>
        public Task SetDependentsAsync(Participant participant, ExchangeVisitorUpdate exchangeVisitor)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(exchangeVisitor != null, "The exchange visitor must not be null.");
            SetDependents(participant, exchangeVisitor);
            return Task.FromResult<object>(null);
        }
        #endregion

        #region Residential Address

        /// <summary>
        /// Sets a null residential address on the given exchange visitor.
        /// </summary>
        /// <param name="exchangeVisitor">The exchange visitor to set a null value on the residential address.</param>

        public void SetResidentialAddress(ExchangeVisitor exchangeVisitor)
        {
            Contract.Requires(exchangeVisitor != null, "The exchange visitor must not be null.");
            exchangeVisitor.ResidentialAddress = null;

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
