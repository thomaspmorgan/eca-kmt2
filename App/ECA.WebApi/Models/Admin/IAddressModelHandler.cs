using ECA.Business.Service;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Threading.Tasks;
using System.Web.Http;
namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The AddressModelHandler is used a single point to add or update addresses for entities that are addressable in the ECA Business
    /// library.  All controller's that handle saving addresses should wrap this class.
    /// </summary>
    public interface IAddressModelHandler
    {
        /// <summary>
        /// Handles a controller action's request to add an address to an entity.
        /// </summary>
        /// <typeparam name="T">The ECA Data entity type that will have an address added.</typeparam>
        /// <param name="additionalAddress">The additional address.</param>
        /// <param name="controller">The controller handling the request.</param>
        /// <returns>The http action result.</returns>
        Task<IHttpActionResult> HandleAdditionalAddress<T>(AddressBindingModelBase<T> additionalAddress, ApiController controller)
            where T : class, IAddressable;
    }
}
