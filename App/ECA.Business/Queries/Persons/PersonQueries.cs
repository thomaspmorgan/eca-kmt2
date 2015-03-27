using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Lookup;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
        /// 
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
                            DateOfBirth = person.DateOfBirth,
                            CountriesOfCitizenship = person.CountriesOfCitizenship.Select(x => new SimpleLookupDTO{Id = x.LocationId, Value = x.LocationName}),
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            NamePrefix = person.NamePrefix,
                            NameSuffix = person.NameSuffix,
                            GivenName = person.GivenName,
                            FamilyName = person.FamilyName,
                            MiddleName = person.MiddleName,
                            Patronym = person.Patronym,
                            Alias = person.Alias,
                            HomeAddresses = person.Addresses.Where(x => x.AddressTypeId == AddressType.Home.Id)
                                                        .Select(x => new LocationDTO {
                                                            Street1 = x.Location.Street1, 
                                                            Street2 = x.Location.Street2, 
                                                            Street3 = x.Location.Street3,
                                                            City = x.Location.City,
                                                            PostalCode = x.Location.PostalCode,
                                                            Country = x.Location.Country.LocationName
                                                        })
                        };
            return query;
        }
    }
}
