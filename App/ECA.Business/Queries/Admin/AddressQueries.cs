using ECA.Business.Queries.Models.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// The AddressQueries class contains LINQ queries for retrieving addresses from the ECAContext.
    /// </summary>
    public static class AddressQueries
    {
        /// <summary>
        /// Returns a query to retrieve address dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve addresses from the context.</returns>
        public static IQueryable<AddressDTO> CreateGetAddressDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from address in context.Addresses
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
                            AddressDisplayName = address.DisplayName,
                            AddressId = address.AddressId,
                            AddressType = addressType.AddressName,
                            AddressTypeId = addressType.AddressTypeId,
                            City = hasCity ? city.LocationName : null,
                            CityId = location.CityId,
                            Country = hasCountry ? country.LocationName : null,
                            CountryId =  location.CountryId,
                            Division = hasDivision ? division.LocationName : null,
                            DivisionId = location.DivisionId,
                            LocationId = location.LocationId,
                            LocationName = location.LocationName,
                            OrganizationId = address.OrganizationId,
                            PersonId = address.PersonId,
                            Street1 = location.Street1,
                            Street2 = location.Street2,
                            Street3 = location.Street3,                            
                        };
            return query;
        }

        /// <summary>
        /// Returns the query to retrieve an address with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="addressId">The address id.</param>
        /// <returns>The query to retrieve the address with the given address id.</returns>
        public static IQueryable<AddressDTO> CreateGetAddressDTOByIdQuery(EcaContext context, int addressId)
        {
            return CreateGetAddressDTOQuery(context).Where(x => x.AddressId == addressId);
        }
    }
}
