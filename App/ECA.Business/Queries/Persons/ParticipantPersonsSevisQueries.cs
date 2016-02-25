using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service;
using ECA.Business.Service.Lookup;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model.Shared;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ParticipantQueries are used to query a DbContext for Participant information.
    /// </summary>
    public static class ParticipantPersonsSevisQueries
    {
        /// <summary>
        /// Query to get a list of participant people with SEVIS information
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of participant people with SEVIS</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from p in context.ParticipantPersons
                         select new ParticipantPersonSevisDTO
                         {
                             ParticipantId = p.ParticipantId,
                             SevisId = p.SevisId,
                             ProjectId = p.Participant.ProjectId,
                             ParticipantType = p.Participant.ParticipantType != null ? p.Participant.ParticipantType.Name : null,
                             ParticipantStatus = p.Participant.Status != null ? p.Participant.Status.Status : null,
                             IsCancelled = p.IsCancelled,
                             IsDS2019Printed = p.IsDS2019Printed,
                             IsDS2019SentToTraveler = p.IsDS2019SentToTraveler,
                             IsNeedsUpdate = p.IsNeedsUpdate,
                             IsSentToSevisViaRTI = p.IsSentToSevisViaRTI,
                             IsValidatedViaRTI = p.IsValidatedViaRTI,
                             StartDate = p.StartDate,
                             EndDate = p.EndDate,
                             SevisCommStatuses = p.ParticipantPersonSevisCommStatuses.Select(s => new ParticipantPersonSevisCommStatusDTO()
                             {
                                 Id = s.Id, ParticipantId = s.ParticipantId, SevisCommStatusId = s.SevisCommStatusId,
                                 SevisCommStatusName = s.SevisCommStatus.SevisCommStatusName, AddedOn = s.AddedOn
                             }).OrderBy(s => s.AddedOn),
                             LastBatchDate =  p.ParticipantPersonSevisCommStatuses.Max(s => s.AddedOn),
                             SevisValidationResult = p.SevisValidationResult,
                             SevisBatchResult = p.SevisBatchResult
                         });
            return query;
        }
        
        /// <summary>
        /// Get populated CREATE participant sevis object
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="user">The current user</param>
        /// <param name="context">The context to query</param>
        /// <returns>Exchange visitor create object</returns>
        public static CreateExchVisitor GetCreateExchangeVisitor(int participantId, User user, EcaContext context)
        {
            //Get student details
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(user != null, "The user must not be null.");
            var participant = GetParticipant(context, participantId);
            var participantPerson = GetParticipantPerson(context, participant.ProjectId, participantId);
            var personalPII = GetPersonPii(context, (int)participant.PersonId);
            var participantExchangeVisitor = GetParticipantExchangeVisitor(context, participant.ProjectId, participantId);
            var personalEmail = GetPersonalEmail(context, participantId);
            var mailingAddress = new Location();
            var physicalAddress = new Location();
            if (participantPerson != null)
            {
                mailingAddress = context.Locations.Where(x => x.LocationId == participantPerson.HomeInstitutionAddressId).FirstOrDefault();
                physicalAddress = context.Locations.Where(x => x.LocationId == participantPerson.HostInstitutionAddressId).FirstOrDefault();
            }
            var locid = personalPII.CountriesOfCitizenship.Select(c => c.Id).FirstOrDefault();
            var citizenship = context.Locations.Where(x => x.LocationId == locid).FirstOrDefault();
            var projectProgram = context.Projects.Where(x => x.ProjectId == participant.ProjectId).FirstOrDefault();

            var ExchVisitor = new ExchangeVisitor
            {
                requestID = participantId.ToString(),
                userID = user.Id.ToString(),
                PositionCode = participantExchangeVisitor.PositionCode,
                PrgStartDate = projectProgram.StartDate.DateTime > DateTime.MinValue ? projectProgram.StartDate.DateTime : DateTime.MinValue,
                PrgEndDate = projectProgram.EndDate.HasValue ? projectProgram.EndDate.Value.DateTime : DateTime.MinValue,
                CategoryCode = participantExchangeVisitor.ProgramCategoryCode,
                OccupationCategoryCode = "99" // unknown
            };
            
            // biographical
            ExchVisitor.Biographical = GetBiographical(personalPII, citizenship, personalEmail);

            // subject field
            ExchVisitor.SubjectField = GetSubjectField(context, participantExchangeVisitor);

            // addresses
            if (physicalAddress != null)
            {
                ExchVisitor.Biographical.PermanentResidenceCountryCode = physicalAddress.LocationIso2 != null ? physicalAddress.LocationIso2 : "";
                ExchVisitor.USAddress = GetUSAddress(physicalAddress);
                //ExchVisitor.Biographical.ResidentialAddress = GetResidentialAddress(physicalAddress);
            }
            else
            {
                ExchVisitor.USAddress = null;
                ExchVisitor.Biographical.ResidentialAddress = null;
            }
            if (mailingAddress != null)
            {
                ExchVisitor.MailAddress = GetUSAddress(mailingAddress);
            }
            else
            {
                ExchVisitor.MailAddress = null;
            }
            // financial
            ExchVisitor.FinancialInfo = GetFinancialInfo(participantExchangeVisitor);

            // dependents
            ExchVisitor.CreateDependent = null;
            //ExchVisitor.CreateDependent = GetDependent();

            // site of activity
            ExchVisitor.AddSiteOfActivity = GetAddSiteOfActivity();

            // attach exchange visitor record
            var createVisitor = new CreateExchVisitor
            {
                ExchangeVisitor = ExchVisitor
            };

            return createVisitor;
        }
        
        /// <summary>
        /// Get populated UPDATE participant sevis object
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="user">The current user</param>
        /// <param name="context">The context to query</param>
        /// <returns>Exchange visitor update object</returns>
        public static UpdateExchVisitor GetUpdateExchangeVisitor(int participantId, User user, EcaContext context)
        {
            //Get student details
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(user != null, "The user must not be null.");
            var participant = GetParticipant(context, participantId);
            var participantPerson = GetParticipantPerson(context, participant.ProjectId, participantId);
            var personalPII = GetPersonPii(context, (int)participant.PersonId);
            var participantExchangeVisitor = GetParticipantExchangeVisitor(context, participant.ProjectId, participantId);
            var personalEmail = GetPersonalEmail(context, participantId);
            var mailingAddress = new Location();
            var physicalAddress = new Location();
            if (participantPerson != null)
            {
                mailingAddress = context.Locations.Where(x => x.LocationId == participantPerson.HomeInstitutionAddressId).FirstOrDefault();
                physicalAddress = context.Locations.Where(x => x.LocationId == participantPerson.HostInstitutionAddressId).FirstOrDefault();
            }
            var locid = personalPII.CountriesOfCitizenship.Select(c => c.Id).FirstOrDefault();
            var citizenship = context.Locations.Where(x => x.LocationId == locid).FirstOrDefault();
            var project = context.Projects.Where(x => x.ProjectId == participant.ProjectId).FirstOrDefault();
            var program = context.Programs.Where(x => x.ProgramId == project.ProgramId).FirstOrDefault();

            var ExchVisitor = new ExchangeVisitorUpdate
            {
                requestID = participantId.ToString(),
                userID = user.Id.ToString(),
                sevisID = participantPerson.SevisId,
                statusCode = participant.StatusId.ToString()
            };

            // biographical
            ExchVisitor.Biographical = GetBiographicalUpdate(personalPII, citizenship, personalEmail, participantExchangeVisitor);

            //if (mailingAddress != null)
            //{
            //    ExchVisitor.Biographical.MailAddress = GetUSAddress(mailingAddress);
            //}
            //else
            //{
            //    ExchVisitor.Biographical.MailAddress = null;
            //}
            //if (physicalAddress != null)
            //{
            //    ExchVisitor.Biographical.PermanentResidenceCountryCode = physicalAddress.LocationIso2 != null ? physicalAddress.LocationIso2 : "";
            //    ExchVisitor.Biographical.USAddress = GetUSAddress(physicalAddress);
            //    ExchVisitor.Biographical.ResidentialAddress = GetResidentialAddress(physicalAddress);
            //}
            //else
            //{
            //    ExchVisitor.Biographical.USAddress = null;
            //    ExchVisitor.Biographical.ResidentialAddress = null;
            //}

            // financial
            ExchVisitor.FinancialInfo = GetFinancialInfoUpdate(participantExchangeVisitor);

            // program and subject
            ExchVisitor.Program = GetProgram(context, participantExchangeVisitor);

            // Reprint
            ExchVisitor.Reprint = GetReprintForm();

            // site of activity
            ExchVisitor.SiteOfActivity = GetSiteOfActivityUpdate();
            
            // Status
            //ExchVisitor.Status = new StatusUpdate
            //{
            //};

            // Validate
            //ExchVisitor.Validate = new ValidateParticipant
            //{
            //};

            // Reprint 7002
            ExchVisitor.Reprint7002 = new Reprint7002
            {
                print7002 = false
            };

            // TODO: complete when dependent feature is available
            ExchVisitor.Dependent = null;
            //ExchVisitor.Dependent = GetDependentUpdate();

            var updateVisitor = new UpdateExchVisitor
            {
                ExchangeVisitor = ExchVisitor
            };

            return updateVisitor;
        }

        private static IEnumerable<EmailAddressDTO> GetPersonalEmail(EcaContext context, int personId)
        {
            var email = PersonQueries.CreateGetContactInfoByIdQuery(context, personId).Select(x => x.EmailAddresses).FirstOrDefault();
            return email;
        }

        private static ParticipantExchangeVisitorDTO GetParticipantExchangeVisitor(EcaContext context, int projectId, int participantId)
        {
            var visitor = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(context, projectId, participantId).FirstOrDefault();
            return visitor;
        }

        private static PiiDTO GetPersonPii(EcaContext context, int personId)
        {
            var pii = PersonQueries.CreateGetPiiByIdQuery(context, personId).FirstOrDefault();
            return pii;
        }

        private static SimpleParticipantPersonDTO GetParticipantPerson(EcaContext context, int projectId, int participantId)
        {
            var pperson = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(context, projectId, participantId).FirstOrDefault();
            return pperson;
        }

        private static ParticipantDTO GetParticipant(EcaContext context, int participantId)
        {
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(context, participantId).FirstOrDefault();
            return participant;
        }

        private static Biographical GetBiographical(PiiDTO personalPII, Location citizenship, IEnumerable<EmailAddressDTO> personalEmail)
        {
            var biographical = new Biographical
            {
                FullName = new FullName
                {
                    FirstName = personalPII.FirstName,
                    LastName = personalPII.LastName,
                    Suffix = personalPII.NameSuffix,
                    PreferredName = personalPII.Alias
                },
                BirthDate = personalPII.DateOfBirth > DateTime.MinValue ? personalPII.DateOfBirth.Value.Date : DateTime.MinValue,
                Gender = personalPII.GenderId.ToString(),
                BirthCity = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.City : "",
                BirthCountryCode = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.CountryIso2 : "",
                CitizenshipCountryCode = citizenship != null ? citizenship.LocationIso2 : "",
                BirthCountryReason = "",
                EmailAddress = personalEmail != null ? personalEmail.Select(x => x.Address).FirstOrDefault() : ""
            };

            return biographical;
        }

        private static BiographicalUpdate GetBiographicalUpdate(PiiDTO personalPII, Location citizenship, 
            IEnumerable<EmailAddressDTO> personalEmail, ParticipantExchangeVisitorDTO participantExchangeVisitor)
        {
            var bio = new BiographicalUpdate
            {
                printForm = false,
                FullName = new FullName
                {
                    FirstName = personalPII.FirstName,
                    LastName = personalPII.LastName,
                    Suffix = personalPII.NameSuffix,
                    PreferredName = personalPII.Alias
                },
                BirthDate = personalPII.DateOfBirth > DateTime.MinValue ? personalPII.DateOfBirth.Value.Date : DateTime.MinValue,
                Gender = personalPII.GenderId.ToString(),
                BirthCity = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.City : "",
                BirthCountryCode = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.CountryIso2 : "",
                CitizenshipCountryCode = citizenship != null ? citizenship.LocationIso2 : "",
                BirthCountryReason = "",
                EmailAddress = personalEmail != null ? personalEmail.Select(x => x.Address).FirstOrDefault() : "",
                PhoneNumber = "",
                PositionCode = participantExchangeVisitor.PositionCode,
                Remarks = ""
            };

            return bio;
        }

        private static SubjectField GetSubjectField(EcaContext context, ParticipantExchangeVisitorDTO participantExchangeVisitor)
        {
            var fos = new FieldOfStudyService(context);
            var defaultSorter = new ExpressionSorter<SimpleSevisLookupDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleSevisLookupDTO>(0, 10, defaultSorter);
            var fosResult = fos.Get(queryOperator).Results.FirstOrDefault();

            var subjectfield = new SubjectField
            {
                SubjectFieldCode = fosResult.Code,
                ForeignDegreeLevel = "", // TODO: add field to UI?
                ForeignFieldOfStudy = participantExchangeVisitor.FieldOfStudy,
                Remarks = fosResult.Description // TODO: add field to UI?
            };

            return subjectfield;
        }

        private static USAddress GetUSAddress(Location physicalAddress)
        {
            var address = new USAddress
            {
                Address1 = physicalAddress.Street1,
                Address2 = physicalAddress.Street2,
                City = physicalAddress.City != null ? physicalAddress.City.LocationName : "",
                State = physicalAddress.Division != null ? physicalAddress.Division.LocationName : "",
                PostalCode = physicalAddress.PostalCode,
                Explanation = "",
                ExplanationCode = ""
            };

            return address;
        }

        private static ResidentialAddress GetResidentialAddress(Location physicalAddress)
        {
            var address = new ResidentialAddress
            {
                ResidentialType = physicalAddress.LocationType.LocationTypeName,
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
                BoardingSchool = new BoardingSchool
                {
                    Name = "",
                    Phone = ""
                },
                LCCoordinator = new LCCoordinator
                {
                    FirsName = "",
                    LastName = ""
                }
            };

            return address;
        }

        private static FinancialInfo GetFinancialInfo(ParticipantExchangeVisitorDTO participantExchangeVisitor)
        {
            var usfunds = participantExchangeVisitor.FundingGovtAgency1 + participantExchangeVisitor.FundingGovtAgency2;

            var financial = new FinancialInfo
            {
                ReceivedUSGovtFunds = usfunds > 0,
                ProgramSponsorFunds = participantExchangeVisitor.FundingSponsor.ToString(),
                OtherFunds = new OtherFunds
                {
                    International = new International
                    {
                        Amount1 = participantExchangeVisitor.FundingIntlOrg1.ToString(),
                        Amount2 = participantExchangeVisitor.FundingIntlOrg2.ToString(),
                        Org1 = participantExchangeVisitor.IntlOrg1Id.ToString(),
                        OtherName1 = participantExchangeVisitor.IntlOrg1Name,
                        Org2 = participantExchangeVisitor.IntlOrg2Id.ToString(),
                        OtherName2 = participantExchangeVisitor.IntlOrg2Name
                    },
                    EVGovt = participantExchangeVisitor.FundingVisGovt.ToString(),
                    BinationalCommission = participantExchangeVisitor.FundingVisBNC.ToString(),
                    USGovt = new USGovt
                    {
                        Amount1 = participantExchangeVisitor.FundingGovtAgency1.ToString(),
                        Org1 = participantExchangeVisitor.GovtAgency1Name,
                        OtherName1 = participantExchangeVisitor.GovtAgency1OtherName,
                        Amount2 = participantExchangeVisitor.FundingGovtAgency2.ToString(),
                        Org2 = participantExchangeVisitor.GovtAgency2Name,
                        OtherName2 = participantExchangeVisitor.GovtAgency2OtherName
                    },
                    Other = new Other
                    {
                        amount = participantExchangeVisitor.FundingOther.ToString(),
                        name = participantExchangeVisitor.OtherName
                    },
                    Personal = participantExchangeVisitor.FundingPersonal.ToString()
                }
            };

            return financial;
        }

        private static FinancialInfoUpdate GetFinancialInfoUpdate(ParticipantExchangeVisitorDTO participantExchangeVisitor)
        {
            var usfunds = participantExchangeVisitor.FundingGovtAgency1 + participantExchangeVisitor.FundingGovtAgency2;

            var financial = new FinancialInfoUpdate
            {
                printForm = false,
                ReceivedUSGovtFunds = usfunds != null ? usfunds > 0 : false,
                ProgramSponsorFunds = participantExchangeVisitor.FundingSponsor.ToString(),
                OtherFunds = new OtherFunds
                {
                    International = new International
                    {
                        Amount1 = participantExchangeVisitor.FundingIntlOrg1.ToString(),
                        Amount2 = participantExchangeVisitor.FundingIntlOrg2.ToString(),
                        Org1 = participantExchangeVisitor.IntlOrg1Id.ToString(),
                        OtherName1 = participantExchangeVisitor.IntlOrg1Name,
                        Org2 = participantExchangeVisitor.IntlOrg2Id.ToString(),
                        OtherName2 = participantExchangeVisitor.IntlOrg2Name
                    },
                    EVGovt = participantExchangeVisitor.FundingVisGovt.ToString(),
                    BinationalCommission = participantExchangeVisitor.FundingVisBNC.ToString(),
                    USGovt = new USGovt
                    {
                        Amount1 = participantExchangeVisitor.FundingGovtAgency1.ToString(),
                        Org1 = participantExchangeVisitor.GovtAgency1Name,
                        OtherName1 = participantExchangeVisitor.GovtAgency1OtherName,
                        Amount2 = participantExchangeVisitor.FundingGovtAgency2.ToString(),
                        Org2 = participantExchangeVisitor.GovtAgency2Name,
                        OtherName2 = participantExchangeVisitor.GovtAgency2OtherName
                    },
                    Other = new Other
                    {
                        amount = participantExchangeVisitor.FundingOther.ToString(),
                        name = participantExchangeVisitor.OtherName
                    },
                    Personal = participantExchangeVisitor.FundingPersonal.ToString()
                }
            };

            return financial;
        }

        private static Validation.Model.Program GetProgram(EcaContext context, ParticipantExchangeVisitorDTO participantExchangeVisitor)
        {
            var fos = new FieldOfStudyService(context);
            var defaultSorter = new ExpressionSorter<SimpleSevisLookupDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleSevisLookupDTO>(0, 10, defaultSorter);
            var fosResult = fos.Get(queryOperator).Results.FirstOrDefault();

            var program = new Validation.Model.Program
            {
                EditSubject = new SubjectFieldUpdate
                {
                    printForm = false,
                    SubjectFieldCode = fosResult.Code,
                    ForeignDegreeLevel = "", // TODO: add field to UI?
                    ForeignFieldOfStudy = participantExchangeVisitor.FieldOfStudy,
                    Remarks = fosResult.Description // TODO: add field to UI?
                }
            };

            return program;
        }

        private static CreateDependent GetDependent()
        {
            var dependent = new CreateDependent
            {
                Dependent = new AddDependent
                {
                    BirthDate = new DateTime(1988, 4, 18),
                    Gender = "1",
                    BirthCountryCode = "01",
                    CitizenshipCountryCode = "01",
                    FullName = new FullName
                    {
                        FirstName = "Some",
                        LastName = "Dependent"
                    }
                }
            };

            return dependent;
        }

        private static UpdatedDependent GetDependentUpdate()
        {
            var dependent = new UpdatedDependent
            {
                Edit = new EditDependent
                {
                    dependentSevisID = "1",
                    printForm = false,
                    BirthDate = new DateTime(1988, 4, 18),
                    Gender = "1",
                    BirthCountryCode = "01",
                    CitizenshipCountryCode = "01",
                    FullName = new FullName
                    {
                        FirstName = "Some",
                        LastName = "Dependent"
                    }
                }
            };

            return dependent;
        }

        private static AddSiteOfActivity GetAddSiteOfActivity()
        {
            var soa = new AddSiteOfActivity
            {
                SiteOfActivitySOA = GetSiteOfActivity(),
                SiteOfActivityExempt = new SiteOfActivityExempt
                {
                    Remarks = ""
                }
            };

            return soa;
        }

        private static SiteOfActivityUpdate GetSiteOfActivityUpdate()
        {
            var soa = new SiteOfActivityUpdate
            {
                AddSOA = GetSiteOfActivity()
            };

            return soa;
        }

        private static SiteOfActivitySOA GetSiteOfActivity()
        {
            var soa = new SiteOfActivitySOA
            {
                printForm = false,
                Address1 = "2200 C Street, NW",
                City = "Washington",
                State = "DC",
                PostalCode = "20522",
                SiteName = "US Department of State",
                PrimarySite = true,
                Remarks = ""
            };

            return soa;
        }

        private static ReprintFormUpdate GetReprintForm()
        {
            var reprint = new ReprintFormUpdate
            {
                printForm = false,
                Reason = "05",
                OtherRemarks = "",
                Remarks = ""
            };

            return reprint;
        }

        /// <summary>
        /// Returns the participantPersonSevis by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPersonSevis</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOByIdQuery(EcaContext context, int projectId, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetParticipantPersonsSevisDTOQuery(context)
                .Where(p => p.ProjectId == projectId)
                .Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
