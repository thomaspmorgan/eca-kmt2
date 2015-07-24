using ECA.Business.Queries.Models.Admin;
using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An ISocialMediaService is used to create or update social media presences for ECA objects.
    /// </summary>
    [ContractClass(typeof(SocialMediaServiceContract))]
    public interface ISocialMediaService : ISaveable
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

        /// <summary>
        /// Retrieves the social media dto with the given id.
        /// </summary>
        /// <param name="id">The id of the social media.</param>
        /// <returns>The social media dto.</returns>
        SocialMediaDTO GetById(int id);

        /// <summary>
        /// Retrieves the social media dto with the given id.
        /// </summary>
        /// <param name="id">The id of the social media.</param>
        /// <returns>The social media dto.</returns>
        Task<SocialMediaDTO> GetByIdAsync(int id);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SocialMediaDTO GetById(int id)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<SocialMediaDTO> GetByIdAsync(int id)
        {
            return Task.FromResult<SocialMediaDTO>(null);
        }
    }
}
