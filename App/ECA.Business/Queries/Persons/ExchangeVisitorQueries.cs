using ECA.Business.Queries.Admin;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Persons
{
    public static class ExchangeVisitorQueries
    {

        public static IQueryable<Biographical> CreateGetBiographicalDataByParticipantIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var emailAddressQuery = EmailAddressQueries.CreateGetEmailAddressDTOQuery(context);     
            var query = from participant in context.Participants
                        let participantPerson = participant.ParticipantPerson
                        let person = participant.Person

                        let gender = person.Gender

                        //right now assuming one country of citizenship, this should be checked by business layer
                        let countryOfCitizenship = person.CountriesOfCitizenship.FirstOrDefault()

                        let hasPlaceOfBirth = person.PlaceOfBirthId.HasValue
                        let placeOfBirth = hasPlaceOfBirth ? person.PlaceOfBirth : null

                        let hasCountryOfBirth = hasPlaceOfBirth && placeOfBirth.CountryId.HasValue
                        let countryOfBirth = hasCountryOfBirth ? placeOfBirth.Country : null

                        let emailAddress = emailAddressQuery.Where(x => x.PersonId.HasValue && x.PersonId == person.PersonId).FirstOrDefault()

                        let hostInsitutionAddressCountry = participantPerson.HostInstitutionAddress != null
                            && participantPerson.HostInstitutionAddress.Location != null
                            && participantPerson.HostInstitutionAddress.Location.Country != null
                            ? participantPerson.HostInstitutionAddress.Location.Country
                            : null
                        
                        where participant.PersonId.HasValue
                        && participant.ParticipantId == participantId

                        select new Biographical
                        {
                            FullName = new Validation.Model.FullName
                            {
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Suffix = person.NameSuffix,
                                PreferredName = person.Alias,
                            },
                            BirthDate = person.DateOfBirth,
                            Gender = gender != null ? gender.SevisGenderCode : null,
                            BirthCity = hasPlaceOfBirth ? placeOfBirth.LocationName : null,
                            BirthCountryCode = hasCountryOfBirth ? countryOfBirth.LocationIso2 : null,
                            CitizenshipCountryCode = countryOfCitizenship != null ? countryOfCitizenship.LocationIso2 : null,
                            BirthCountryReason = null,
                            EmailAddress = emailAddress != null ? emailAddress.Address : null,
                            PermanentResidenceCountryCode = hostInsitutionAddressCountry != null
                                ? hostInsitutionAddressCountry.LocationIso2
                                : null,
                            ResidentialAddress = null
                        };
            return query;
        }
    }
}
