using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        /// Returns a query capable of retrieving people from the given context.  A FullName value is also calculated for the person.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve people from the context.</returns>
        public static IQueryable<SimplePersonDTO> CreateGetSimplePersonDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from person in context.People
                        let gender = person.Gender
                        let currentParticipation = person.Participations.OrderByDescending(p => p.StatusDate).FirstOrDefault()
                        let hasCurrentParticipation = currentParticipation != null
                            && currentParticipation.Status != null
                            && currentParticipation.Status.Status != null

                        select new SimplePersonDTO
                        {
                            Alias = person.Alias,
                            DateOfBirth = person.DateOfBirth,
                            FamilyName = person.FamilyName,
                            FirstName = person.FirstName,
                            Gender = gender.GenderName,
                            GivenName = person.GivenName,
                            PersonId = person.PersonId,
                            LastName = person.LastName,
                            MiddleName = person.MiddleName,
                            NamePrefix = person.NamePrefix,
                            NameSuffix = person.NameSuffix,
                            Patronym = person.Patronym,

                            FullName = (((person.NamePrefix != null && person.NamePrefix.Trim().Length > 0) ? (person.NamePrefix.Trim() + " ") : String.Empty)
                                        + ((person.FirstName != null && person.FirstName.Trim().Length > 0) ? (person.FirstName.Trim() + " ") : String.Empty)
                                        + ((person.MiddleName != null && person.MiddleName.Trim().Length > 0) ? (person.MiddleName.Trim() + " ") : String.Empty)
                                        + ((person.LastName != null && person.LastName.Trim().Length > 0) ? (person.LastName.Trim() + " ") : String.Empty)
                                        + ((person.Patronym != null && person.Patronym.Trim().Length > 0) ? (person.Patronym.Trim() + " ") : String.Empty)
                                        + ((person.NameSuffix != null && person.NameSuffix.Trim().Length > 0) ? (person.NameSuffix.Trim() + " ") : String.Empty)
                                        + ((person.Alias != null && person.Alias.Trim().Length > 0) ? ("(" + person.Alias.Trim() + ")") : String.Empty)
                                        ).Trim(),
                            CurrentStatus = hasCurrentParticipation ? currentParticipation.Status.Status : UNKNOWN_PARTICIPANT_STATUS
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

            var query = from person in context.People
                        where person.PersonId == personId
                        select new PiiDTO
                        {
                            Gender = person.Gender.GenderName,
                            GenderId = person.GenderId,
                            DateOfBirth = person.DateOfBirth,
                            CountriesOfCitizenship = person.CountriesOfCitizenship.Select(x => new SimpleLookupDTO { Id = x.LocationId, Value = x.LocationName }),
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            NamePrefix = person.NamePrefix,
                            NameSuffix = person.NameSuffix,
                            GivenName = person.GivenName,
                            FamilyName = person.FamilyName,
                            MiddleName = person.MiddleName,
                            Patronym = person.Patronym,
                            Alias = person.Alias,
                            MaritalStatus = person.MaritalStatus.Description,
                            MaritalStatusId = person.MaritalStatus.MaritalStatusId,
                            Ethnicity = person.Ethnicity,
                            MedicalConditions = person.MedicalConditions,
                            HomeAddresses = person.Addresses.Where(x => x.AddressTypeId == AddressType.Home.Id)
                                                        .Select(x => new LocationDTO
                                                        {
                                                            Id = x.LocationId,
                                                            Street1 = x.Location.Street1,
                                                            Street2 = x.Location.Street2,
                                                            Street3 = x.Location.Street3,
                                                            City = x.Location.City.LocationName,
                                                            CityId = context.Locations.Where(y => y.CountryId == x.Location.Country.LocationId &&
                                                                                             y.LocationName == x.Location.City.LocationName &&
                                                                                             y.LocationTypeId == LocationType.City.Id)
                                                                                             .FirstOrDefault().LocationId,

                                                            PostalCode = x.Location.PostalCode,
                                                            Country = x.Location.Country.LocationName,
                                                            CountryId = x.Location.Country.LocationId
                                                        }),
                            CityOfBirth = person.PlaceOfBirth.LocationName,
                            CityOfBirthId = person.PlaceOfBirthId,
                            CountryOfBirth = person.PlaceOfBirth.Country.LocationName,
                            CountryOfBirthId = person.PlaceOfBirth.Country.LocationId
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

            var query = from person in context.People
                        where person.PersonId == personId
                        select new ContactInfoDTO
                        {
                            Emails = person.Emails.Select(x => new SimpleLookupDTO() { Id = x.EmailAddressId, Value = x.Address }),
                            SocialMedias = person.SocialMedias.Select(x => new SimpleTypeLookupDTO() { Id = x.SocialMediaId, Type = x.SocialMediaType.SocialMediaTypeName, Value = x.SocialMediaValue }),
                            PhoneNumbers = person.PhoneNumbers.Select(x => new SimpleTypeLookupDTO() { Id = x.PhoneNumberId, Type = x.PhoneNumberType.PhoneNumberTypeName, Value = x.Number }),
                        };
            return query;
        }

        public static IQueryable<GeneralDTO> CreateGetGeneralByIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from person in context.People
                        let currentParticipation = person.Participations.OrderByDescending(p => p.StatusDate).FirstOrDefault()
                        let hasCurrentParticipation = currentParticipation != null
                            && currentParticipation.Status != null
                            && currentParticipation.Status.Status != null
                        where person.PersonId == personId
                        select new GeneralDTO
                        {
                            PersonId = person.PersonId,
                            ProminentCategories = person.ProminentCategories.Select(x => new SimpleLookupDTO() { Id = x.ProminentCategoryId, Value = x.Name }),
                            Events = person.Events.Select(x => new SimpleLookupDTO() { Id = x.EventId, Value = x.Title }),
                            Memberships = person.Memberships.Select(x => new ECA.Business.Queries.Models.Admin.SimpleOrganizationDTO() { OrganizationId = x.MembershipId, Name = x.Name }),
                            LanguageProficiencies = person.LanguageProficiencies.Select(x => new SimpleLookupDTO() { Id = x.LanguageProficiencyId, Value = x.LanguageName }),
                            Dependants = person.Family.Select(x => new SimpleLookupDTO() { Id = x.PersonId, Value = (x.LastName + ", " + x.FirstName) }),
                            // RelatedReports TBD
                            ImpactStories = person.Impacts.Select(x => new SimpleLookupDTO() { Id = x.ImpactId, Value = x.Description }),
                            CurrentStatus = hasCurrentParticipation ? currentParticipation.Status.Status : UNKNOWN_PARTICIPANT_STATUS

                        };

            return query;

        }

        public static IQueryable<EducationEmploymentDTO> CreateGetEducationsByPersonIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from education in context.ProfessionEducations
                        where education.PersonOfEducation.PersonId == personId
                        orderby education.DateFrom descending
                        select new EducationEmploymentDTO
                        {
                            Id = education.ProfessionEducationId,
                            Title = education.Title,
                            Role = education.Role,
                            StartDate = education.DateFrom,
                            EndDate = education.DateTo,
                            Organization = (education.Organization != null) ? new Models.Admin.SimpleOrganizationDTO()
                            {
                                OrganizationId = education.Organization.OrganizationId,
                                Name = education.Organization.Name,
                                OrganizationType = education.Organization.OrganizationType.OrganizationTypeName,
                                Location = education.Organization.Addresses.FirstOrDefault().DisplayName,
                                Status = education.Organization.Status
                            } : null
                        };

            return query;
        }

        public static IQueryable<EducationEmploymentDTO> CreateGetEmploymentsByPersonIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from employment in context.ProfessionEducations
                        where employment.PersonOfProfession.PersonId == personId
                        orderby employment.DateFrom descending
                        select new EducationEmploymentDTO
                        {
                            Id = employment.ProfessionEducationId,
                            Title = employment.Title,
                            Role = employment.Role,
                            StartDate = employment.DateFrom,
                            EndDate = employment.DateTo,
                            Organization = (employment.Organization != null) ? new Models.Admin.SimpleOrganizationDTO()
                            {
                                OrganizationId = employment.Organization.OrganizationId,
                                Name = employment.Organization.Name,
                                OrganizationType = employment.Organization.OrganizationType.OrganizationTypeName,
                                Location = employment.Organization.Addresses.FirstOrDefault().DisplayName,
                                Status = employment.Organization.Status
                            } : null
                        };

            return query;
        }

        public static IQueryable<EvaluationNoteDTO> CreateGetEvaluationNotesByPersonIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from evaluationNote in context.PersonEvaluationNotes
                        join user in context.UserAccounts on evaluationNote.History.CreatedBy equals user.PrincipalId
                        where evaluationNote.PersonId == personId
                        orderby evaluationNote.History.CreatedOn descending
                        select new EvaluationNoteDTO
                        {
                            EvaluationNoteId = evaluationNote.EvaluationNoteId,
                            EvaluationNote = evaluationNote.EvaluationNote,
                            AddedOn = evaluationNote.History.CreatedOn,
                            UserId = evaluationNote.History.CreatedBy,
                            UserName = user.DisplayName,
                            EmailAddress = user.EmailAddress
                        };
            return query;
        }
    }
}
