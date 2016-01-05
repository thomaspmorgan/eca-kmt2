﻿using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using NLog;
using ECA.Core.Exceptions;
using ECA.Business.Validation;
using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Service.Persons
{

    /// <summary>
    /// A ParticipantPersonService is capable of performing crud operations on participantPersons in the ECA system.
    /// </summary>
    public class ParticipantPersonsSevisService : DbContextService<EcaContext>, IParticipantPersonsSevisService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        
        /// <summary>
        /// Creates a new ParticipantPersonService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantPersonsSevisService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfModelDoesNotExist = (id, instance, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model of type [{0}] with id [{1}] was not found.", type.Name, id));
                }
            };
        }

        #region Get

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonsSevis(QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons with query operator [{0}].", queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonsSevisAsync(QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons with query operator [{0}].", queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        public PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByProjectId(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonsSevisByProjectIdAsync(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns a participantPersonSevis
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public ParticipantPersonSevisDTO GetParticipantPersonsSevisById(int participantId)
        {
            var participantPersonSevis = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevis;
        }

        /// <summary>
        /// Returns a participantPersonSevis asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public Task<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByIdAsync(int participantId)
        {
            var participantPersonSevis = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevis;
        }

        /// <summary>
        /// Get populated create participant sevis object
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns></returns>
        public SEVISBatchCreateUpdateEV GetCreateExchangeVisitor(int participantId)
        {
            //Get student details
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            var participantPerson = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            var personalPII = PersonQueries.CreateGetPiiByIdQuery(this.Context, (int)participant.PersonId).FirstOrDefault();
            var personalEmail = PersonQueries.CreateGetContactInfoByIdQuery(this.Context, (int)participant.PersonId).Select(x => x.EmailAddresses).FirstOrDefault();
            var mailingAddress = Context.Locations.Where(x => x.LocationId == participantPerson.HomeInstitutionAddressId).FirstOrDefault();
            var physicalAddress = Context.Locations.Where(x => x.LocationId == participantPerson.HostInstitutionAddressId).FirstOrDefault();
            var locid = personalPII.CountriesOfCitizenship.Select(c => c.Id).FirstOrDefault();
            var citizenship = Context.Locations.Where(x => x.LocationId == locid).FirstOrDefault();

            var ExchVisitor = new ExchangeVisitor();
            // biographical
            ExchVisitor.Biographical = new Biographical
            {
                BirthCity = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.City : "",
                BirthCountryCode = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.CountryIso2 : "",
                BirthCountryReason = "",
                BirthDate = personalPII.DateOfBirth != null ? personalPII.DateOfBirth.Value.Date : (DateTime?)null,
                CitizenshipCountryCode = citizenship != null ? citizenship.LocationIso2 : "",
                EmailAddress = personalEmail != null ? personalEmail.Select(x => x.Address).FirstOrDefault() : "",
                FullName = new FullName
                {
                    FirsName = personalPII.FirstName,
                    LastName = personalPII.LastName,
                    Suffix = personalPII.NameSuffix,
                    PreferredName = personalPII.Alias
                },
                Gender = personalPII.GenderId.ToString()
            };
            ExchVisitor.PositionCode = "";
            ExchVisitor.PrgStartDate = new DateTime(2019, 4, 18);
            ExchVisitor.PrgEndDate = (DateTime?)null;
            ExchVisitor.CategoryCode = "";
            ExchVisitor.OccupationCategoryCode = "";
            ExchVisitor.SubjectField = new SubjectField
            {
                SubjectFieldCode = "12 1234",
                ForeignDegreeLevel = "Bachelors",
                ForeignFieldOfStudy = "Business",
                Remarks = "testing"
            };
            // addresses
            if (physicalAddress != null)
            {
                ExchVisitor.USAddress = new USAddress
                {
                    Address1 = physicalAddress.Street1,
                    Address2 = physicalAddress.Street2,
                    City = physicalAddress.City.LocationName,
                    State = physicalAddress.Division.LocationName,
                    PostalCode = physicalAddress.PostalCode
                };
            }
            if (mailingAddress != null)
            {
                ExchVisitor.MailAddress = new USAddress
                {
                    Address1 = mailingAddress.Street1,
                    Address2 = mailingAddress.Street2,
                    City = mailingAddress.City.LocationName,
                    State = mailingAddress.Division.LocationName,
                    PostalCode = mailingAddress.PostalCode
                };
            }
            // financial
            ExchVisitor.FinancialInfo = new FinancialInfo
            {
                ReceivedUSGovtFunds = false,
                ProgramSponsorFunds = "1200",
                OtherFunds = new OtherFunds
                {
                    International = new International
                    {
                        Amount1 = "10",
                        Amount2 = "20",
                        Org1 = "org 1",
                        OtherName1 = "on 1",
                        Org2 = "org 2",
                        OtherName2 = "on 2"
                    },
                    USGovt = new USGovt
                    {
                        Amount1 = "10",
                        Amount2 = "20",
                        Org1 = "govorg 1",
                        OtherName1 = "govon 1",
                        Org2 = "govorg 2",
                        OtherName2 = "govon 2"
                    },
                    Other = new Other
                    {
                        amount = "30",
                        name = "other nm"
                    }
                }
            };
            // TODO: complete when dependent feature is available
            //ExchVisitor.CreateDependent = new CreateDependent
            //{
            //    Dependent = new AddDependent
            //    {
            //        BirthDate = new DateTime(1988, 4, 18),
            //        Gender = "1",
            //        BirthCountryCode = "01",
            //        CitizenshipCountryCode = "01",
            //        FullName = new FullName
            //        {
            //            FirsName = "Some",
            //            LastName = "Dependent"
            //        }
            //    }
            //};
            // site of activity
            ExchVisitor.AddSiteOfActivity = new AddSiteOfActivity
            {
                SiteOfActivitySOA = new SiteOfActivitySOA
                {
                    printForm = false,
                    Address1 = "123 Some St",
                    City = "Arlington",
                    State = "VA",
                    PostalCode = "22206",
                    SiteName = "Office 1",
                    PrimarySite = true,
                    Remarks = "Test site"                    
                },
                SiteOfActivityExempt = new SiteOfActivityExempt
                {
                    Remarks = "test site exempt"
                }
            };
            // T/IPP
            ExchVisitor.AddTIPP = new AddTIPP
            {
                print7002 = false,
                ParticipantInfo = new ParticipantInfo
                {
                    EmailAddress = "test@domain.com",
                    FieldOfStudy = "Business",
                    TypeOfDegree = "Bachelors",
                    DateAwardedOrExpected = new DateTime(2009, 5, 12)
                }
            };

            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };

            var createVisitor = new CreateExchVisitor
            {
                ExchangeVisitor = ExchVisitor
            };

            var createEVBatch = new SEVISBatchCreateUpdateEV
            {
                userID = "1",
                BatchHeader = batchHeader,
                UpdateEV = null,
                CreateEV = createVisitor
            };

            return createEVBatch;
        }

        /// <summary>
        /// Get populated update participant sevis object
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns></returns>
        public SEVISBatchCreateUpdateEV GetUpdateExchangeVisitor(int participantId)
        {
            //Get student details
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            var participantPerson = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            var personalPII = PersonQueries.CreateGetPiiByIdQuery(this.Context, (int)participant.PersonId).FirstOrDefault();
            var personalEmail = PersonQueries.CreateGetContactInfoByIdQuery(this.Context, (int)participant.PersonId).Select(x => x.EmailAddresses).FirstOrDefault();
            var mailingAddress = Context.Locations.Where(x => x.LocationId == participantPerson.HomeInstitutionAddressId).FirstOrDefault();
            var physicalAddress = Context.Locations.Where(x => x.LocationId == participantPerson.HostInstitutionAddressId).FirstOrDefault();
            var locid = personalPII.CountriesOfCitizenship.Select(c => c.Id).FirstOrDefault();
            var citizenship = Context.Locations.Where(x => x.LocationId == locid).FirstOrDefault();

            var ExchVisitor = new ExchangeVisitorUpdate();
            // biographical
            ExchVisitor.Biographical = new BiographicalUpdate
            {
                BirthCity = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.City : "",
                BirthCountryCode = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.CountryIso2 : "",
                BirthCountryReason = "",
                BirthDate = personalPII.DateOfBirth != null ? personalPII.DateOfBirth.Value.Date : (DateTime?)null,
                CitizenshipCountryCode = citizenship != null ? citizenship.LocationIso2 : "",
                EmailAddress = personalEmail != null ? personalEmail.Select(x => x.Address).FirstOrDefault() : "",
                FullName = new FullName
                {
                    FirsName = personalPII.FirstName,
                    LastName = personalPII.LastName,
                    Suffix = personalPII.NameSuffix,
                    PreferredName = personalPII.Alias
                },
                Gender = personalPII.GenderId.ToString(),
                PhoneNumber = "",
                PositionCode = "",
                printForm = false,
                Remarks = "Test remark"
            };
            if (mailingAddress != null)
            {
                ExchVisitor.Biographical.MailAddress = new USAddress
                {
                    Address1 = mailingAddress.Street1,
                    Address2 = mailingAddress.Street2,
                    City = mailingAddress.City.LocationName,
                    State = mailingAddress.Division.LocationName,
                    PostalCode = mailingAddress.PostalCode,
                    Explanation = "",
                    ExplanationCode = ""
                };
            }
            if (physicalAddress != null)
            {
                ExchVisitor.Biographical.PermanentResidenceCountryCode = physicalAddress.LocationIso2 != null ? physicalAddress.LocationIso2 : "";
                ExchVisitor.Biographical.USAddress = new USAddress
                {
                    Address1 = physicalAddress.Street1,
                    Address2 = physicalAddress.Street2,
                    City = physicalAddress.City.LocationName,
                    State = physicalAddress.Division.LocationName,
                    PostalCode = physicalAddress.PostalCode,
                    Explanation = "",
                    ExplanationCode = ""
                };
                ExchVisitor.Biographical.ResidentialAddress = new ResidentialAddress
                {
                    BoardingSchool = new BoardingSchool
                    {
                        Name = "",
                        Phone = ""
                    },
                    HostFamily = new HostFamily
                    {
                        PContact = new PContact
                        {
                            FirsName = "",
                            LastName = ""
                        },
                        SContact = new SContact
                        {
                            FirsName = "",
                            LastName = ""
                        },
                        Phone = ""
                    },
                    LCCoordinator = new LCCoordinator
                    {
                        FirsName = "",
                        LastName = ""
                    },
                    ResidentialType = ""
                };
            }
            // financial
            ExchVisitor.FinancialInfo = new FinancialInfoUpdate
            {
                printForm = false,
                ReceivedUSGovtFunds = false,
                ProgramSponsorFunds = "1200",
                OtherFunds = new OtherFunds
                {
                    International = new International
                    {
                        Amount1 = "10",
                        Amount2 = "20",
                        Org1 = "org 1",
                        OtherName1 = "on 1",
                        Org2 = "org 2",
                        OtherName2 = "on 2"
                    },
                    USGovt = new USGovt
                    {
                        Amount1 = "10",
                        Amount2 = "20",
                        Org1 = "govorg 1",
                        OtherName1 = "govon 1",
                        Org2 = "govorg 2",
                        OtherName2 = "govon 2"
                    },
                    Other = new Other
                    {
                        amount = "30",
                        name = "other nm"
                    }
                }
            };
            // program and subject
            ExchVisitor.Program = new Validation.Model.Program
            {
                EditSubject = new SubjectFieldUpdate
                {
                    printForm = false,
                    SubjectFieldCode = "SF code",
                    SubjectFieldRemarks = "SF rmks",
                    ForeignDegreeLevel = "FD lvl",
                    ForeignFieldOfStudy = "F FS",
                    Remarks = "Rmks"
                }
            };
            // site of activity
            ExchVisitor.SiteOfActivity = new SiteOfActivityUpdate
            {
                AddSOA = new SiteOfActivitySOA
                {
                    printForm = false,
                    Address1 = "123 Some St",
                    PostalCode = "12345",
                    SiteName = "site 1",
                    PrimarySite = true
                }
            };
            // Reprint
            ExchVisitor.Reprint = new ReprintFormUpdate
            {
                printForm = false,
                dependentSevisID = "1",
                Reason = "Reprint reason",
                Remarks = "Remarks",
                OtherRemarks = "Other remarks"
            };
            // Reprint 7002
            ExchVisitor.Reprint7002 = new Reprint7002
            {
                print7002 = false,
                SiteId = "1"
            };
            ExchVisitor.requestID = "1";
            ExchVisitor.sevisID = "1";
            ExchVisitor.SiteOfActivity = new SiteOfActivityUpdate
            {

            };
            ExchVisitor.Status = new StatusUpdate
            {
            };
            ExchVisitor.statusCode = "A";
            ExchVisitor.TIPP = new TippUpdate
            {
            };
            ExchVisitor.UserDefinedA = "UD A";
            ExchVisitor.UserDefinedB = "UD B";
            ExchVisitor.Validate = new ValidateParticipant
            {

            };
            ExchVisitor.userID = participant.PersonId.ToString();

            // TODO: complete when dependent feature is available
            //ExchVisitor.Dependent = new UpdatedDependent
            //{
            //    Edit = new EditDependent
            //    {
            //        dependentSevisID = "1",
            //        printForm = false,
            //        BirthDate = new DateTime(1988, 4, 18),
            //        Gender = "1",
            //        BirthCountryCode = "01",
            //        CitizenshipCountryCode = "01",
            //        FullName = new FullName
            //        {
            //            FirsName = "Some",
            //            LastName = "Dependent"
            //        }
            //    }
            //};

            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };
            var updateVisitor = new UpdateExchVisitor
            {
                ExchangeVisitor = ExchVisitor
            };
            var updateVisitorBatch = new SEVISBatchCreateUpdateEV
            {
                userID = "1",
                BatchHeader = batchHeader,
                UpdateEV = updateVisitor,
                CreateEV = null
            };

            return updateVisitorBatch;
        }
        

        #endregion

        #region ParticipantPersonSevisStatus


        /// Sevis Comm Status

        /// <summary>
        /// Returns the participantPersonSevisCommStatus in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonsSevisCommStatuses(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonsSevisCommStatusQueries.CreateGetParticipantPersonsSevisCommStatusDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevisCommStatuses with query operator [{0}].", queryOperator);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns the participantPersonSevisCommStatus in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonsSevisCommStatusesAsync(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonsSevisCommStatusQueries.CreateGetParticipantPersonsSevisCommStatusDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevisCommStatuses with query operator [{0}].", queryOperator);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns a participantPersonSevis
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonsSevisCommStatusesById(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonsSevisCommStatusQueries.CreateGetParticipantPersonsSevisCommStatusDTOByIdQuery(this.Context, participantId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns a participantPersonSevisCommStatus asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonsSevisCommStatusesByIdAsync(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonsSevisCommStatusQueries.CreateGetParticipantPersonsSevisCommStatusDTOByIdQuery(this.Context, participantId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participantIds"></param>
        /// <returns></returns>
        public IQueryable<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonsSevisCommStatusesByParticipantIds(int[] participantIds)
        {
            var results = ParticipantPersonsSevisCommStatusQueries.CreateParticipantPersonsSevisCommStatusesDTOsByParticipantIdsQuery(Context, participantIds);
            logger.Trace("Retrieved participantPersonSevises by array of participant Ids");
            return results;
        }

        /// <summary>
        /// Sets sevis communication status for participant ids
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <returns>List of participant ids that were updated</returns>
        public async Task<int[]> SendToSevis(int[] participantIds)
        {
            var statuses = await Context.ParticipantPersonSevisCommStatuses.GroupBy(x => x.ParticipantId)
                .Select(s => s.OrderByDescending(x => x.AddedOn).FirstOrDefault())
                .Where(w => w.SevisCommStatusId == SevisCommStatus.ReadyToSubmit.Id && participantIds.Contains(w.ParticipantId))
                .ToListAsync();

            var participantsUpdated = new List<int>();

            foreach (var status in statuses)
            {
                var newStatus = new ParticipantPersonSevisCommStatus
                {
                    ParticipantId = status.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                    AddedOn = DateTimeOffset.Now
                };

                Context.ParticipantPersonSevisCommStatuses.Add(newStatus);
                participantsUpdated.Add(status.ParticipantId);
            }

            return participantsUpdated.ToArray();
        }

        #endregion

        #region update

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        public ParticipantPersonSevisDTO Update(UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            var participantPerson = CreateGetParticipantPersonsByIdQuery(updatedParticipantPersonSevis.ParticipantId).FirstOrDefault();
            throwIfModelDoesNotExist(updatedParticipantPersonSevis.ParticipantId, participantPerson, typeof(ParticipantPerson));

            DoUpdate(participantPerson, updatedParticipantPersonSevis);
            return this.GetParticipantPersonsSevisById(updatedParticipantPersonSevis.ParticipantId);
        }

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        /// <returns>The task.</returns>
        public async Task<ParticipantPersonSevisDTO> UpdateAsync(UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            var participantPerson = await CreateGetParticipantPersonsByIdQuery(updatedParticipantPersonSevis.ParticipantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(updatedParticipantPersonSevis.ParticipantId, participantPerson, typeof(ParticipantPerson));

            DoUpdate(participantPerson, updatedParticipantPersonSevis);

            return await this.GetParticipantPersonsSevisByIdAsync(updatedParticipantPersonSevis.ParticipantId);
        }

        private void DoUpdate(ParticipantPerson participantPerson, UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            updatedParticipantPersonSevis.Audit.SetHistory(participantPerson);

            participantPerson.SevisId = updatedParticipantPersonSevis.SevisId;
            participantPerson.IsSentToSevisViaRTI = updatedParticipantPersonSevis.IsSentToSevisViaRTI;
            participantPerson.IsValidatedViaRTI = updatedParticipantPersonSevis.IsValidatedViaRTI;
            participantPerson.IsCancelled = updatedParticipantPersonSevis.IsCancelled;
            participantPerson.IsDS2019Printed = updatedParticipantPersonSevis.IsDS2019Printed;
            participantPerson.IsNeedsUpdate = updatedParticipantPersonSevis.IsNeedsUpdate;
            participantPerson.IsDS2019SentToTraveler = updatedParticipantPersonSevis.IsDS2019SentToTraveler;
            participantPerson.StartDate = updatedParticipantPersonSevis.StartDate;
            participantPerson.EndDate = updatedParticipantPersonSevis.EndDate;
        }

        private UpdatedParticipantPersonSevisValidationEntity GetUpdatedParticipantPersonSevisValidationEntity(ParticipantPerson participantPerson, UpdatedParticipantPersonSevis participantPersonSevis)
        {
            return new UpdatedParticipantPersonSevisValidationEntity(participantPerson, participantPersonSevis);
        }

        private IQueryable<ParticipantPerson> CreateGetParticipantPersonsByIdQuery(int participantId)
        {
            return Context.ParticipantPersons.Where(x => x.ParticipantId == participantId);
        }

        /// <summary>
        /// Update a participant SEVIS pre-validation status
        /// </summary>
        /// <param name="participantId">Participant ID</param>
        /// <param name="errorCount">Count of validation errors</param>
        /// <param name="isValid">Validation status</param>
        public void UpdateParticipantPersonSevisCommStatus(int participantId, int errorCount, bool isValid)
        {
            var newStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participantId,
                AddedOn = DateTimeOffset.Now
            };

            if (errorCount > 0)
            {
                newStatus.SevisCommStatusId = SevisCommStatus.InformationRequired.Id;
            }
            else if (isValid)
            {
                newStatus.SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id;
            }

            Context.ParticipantPersonSevisCommStatuses.Add(newStatus);
        }

        #endregion
    }
}
