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
                             SevisValidationResult = p.SevisValidationResult
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
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(context, participantId).FirstOrDefault();
            var participantPerson = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(context, participantId).FirstOrDefault();
            var personalPII = PersonQueries.CreateGetPiiByIdQuery(context, (int)participant.PersonId).FirstOrDefault();
            var participantExchangeVisitor = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(context, participantId).FirstOrDefault();
            var personalEmail = PersonQueries.CreateGetContactInfoByIdQuery(context, (int)participant.PersonId).Select(x => x.EmailAddresses).FirstOrDefault();
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
            
            var ExchVisitor = new ExchangeVisitor();

            ExchVisitor.requestID = participantId.ToString();
            ExchVisitor.userID = user.Id.ToString();
            ExchVisitor.PositionCode = participantExchangeVisitor.PositionCode;
            ExchVisitor.PrgStartDate = projectProgram.StartDate.DateTime > DateTime.MinValue ? projectProgram.StartDate.DateTime : DateTime.MinValue;
            ExchVisitor.PrgEndDate = projectProgram.EndDate.HasValue ? projectProgram.EndDate.Value.DateTime : DateTime.MinValue;
            ExchVisitor.CategoryCode = participantExchangeVisitor.ProgramCategoryCode;
            ExchVisitor.OccupationCategoryCode = "99"; // unknown

            // biographical
            ExchVisitor.Biographical = new Biographical
            {
                FullName = new FullName
                {
                    FirsName = personalPII.FirstName,
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
            // subject field
            var fos = new FieldOfStudyService(context);
            var defaultSorter = new ExpressionSorter<SimpleSevisLookupDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleSevisLookupDTO>(0, 10, defaultSorter);
            var fosResult = fos.Get(queryOperator).Results.FirstOrDefault();

            ExchVisitor.SubjectField = new SubjectField
            {
                SubjectFieldCode = fosResult.Code,                
                ForeignDegreeLevel = "", // TODO: add field to UI?
                ForeignFieldOfStudy = participantExchangeVisitor.FieldOfStudy,
                Remarks = fosResult.Description // TODO: add field to UI?
            };
            // addresses
            if (physicalAddress != null)
            {
                ExchVisitor.Biographical.PermanentResidenceCountryCode = physicalAddress.LocationIso2 != null ? physicalAddress.LocationIso2 : "";
                ExchVisitor.USAddress = new USAddress
                {
                    Address1 = physicalAddress.Street1,
                    Address2 = physicalAddress.Street2,
                    City = physicalAddress.City.LocationName,
                    State = physicalAddress.Division.LocationName,
                    PostalCode = physicalAddress.PostalCode
                };
                //ExchVisitor.Biographical.ResidentialAddress = new ResidentialAddress
                //{
                //    ResidentialType = physicalAddress.LocationType.LocationTypeName,
                //    HostFamily = new HostFamily
                //    {
                //        PContact = new PContact
                //        {
                //            FirsName = "",
                //            LastName = ""
                //        },
                //        SContact = new SContact
                //        {
                //            FirsName = "",
                //            LastName = ""
                //        },
                //        Phone = ""
                //    },
                //    BoardingSchool = new BoardingSchool
                //    {
                //        Name = "",
                //        Phone = ""
                //    },
                //    LCCoordinator = new LCCoordinator
                //    {
                //        FirsName = "",
                //        LastName = ""
                //    }
                //};
            }
            else
            {
                ExchVisitor.USAddress = null;
                ExchVisitor.Biographical.ResidentialAddress = null;
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
            else
            {
                ExchVisitor.MailAddress = null;
            }
            // financial
            var usfunds = participantExchangeVisitor.FundingGovtAgency1 + participantExchangeVisitor.FundingGovtAgency2;
            ExchVisitor.FinancialInfo = new FinancialInfo
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
            // TODO: complete when dependent feature is available
            ExchVisitor.CreateDependent = null;
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

            // T/IPP
            ExchVisitor.AddTIPP = new AddTIPP
            {
                print7002 = false
            };
            // site of activity
            ExchVisitor.AddSiteOfActivity = new AddSiteOfActivity
            {
                SiteOfActivitySOA = new SiteOfActivitySOA
                {
                    printForm = false,
                    Address1 = "2201 C St NW",
                    City = "Washington",
                    State = "DC",
                    PostalCode = "20520",
                    SiteName = "US Department of State",
                    PrimarySite = true,
                    Remarks = ""
                },
                SiteOfActivityExempt = new SiteOfActivityExempt
                {
                    Remarks = ""
                }
            };

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
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(context, participantId).FirstOrDefault();
            var participantPerson = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(context, participantId).FirstOrDefault();
            var personalPII = PersonQueries.CreateGetPiiByIdQuery(context, (int)participant.PersonId).FirstOrDefault();
            var participantExchangeVisitor = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(context, participantId).FirstOrDefault();
            var personalEmail = PersonQueries.CreateGetContactInfoByIdQuery(context, (int)participant.PersonId).Select(x => x.EmailAddresses).FirstOrDefault();
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

            var ExchVisitor = new ExchangeVisitorUpdate();

            ExchVisitor.requestID = participantId.ToString();
            ExchVisitor.userID = user.Id.ToString();
            ExchVisitor.sevisID = participant.SevisId;
            ExchVisitor.statusCode = participant.StatusId.ToString();

            // biographical
            ExchVisitor.Biographical = new BiographicalUpdate
            {
                printForm = false,
                FullName = new FullName
                {
                    FirsName = personalPII.FirstName,
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
            if (mailingAddress != null)
            {
                ExchVisitor.Biographical.MailAddress = new USAddress
                {
                    Address1 = mailingAddress.Street1 != null ? mailingAddress.Street1 : "",
                    Address2 = mailingAddress.Street2 != null ? mailingAddress.Street2 : "",
                    City = mailingAddress.City != null ? mailingAddress.City.LocationName != null ? mailingAddress.City.LocationName : "" : "",
                    State = mailingAddress.Division != null ? mailingAddress.Division.LocationName != null ? mailingAddress.Division.LocationName : "" : "",
                    PostalCode = mailingAddress.PostalCode != null ? mailingAddress.PostalCode : "",
                    Explanation = "",
                    ExplanationCode = ""
                };
            }
            else
            {
                ExchVisitor.Biographical.MailAddress = null;
            }
            if (physicalAddress != null)
            {
                ExchVisitor.Biographical.PermanentResidenceCountryCode = physicalAddress.LocationIso2 != null ? physicalAddress.LocationIso2 : "";
                ExchVisitor.Biographical.USAddress = new USAddress
                {
                    Address1 = physicalAddress.Street1 != null ? physicalAddress.Street1 : "",
                    Address2 = physicalAddress.Street2 != null ? physicalAddress.Street2 : "",
                    City = physicalAddress.City != null ? physicalAddress.City.LocationName != null ? physicalAddress.City.LocationName : "" : "",
                    State = physicalAddress.Division != null ? physicalAddress.Division.LocationName != null ? physicalAddress.Division.LocationName : "" : "",
                    PostalCode = physicalAddress.PostalCode != null ? physicalAddress.PostalCode : "",
                    Explanation = "",
                    ExplanationCode = ""
                };
                //ExchVisitor.Biographical.ResidentialAddress = new ResidentialAddress
                //{
                //    ResidentialType = "",
                //    BoardingSchool = new BoardingSchool
                //    {
                //        Name = "",
                //        Phone = ""
                //    },
                //    HostFamily = new HostFamily
                //    {
                //        PContact = new PContact
                //        {
                //            FirsName = "",
                //            LastName = ""
                //        },
                //        SContact = new SContact
                //        {
                //            FirsName = "",
                //            LastName = ""
                //        },
                //        Phone = ""
                //    },
                //    LCCoordinator = new LCCoordinator
                //    {
                //        FirsName = "",
                //        LastName = ""
                //    }
                //};
            }
            else
            {
                ExchVisitor.Biographical.USAddress = null;
                ExchVisitor.Biographical.ResidentialAddress = null;
            }
            // financial
            var usfunds = participantExchangeVisitor.FundingGovtAgency1 + participantExchangeVisitor.FundingGovtAgency2;
            ExchVisitor.FinancialInfo = new FinancialInfoUpdate
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
            // program and subject
            var fos = new FieldOfStudyService(context);
            var defaultSorter = new ExpressionSorter<SimpleSevisLookupDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleSevisLookupDTO>(0, 10, defaultSorter);
            var fosResult = fos.Get(queryOperator).Results.FirstOrDefault();

            ExchVisitor.Program = new Validation.Model.Program
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
            // Reprint
            ExchVisitor.Reprint = new ReprintFormUpdate
            {
                printForm = false,
                Reason = "05",
                OtherRemarks = "",
                Remarks = ""
            };
            // site of activity
            ExchVisitor.SiteOfActivity = new SiteOfActivityUpdate
            {
                AddSOA = new SiteOfActivitySOA
                {
                    printForm = false,
                    Address1 = "2201 C St NW",
                    City = "Washington",
                    State = "DC",
                    PostalCode = "20520",
                    SiteName = "US Department of State",
                    PrimarySite = true
                }
            };
            // TIPP
            //ExchVisitor.TIPP = new TippUpdate
            //{
            //    AddTIPP = new AddTIPPUpdate
            //    {
            //        print7002 = false,
            //        ParticipantInfo = new ParticipantInfoUpdate
            //        {
            //            EmailAddress = personalEmail != null ? personalEmail.Select(x => x.Address).FirstOrDefault() : "",
            //            FieldOfStudy = participantPerson != null ? participantPerson.FieldOfStudy : "",
            //            TypeOfDegree = "",
            //            DateAwardedOrExpected = program.EndDate.Value.Date > DateTime.MinValue ? program.EndDate.Value.Date : DateTime.MinValue
            //        },
            //        TippSite = new TippSiteUpdate
            //        {
            //            Address1 = "2201 C St NW",
            //            PostalCode = "20520",
            //            SiteName = "US Department of State",
            //            PrimarySite = true,
            //            EmployerID = "123456789",
            //            FullTimeEmployees = "1",
            //            AnnualRevenue = "0",
            //            WebsiteURL = "",
            //            WorkersCompInd = false,
            //            WorkersCompForEvInd = false,
            //            EvHoursPerWeek = "20",
            //            StipendInd = false,
            //            SupervisorLastName = "",
            //            SupervisorFirstName = "",
            //            SupervisorTitle = "",
            //            SupervisorPhone = "",
            //            SupervisorEmail = "",
            //            TippPhase = new TippPhase
            //            {
            //                PhaseName = "",
            //                StartDate = new DateTime(1998, 2, 10),
            //                EndDate = new DateTime(2002, 4, 20),
            //                TrainingField = "",
            //                SuperLastName = "",
            //                SuperFirstName = "",
            //                SuperTitle = "",
            //                SuperEmail = "",
            //                SuperPhone = "",
            //                EvRole = "",
            //                GoalsAndObjectives = "",
            //                SupervisorAndQualifications = "",
            //                CulturalActivities = "",
            //                SkillsLearned = "",
            //                TeachingMethod = "",
            //                HowCompetencyMeasured = ""
            //            }
            //        }
            //    },
            //    AddSite = new TippSite
            //    {
            //        Address1 = "2201 C St NW",
            //        PostalCode = "20520",
            //        SiteName = "US Department of State",
            //        PrimarySite = true,
            //        EmployerID = "123456789",
            //        FullTimeEmployees = "1",
            //        AnnualRevenue = "0",
            //        WebsiteURL = "",
            //        WorkersCompInd = false,
            //        WorkersCompForEvInd = false,
            //        EvHoursPerWeek = "20",
            //        StipendInd = false,
            //        SupervisorLastName = "",
            //        SupervisorFirstName = "",
            //        SupervisorTitle = "",
            //        SupervisorPhone = "",
            //        SupervisorEmail = "",
            //        TippPhase = new TippPhase
            //        {
            //            PhaseName = "",
            //            StartDate = new DateTime(1998, 2, 10),
            //            EndDate = new DateTime(2002, 4, 20),
            //            TrainingField = "",
            //            SuperLastName = "",
            //            SuperFirstName = "",
            //            SuperTitle = "",
            //            SuperEmail = "",
            //            SuperPhone = "",
            //            EvRole = "",
            //            GoalsAndObjectives = "",
            //            SupervisorAndQualifications = "",
            //            CulturalActivities = "",
            //            SkillsLearned = "",
            //            TeachingMethod = "",
            //            HowCompetencyMeasured = ""
            //        }
            //    },
            //    EditSite = new EditTippSite
            //    {
            //        Supervisors = new SupervisorsUpdate
            //        {
            //            TippPhase = new TippSupervisorPhaseUpdate
            //            {
            //                PhaseId = "123",
            //                SignatureDate = DateTime.Today
            //            }
            //        }
            //    },
            //    DeleteSite = new DeleteTippSite
            //    {
            //        SiteId = "1"
            //    },
            //    AddPhase = new AddPhase
            //    {
            //        SiteId = "1",
            //        PhaseName = "",
            //        StartDate = new DateTime(1998, 2, 10),
            //        EndDate = new DateTime(2002, 4, 20),
            //        TrainingField = "",
            //        SuperLastName = "",
            //        SuperFirstName = "",
            //        SuperTitle = "",
            //        SuperEmail = "",
            //        SuperPhone = "",
            //        EvRole = "",
            //        GoalsAndObjectives = "",
            //        SupervisorAndQualifications = "",
            //        CulturalActivities = "",
            //        SkillsLearned = "",
            //        TeachingMethod = "",
            //        HowCompetencyMeasured = ""
            //    },
            //    EditPhase = new EditPhase
            //    {
            //        PhaseId = "123",
            //        PhaseName = "",
            //        StartDate = new DateTime(1998, 2, 10),
            //        EndDate = new DateTime(2002, 4, 20),
            //        TrainingField = "",
            //        SuperLastName = "",
            //        SuperFirstName = "",
            //        SuperTitle = "",
            //        SuperEmail = "",
            //        SuperPhone = "",
            //        EvRole = "",
            //        GoalsAndObjectives = "",
            //        SupervisorAndQualifications = "",
            //        CulturalActivities = "",
            //        SkillsLearned = "",
            //        TeachingMethod = "",
            //        HowCompetencyMeasured = ""
            //    },
            //    DeletePhase = new DeletePhase
            //    {
            //        PhaseId = "123"
            //    },
            //    UpdateSignatureDates = new UpdateSignatureDates
            //    {
            //        TippSite = new TippSiteUpdate
            //        {
            //            SiteId = "1",
            //            ProgramOfficial = new ProgramOfficial
            //            {
            //                UserName = "",
            //                SignatureDate = DateTime.Today
            //            },
            //            EvSignatureDate = DateTime.Today,
            //            Supervisors = new TippSupervisorsUpdate
            //            {
            //                TippPhase = new TippSupervisorPhaseUpdate
            //                {
            //                    PhaseId = "123",
            //                    SignatureDate = DateTime.Today
            //                }
            //            }
            //        },
            //        UpdateParticipantInfo = new ParticipantInfoUpdate
            //        {
            //            EmailAddress = "",
            //            FieldOfStudy = "",
            //            TypeOfDegree = "",
            //            DateAwardedOrExpected = new DateTime(2002, 4, 20)
            //        }
            //    }
            //};
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

            var updateVisitor = new UpdateExchVisitor
            {
                ExchangeVisitor = ExchVisitor
            };

            return updateVisitor;
        }
        
        /// <summary>
        /// Creates a query to return all participantPersonSevises in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersonSevises.</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOQuery(EcaContext context, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantPersonsSevisDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantPersonSevises for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersonSevises.</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantPersonsSevisDTOQuery(context).Where(x => x.ProjectId == projectId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participantPersonSevis by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOByIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetParticipantPersonsSevisDTOQuery(context).
                Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
