using ECA.Business.Queries.Models.Admin;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// Contains queries against an ECA Context for social media entities.
    /// </summary>
    public static class SocialMediaQueries
    {
        /// <summary>
        /// Returns a query to get social media dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve social media dtos.</returns>
        public static IQueryable<SocialMediaDTO> CreateGetSocialMediaDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.SocialMedias.Select(x => new SocialMediaDTO
            {
                Id = x.SocialMediaId,
                SocialMediaType = x.SocialMediaType.SocialMediaTypeName,
                SocialMediaTypeId = x.SocialMediaTypeId,
                SocialMediaValue = x.SocialMediaValue
            });
        }

        /// <summary>
        /// Returns a query to get the social media dto for the social media entity with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="id">The social media id.</param>
        /// <returns>The social media dto with the given id.</returns>
        public static IQueryable<SocialMediaDTO> CreateGetSocialMediaDTOByIdQuery(EcaContext context, int id)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetSocialMediaDTOQuery(context).Where(x => x.Id == id);
        }
    }
}
