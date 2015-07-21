using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An ISocialMediaService is used to create or update social media presences for ECA objects.
    /// </summary>
    [ContractClass(typeof(SocialMediaServiceContract))]
    public interface ISocialMediaService
    {
        /// <summary>
        /// Creates a new social media in the ECA system.
        /// </summary>
        /// <typeparam name="T">The ISocialable entity type.</typeparam>
        /// <param name="socialMedia">The social media.</param>
        /// <returns>The created social media entity.</returns>
        SocialMedia Create<T>(SocialMediaPresence<T> socialMedia) where T : class, ISocialable;

        /// <summary>
        /// Creates a new social media in the ECA system.
        /// </summary>
        /// <typeparam name="T">The ISocialable entity type.</typeparam>
        /// <param name="socialMedia">The social media.</param>
        /// <returns>The created social media entity.</returns>
        Task<SocialMedia> CreateAsync<T>(SocialMediaPresence<T> socialMedia) where T : class, ISocialable;

        /// <summary>
        /// Updates the ECA system's social media data with the given updated social media.
        /// </summary>
        /// <param name="updatedSocialMedia">The updated social media.</param>
        void Update(UpdatedSocialMediaPresence updatedSocialMedia);

        /// <summary>
        /// Updates the ECA system's social media data with the given updated social media.
        /// </summary>
        /// <param name="updatedSocialMedia">The updated social media.</param>
        Task UpdateAsync(UpdatedSocialMediaPresence updatedSocialMedia);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ISocialMediaService))]
    public abstract class SocialMediaServiceContract : ISocialMediaService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="socialMedia"></param>
        /// <returns></returns>
        public SocialMedia Create<T>(SocialMediaPresence<T> socialMedia) where T : class, ISocialable
        {
            Contract.Requires(socialMedia != null, "The social media entity must not be null.");
            Contract.Ensures(Contract.Result<SocialMedia>() != null, "The social media entity returned must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="socialMedia"></param>
        /// <returns></returns>
        public Task<SocialMedia> CreateAsync<T>(SocialMediaPresence<T> socialMedia) where T : class, ISocialable
        {
            Contract.Requires(socialMedia != null, "The social media entity must not be null.");
            Contract.Ensures(Contract.Result<Task<SocialMedia>>() != null, "The social media entity returned must not be null.");
            return Task.FromResult<SocialMedia>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedSocialMedia"></param>
        public void Update(UpdatedSocialMediaPresence updatedSocialMedia)
        {
            Contract.Requires(updatedSocialMedia != null, "The updated social media must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedSocialMedia"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedSocialMediaPresence updatedSocialMedia)
        {
            Contract.Requires(updatedSocialMedia != null, "The updated social media must not be null.");
            return Task.FromResult<object>(null);
        }
    }
}
