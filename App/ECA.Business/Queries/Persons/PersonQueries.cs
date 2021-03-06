﻿using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// Provides linq queries for person
    /// </summary>
    public class PersonQueries
    {
        /// <summary>
        /// The current status value to show for a person who is not a participant in any projects.
        /// </summary>
        public const string UNKNOWN_PARTICIPANT_STATUS = "Unknown";

        /// <summary>
        /// Creates a query to get the simple person dto's participating family member.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="dependentId">The dependent id.</param>
        /// <returns>The simple person dto query of the participating family member.</returns>
        public static IQueryable<SimplePersonDTO> CreateGetRelatedPersonByDependentFamilyMemberQuery(EcaContext context, int dependentId)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var personDTOQuery = CreateGetSimplePersonDTOsQuery(context);

            var query = from dependent in context.PersonDependents
                        let participantFamilyMember = dependent.Person
                        let participantFamilyMemberDTO = participantFamilyMember != null ? personDTOQuery.Where(x => x.PersonId == participantFamilyMember.PersonId).FirstOrDefault() : null
                        where dependent.DependentId == dependentId
                        select participantFamilyMemberDTO;
            return query;
        }

        /// <summary>
        /// Returns a query capable of retrieving people from the given context.  A FullName value is also calculated for the person.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve people from the context.</returns>
        public static IQueryable<SimplePersonDTO> CreateGetSimplePersonDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from person in context.People
                        let gender = person.Gender
                        let currentParticipation = person.Participations.OrderByDescending(p => p.ParticipantStatusId).FirstOrDefault() // the ID order has default precidence, for example if there are two statues, Active(2) and Alumnus(1), Active is shown.
                        let hasCurrentParticipation = currentParticipation != null
                            && currentParticipation.Status != null
                            && currentParticipation.Status.Status != null

                        let hasPlaceOfBirth = person.PlaceOfBirth != null
                        let cityOfBirth = person.PlaceOfBirth

                        let cityOfBirthName = hasPlaceOfBirth ? cityOfBirth.LocationName : null

                        let hasCountryOfBirth = hasPlaceOfBirth && cityOfBirth.Country != null
                        let countryOfBirthName = hasCountryOfBirth ? cityOfBirth.Country.LocationName : null

                        let hasDivisionOfBirth = hasPlaceOfBirth && cityOfBirth.Division != null
                        let divisionOfBirthName = hasDivisionOfBirth ? cityOfBirth.Division.LocationName : null

                        select new SimplePersonDTO
                        {
                            Alias = person.Alias,
                            DateOfBirth = person.DateOfBirth,
                            IsDateOfBirthEstimated = person.IsDateOfBirthEstimated,
                            IsDateOfBirthUnknown = person.IsDateOfBirthUnknown,
                            IsPlaceOfBirthUnknown = person.IsPlaceOfBirthUnknown,
                            IsSingleName = person.IsSingleName,
                            FamilyName = person.FamilyName,
                            FirstName = person.FirstName,
                            Gender = gender.GenderName,
                            GenderId = gender.GenderId,
                            GivenName = person.GivenName,
                            PersonId = person.PersonId,
                            LastName = person.LastName,
                            MiddleName = person.MiddleName,
                            NamePrefix = person.NamePrefix,
                            NameSuffix = person.NameSuffix,
                            Patronym = person.Patronym,
                            FullName = person.FullName,
                            PassportName = person.PassportName,
                            CurrentStatus = hasCurrentParticipation ? currentParticipation.Status.Status : UNKNOWN_PARTICIPANT_STATUS,
                            ProjectId = hasCurrentParticipation ? currentParticipation.ProjectId : default(int?),
                            ParticipantId = hasCurrentParticipation ? currentParticipation.ParticipantId : default(int?),
                            CountryOfBirth = countryOfBirthName,
                            DivisionOfBirth = divisionOfBirthName,
                            CityOfBirth = cityOfBirthName,
                            CityOfBirthId = hasPlaceOfBirth ? cityOfBirth.LocationId : default(int?),
                            ProminentCategories = person.ProminentCategories.Select(x => x.Name)
                        };
            return query;
        }
        
        public static IQueryable<SimplePersonDTO> CreateGetSimplePersonDTOByIdQuery(EcaContext context, int personId)
        {
            return CreateGetSimplePersonDTOsQuery(context).Where(x => x.PersonId == personId);
        }

        /// <summary>
        /// Returns a query capable of retrieving dependents from the given context.
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>The query to retrieve dependents from the context</returns>
        public static IQueryable<SimplePersonDependentDTO> CreateGetSimplePersonDependentDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var locationsQuery = LocationQueries.CreateGetLocationsQuery(context);
            var dependentTypesQuery = CreateGetDependentTypesQuery(context);
            var birthCountryReasonQuery = CreateGetBirthCountryReasonsQuery(context);

            var query = from dependent in context.PersonDependents

                        let locationOfBirth = locationsQuery.Where(x => x.Id == dependent.PlaceOfBirthId).FirstOrDefault()
                        let dependentType = dependentTypesQuery.Where(x => x.Id == dependent.DependentTypeId).FirstOrDefault()
                        let birthCountryReason = birthCountryReasonQuery.Where(x => x.Id == dependent.BirthCountryReasonId).FirstOrDefault()
                        let permanentResidence = locationsQuery.Where(x => x.Id == dependent.PlaceOfResidenceId).FirstOrDefault()

                        let PermanentResidence = (from address in context.Addresses
                                                  let addressType = address.AddressType
                                                  let location = address.Location
                                                  let hasCity = location.City != null
                                                  let city = location.City
                                                  let hasCountry = location.Country != null
                                                  let country = location.Country
                                                  let hasDivision = location.Division != null
                                                  let division = location.Division
                                                  where address.LocationId == dependent.PlaceOfResidenceId
                                                  select new AddressDTO
                                                  {
                                                      AddressId = address.AddressId,
                                                      AddressType = addressType.AddressName,
                                                      AddressTypeId = addressType.AddressTypeId,
                                                      City = hasCity ? city.LocationName : null,
                                                      CityId = location.CityId,
                                                      Country = hasCountry ? country.LocationName : null,
                                                      CountryId = location.CountryId,
                                                      CountryIso2 = location.LocationIso2,
                                                      Division = hasDivision ? division.LocationName : null,
                                                      DivisionId = location.DivisionId,
                                                      IsPrimary = address.IsPrimary,
                                                      LocationId = location.LocationId,
                                                      LocationName = location.LocationName,
                                                      OrganizationId = address.OrganizationId,
                                                      PersonId = address.PersonId,
                                                  }).FirstOrDefault()
                        where dependent.IsDeleted == false
                        select new SimplePersonDependentDTO
                        {
                            DependentId = dependent.DependentId,
                            PersonId = dependent.PersonId,
                            SevisId = dependent.SevisId,
                            DependentTypeId = dependent.DependentTypeId,
                            DependentType = dependent.DependentType.Name,
                            IsSingleName = dependent.IsSingleName,
                            FirstName = dependent.FirstName,
                            LastName = dependent.LastName,
                            NameSuffix = dependent.NameSuffix,
                            PassportName = dependent.PassportName,
                            PreferredName = dependent.PreferredName,
                            GenderId = dependent.GenderId,
                            Gender = dependent.Gender.GenderName,
                            EmailAddress = dependent.EmailAddresses.Select(x => x.Address).FirstOrDefault(),
                            DateOfBirth = dependent.DateOfBirth,
                            PlaceOfBirthId = dependent.PlaceOfBirthId,
                            PlaceOfBirth = locationOfBirth,
                            CountriesOfCitizenship = dependent.CountriesOfCitizenship.Select(x => new CitizenCountryDTO { LocationId = x.LocationId, LocationName = x.Location.LocationName, IsPrimary = x.IsPrimary }),
                            PlaceOfResidenceId = dependent.PlaceOfResidenceId,
                            PlaceOfResidence = permanentResidence,
                            BirthCountryReasonId = dependent.BirthCountryReasonId,
                            BirthCountryReason = birthCountryReason,
                            IsTravellingWithParticipant = dependent.IsTravellingWithParticipant,
                            IsDeleted = dependent.IsDeleted,
                            IsSevisDeleted = dependent.IsSevisDeleted,
                            HasDS2019 = dependent.DS2019FileName != null
                        };
            return query;
        }

        /// <summary>
        /// Returns a query capable of retrieving filtered and sorted simple person dtos in the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to return filtered and sorted simple person dtos.</returns>
        public static IQueryable<SimplePersonDTO> CreateGetSimplePersonDTOsQuery(EcaContext context, QueryableOperator<SimplePersonDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetSimplePersonDTOsQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Get pii by id
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        public static IQueryable<PiiDTO> CreateGetPiiByIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var locationsQuery = LocationQueries.CreateGetLocationsQuery(context);

            var query = from person in context.People
                        let currentParticipant = person.Participations.OrderByDescending(p => p.ParticipantStatusId).FirstOrDefault()
                        let hasPlaceOfBirth = person.PlaceOfBirthId.HasValue
                        let cityOfBirth = hasPlaceOfBirth ? person.PlaceOfBirth : null
                        let locationOfBirth = hasPlaceOfBirth ? locationsQuery.Where(x => x.Id == person.PlaceOfBirthId).FirstOrDefault() : null

                        where person.PersonId == personId

                        select new PiiDTO
                        {
                            Gender = person.Gender.GenderName,
                            GenderId = person.GenderId,
                            DateOfBirth = person.DateOfBirth,
                            IsDateOfBirthUnknown = person.IsDateOfBirthUnknown,
                            IsDateOfBirthEstimated = person.IsDateOfBirthEstimated,
                            CountriesOfCitizenship = person.CountriesOfCitizenship.Select(x => new SimpleLookupDTO { Id = x.LocationId, Value = x.LocationName }).OrderBy(l => l.Value),
                            Dependents = person.Family.Where(x => x.IsDeleted == false).Select(x => new SimpleLookupDTO { Id = x.DependentId, Value = x.LastName + ", " + x.FirstName + " (" + x.DependentType.Name + ")" }),
                            IsSingleName = person.IsSingleName,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            NamePrefix = person.NamePrefix,
                            NameSuffix = person.NameSuffix,
                            GivenName = person.GivenName,
                            FamilyName = person.FamilyName,
                            MiddleName = person.MiddleName,
                            PassportName = person.PassportName,
                            Patronym = person.Patronym,
                            Alias = person.Alias,
                            MaritalStatus = person.MaritalStatus.Description,
                            MaritalStatusId = person.MaritalStatus.MaritalStatusId,
                            Ethnicity = person.Ethnicity,
                            MedicalConditions = person.MedicalConditions,
                            Addresses = (from address in person.Addresses
                                         let addressType = address.AddressType

                                         let location = address.Location

                                         let hasCity = location.City != null
                                         let city = location.City

                                         let hasCountry = location.Country != null
                                         let country = location.Country

                                         let hasDivision = location.Division != null
                                         let division = location.Division

                                         select new AddressDTO
                                         {
                                             AddressId = address.AddressId,
                                             AddressType = addressType.AddressName,
                                             AddressTypeId = addressType.AddressTypeId,
                                             City = hasCity ? city.LocationName : null,
                                             CityId = location.CityId,
                                             Country = hasCountry ? country.LocationName : null,
                                             CountryId = location.CountryId,
                                             CountryIso2 = location.LocationIso2,
                                             Division = hasDivision ? division.LocationName : null,
                                             DivisionId = location.DivisionId,
                                             IsPrimary = address.IsPrimary,
                                             LocationId = location.LocationId,
                                             LocationName = location.LocationName,
                                             OrganizationId = address.OrganizationId,
                                             PostalCode = location.PostalCode,
                                             PersonId = address.PersonId,
                                             Street1 = location.Street1,
                                             Street2 = location.Street2,
                                             Street3 = location.Street3,
                                         }).OrderByDescending(a => a.IsPrimary).ThenBy(a => a.AddressType),
                            IsPlaceOfBirthUnknown = person.IsPlaceOfBirthUnknown,
                            PlaceOfBirth = hasPlaceOfBirth ? locationOfBirth : null,
                            ParticipantId = currentParticipant == null ? 0 : currentParticipant.ParticipantId,
                            ProjectId = currentParticipant == null ? 0 : currentParticipant.ProjectId,
                            SevisId = currentParticipant == null ? "" : currentParticipant.ParticipantPerson.SevisId
                        };
            return query;
        }

        /// <summary>
        /// Returns the contact information for a person 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>The contact information for a person</returns>
        public static IQueryable<ContactInfoDTO> CreateGetContactInfoByIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var emailAddressQuery = EmailAddressQueries.CreateGetEmailAddressDTOQuery(context);
            var phoneNumberQuery = PhoneNumberQueries.CreateGetPhoneNumberDTOQuery(context);

            var query = from person in context.People
                        let participantPerson = context.ParticipantPersons.Where(x => x.Participant.PersonId == person.PersonId).FirstOrDefault()
                        let currentParticipant = person.Participations.OrderByDescending(p => p.ParticipantStatusId).FirstOrDefault()
                        where person.PersonId == personId
                        select new ContactInfoDTO
                        {
                            EmailAddresses = emailAddressQuery.Where(x => x.PersonId == personId),
                            SocialMedias = person.SocialMedias.Select(x => new SocialMediaDTO
                            {
                                Id = x.SocialMediaId,
                                SocialMediaType = x.SocialMediaType.SocialMediaTypeName,
                                SocialMediaTypeId = x.SocialMediaTypeId,
                                Value = x.SocialMediaValue
                            }).OrderBy(s => s.SocialMediaType),
                            PhoneNumbers = phoneNumberQuery.Where(x => x.PersonId == personId),
                            HasContactAgreement = person.HasContactAgreement,
                            PersonId = person.PersonId,
                            ParticipantId = currentParticipant == null ? 0 : currentParticipant.ParticipantId,
                            ProjectId = currentParticipant == null ? 0 : currentParticipant.ProjectId,
                            SevisId = currentParticipant == null ? "" : currentParticipant.ParticipantPerson.SevisId
                        };
            return query;
        }

        /// <summary>
        /// Returns the general information for a person
        /// </summary>
        /// <param name="context"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        public static IQueryable<GeneralDTO> CreateGetGeneralByIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from person in context.People
                        let participantPerson = context.ParticipantPersons.Where(x => x.Participant.PersonId == person.PersonId).FirstOrDefault()
                        let currentParticipant = person.Participations.OrderByDescending(p => p.ParticipantStatusId).FirstOrDefault() // the ID order has default precidence, for example if there are two statuses, Active(2) and Alumnus(1), Active is shown.
                        let hasCurrentParticipation = currentParticipant != null
                            && currentParticipant.Status != null
                            && currentParticipant.Status.Status != null
                        where person.PersonId == personId
                        select new GeneralDTO
                        {
                            PersonId = person.PersonId,
                            ProminentCategories = person.ProminentCategories.Select(x => new SimpleLookupDTO() { Id = x.ProminentCategoryId, Value = x.Name }),
                            Activities = person.Activities.Select(x => new SimpleLookupDTO() { Id = x.ActivityId, Value = x.Title }),
                            Memberships = person.Memberships.Select(x => new ECA.Business.Queries.Models.Persons.MembershipDTO() { Id = x.MembershipId, Name = x.Name }),
                            LanguageProficiencies = person.LanguageProficiencies.Select(x => new LanguageProficiencyDTO() { LanguageId = x.LanguageId, PersonId = x.PersonId, LanguageName = x.Language.LanguageName, IsNativeLanguage = x.IsNativeLanguage, ComprehensionProficiency = x.ComprehensionProficiency, ReadingProficiency = x.ReadingProficiency, SpeakingProficiency = x.SpeakingProficiency }),
                            // RelatedReports TBD
                            ImpactStories = person.Impacts.Select(x => new SimpleLookupDTO() { Id = x.ImpactId, Value = x.Description }),
                            CurrentStatus = hasCurrentParticipation ? currentParticipant.Status.Status : UNKNOWN_PARTICIPANT_STATUS,
                            ParticipantId = currentParticipant == null ? 0 : currentParticipant.ParticipantId,
                            ProjectId = currentParticipant == null ? 0 : currentParticipant.ProjectId,
                            SevisId = currentParticipant == null ? "" : currentParticipant.ParticipantPerson.SevisId
                        };

            return query;
        }

        /// <summary>
        /// Returns the education history query for the person with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="personId">The id of the person.</param>
        /// <returns>The education history query.</returns>
        public static IQueryable<EducationEmploymentDTO> CreateGetEducationsByPersonIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from education in context.ProfessionEducations
                        where education.PersonOfEducation_PersonId == personId
                        orderby education.DateFrom descending
                        select new EducationEmploymentDTO
                        {
                            ProfessionEducationId = education.ProfessionEducationId,
                            Title = education.Title,
                            Role = education.Role,
                            StartDate = education.DateFrom,
                            EndDate = education.DateTo,
                            OrganizationId = null,
                            PersonOfEducation_PersonId = personId,
                            PersonOfProfession_PersonId = null
                        };

            return query;
        }

        /// <summary>
        /// Returns the employment history query for the person with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="personId">The id of the person.</param>
        /// <returns>The employment history query.</returns>
        public static IQueryable<EducationEmploymentDTO> CreateGetEmploymentsByPersonIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from employment in context.ProfessionEducations
                        where employment.PersonOfProfession_PersonId == personId
                        orderby employment.DateFrom descending
                        select new EducationEmploymentDTO
                        {
                            ProfessionEducationId = employment.ProfessionEducationId,
                            Title = employment.Title,
                            Role = employment.Role,
                            StartDate = employment.DateFrom,
                            EndDate = employment.DateTo,
                            OrganizationId = null,
                            PersonOfEducation_PersonId = null,
                            PersonOfProfession_PersonId = personId
                        };

            return query;
        }

        /// <summary>
        /// Returns the query for the evaluation notes for the person with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="personId">The id of the person.</param>
        /// <returns>The person evaluation notes query.</returns>
        public static IQueryable<EvaluationNoteDTO> CreateGetEvaluationNotesByPersonIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from evaluationNote in context.PersonEvaluationNotes
                        join user in context.UserAccounts on evaluationNote.History.CreatedBy equals user.PrincipalId
                        join participant in context.Participants on user.PrincipalId equals participant.ParticipantId
                        where evaluationNote.PersonId == personId
                        orderby evaluationNote.History.CreatedOn descending
                        select new EvaluationNoteDTO
                        {
                            EvaluationNoteId = evaluationNote.EvaluationNoteId,
                            EvaluationNote = evaluationNote.EvaluationNote,
                            AddedOn = evaluationNote.History.CreatedOn,
                            RevisedOn = evaluationNote.History.RevisedOn,
                            UserId = evaluationNote.History.CreatedBy,
                            UserName = user.DisplayName,
                            EmailAddress = user.EmailAddress,
                            OfficeSymbol = participant.Organization.OfficeSymbol
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to retrieve dependent type dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public static IQueryable<DependentTypeDTO> CreateGetDependentTypesQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from dependentTypes in context.DependentTypes
                        select new DependentTypeDTO
                        {
                            Id = dependentTypes.DependentTypeId,
                            Name = dependentTypes.Name,
                            SevisDependentTypeCode = dependentTypes.SevisDependentTypeCode
                        };

            return query;
        }

        /// <summary>
        /// Returns a query to retrieve birth country reason dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public static IQueryable<BirthCountryReasonDTO> CreateGetBirthCountryReasonsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from birthCountryReasons in context.BirthCountryReasons
                        select new BirthCountryReasonDTO
                        {
                            Id = birthCountryReasons.BirthCountryReasonId,
                            Name = birthCountryReasons.Description
                        };

            return query;
        }
    }
}

