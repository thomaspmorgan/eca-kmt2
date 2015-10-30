﻿using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
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

                        select new SimplePersonDTO
                        {
                            Alias = person.Alias,
                            DateOfBirth = person.DateOfBirth,
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

                            FullName = (((person.NamePrefix != null && person.NamePrefix.Trim().Length > 0) ? (person.NamePrefix.Trim() + " ") : String.Empty)
                                        + ((person.FirstName != null && person.FirstName.Trim().Length > 0) ? (person.FirstName.Trim() + " ") : String.Empty)
                                        + ((person.MiddleName != null && person.MiddleName.Trim().Length > 0) ? (person.MiddleName.Trim() + " ") : String.Empty)
                                        + ((person.LastName != null && person.LastName.Trim().Length > 0) ? (person.LastName.Trim() + " ") : String.Empty)
                                        + ((person.Patronym != null && person.Patronym.Trim().Length > 0) ? (person.Patronym.Trim() + " ") : String.Empty)
                                        + ((person.NameSuffix != null && person.NameSuffix.Trim().Length > 0) ? (person.NameSuffix.Trim() + " ") : String.Empty)
                                        + ((person.Alias != null && person.Alias.Trim().Length > 0) ? ("(" + person.Alias.Trim() + ")") : String.Empty)
                                        ).Trim(),
                            CurrentStatus = hasCurrentParticipation ? currentParticipation.Status.Status : UNKNOWN_PARTICIPANT_STATUS,
                            CountryOfBirth = person.PlaceOfBirth != null ? person.PlaceOfBirth.Country.LocationName : null,
                            CityOfBirth = person.PlaceOfBirth != null ? person.PlaceOfBirth.LocationName : null,
                            CityOfBirthId = person.PlaceOfBirth != null ? person.PlaceOfBirth.LocationId : (int?)null
                        };
            return query;
        }

        /// <summary>
        /// Get a simple person dto by person id
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Simple person dto</returns>
        public static IQueryable<SimplePersonDTO> CreateGetSimplePersonDTOByPersonIdQuery(EcaContext context, int personId)
        {
            return CreateGetSimplePersonDTOsQuery(context).Where(x => x.PersonId == personId);
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
                            IsDateOfBirthUnknown = person.IsDateOfBirthUnknown,
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
                            CityOfBirth = person.PlaceOfBirth.LocationName,
                            CityOfBirthId = person.PlaceOfBirthId,
                            CountryOfBirth = person.PlaceOfBirth.Country.LocationName,
                            CountryOfBirthId = person.PlaceOfBirth.Country.LocationId,
                            IsPlaceOfBirthUnknown = person.IsPlaceOfBirthUnknown
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
                            EmailAddresses = person.EmailAddresses.Select(x => new EmailAddressDTO
                            {
                                Id = x.EmailAddressId,
                                Address = x.Address,
                                EmailAddressType = x.EmailAddressType.EmailAddressTypeName,
                                EmailAddressTypeId = x.EmailAddressTypeId
                            }),
                            SocialMedias = person.SocialMedias.Select(x => new SocialMediaDTO
                            {
                                Id = x.SocialMediaId,
                                SocialMediaType = x.SocialMediaType.SocialMediaTypeName,
                                SocialMediaTypeId = x.SocialMediaTypeId,
                                Value = x.SocialMediaValue
                            }).OrderBy(s => s.SocialMediaType),
                            PhoneNumbers = person.PhoneNumbers.Select(x => new PhoneNumberDTO() { Id = x.PhoneNumberId, PhoneNumberType = x.PhoneNumberType.PhoneNumberTypeName, PhoneNumberTypeId = x.PhoneNumberTypeId, Number = x.Number }),
                            HasContactAgreement = person.HasContactAgreement,
                            PersonId = person.PersonId
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
                        let currentParticipation = person.Participations.OrderByDescending(p => p.ParticipantStatusId).FirstOrDefault() // the ID order has default precidence, for example if there are two statues, Active(2) and Alumnus(1), Active is shown.
                        let hasCurrentParticipation = currentParticipation != null
                            && currentParticipation.Status != null
                            && currentParticipation.Status.Status != null
                        where person.PersonId == personId
                        select new GeneralDTO
                        {
                            PersonId = person.PersonId,
                            ProminentCategories = person.ProminentCategories.Select(x => new SimpleLookupDTO() { Id = x.ProminentCategoryId, Value = x.Name }),
                            Activities = person.Activities.Select(x => new SimpleLookupDTO() { Id = x.ActivityId, Value = x.Title }),
                            Memberships = person.Memberships.Select(x => new ECA.Business.Queries.Models.Persons.MembershipDTO() { Id = x.MembershipId, Name = x.Name }),
                            LanguageProficiencies = person.LanguageProficiencies.Select(x => new LanguageProficiencyDTO() { LanguageId = x.LanguageId, PersonId = x.PersonId, LanguageName = x.Language.LanguageName, IsNativeLanguage = x.IsNativeLanguage, ComprehensionProficiency = x.ComprehensionProficiency, ReadingProficiency = x.ReadingProficiency, SpeakingProficiency = x.SpeakingProficiency }),
                            Dependants = person.Family.Select(x => new SimpleLookupDTO() { Id = x.PersonId, Value = (x.LastName + ", " + x.FirstName) }),
                            // RelatedReports TBD
                            ImpactStories = person.Impacts.Select(x => new SimpleLookupDTO() { Id = x.ImpactId, Value = x.Description }),
                            CurrentStatus = hasCurrentParticipation ? currentParticipation.Status.Status : UNKNOWN_PARTICIPANT_STATUS
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
            //var allOrganizations = OrganizationQueries.CreateGetSimpleOrganizationsDTOQuery(context);

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
    }
}

