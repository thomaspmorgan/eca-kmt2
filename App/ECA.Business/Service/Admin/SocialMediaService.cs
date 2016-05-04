using ECA.Business.Exceptions;
using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The SocialMediaService is capable of handling crud operations on an ISocialable entity
    /// and its social media presence.
    /// </summary>
    public class SocialMediaService : EcaService, ECA.Business.Service.Admin.ISocialMediaService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<ISocialable, int> throwIfSocialableEntityNotFound;
        private readonly Action<SocialMedia, int> throwIfSocialMediaNotFound;
        private Action<Participant> throwValidationErrorIfParticipantSevisInfoIsLocked;
        public readonly int[] LOCKED_SEVIS_COMM_STATUSES = { SevisCommStatus.QueuedToSubmit.Id, SevisCommStatus.PendingSevisSend.Id, SevisCommStatus.SentByBatch.Id };

        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public SocialMediaService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfSocialableEntityNotFound = (socialableEntity, id) =>
            {
                if (socialableEntity == null)
                {
                    throw new ModelNotFoundException(String.Format("The sociable entity with id [{0}] was not found.", id));
                }
            };
            throwIfSocialMediaNotFound = (socialMedia, id) =>
            {
                if (socialMedia == null)
                {
                    throw new ModelNotFoundException(String.Format("The social media with id [{0}] was not found.", id));
                }
            };
            throwValidationErrorIfParticipantSevisInfoIsLocked = (participant) =>
            {
                if (participant.ParticipantPerson != null)
                {
                    var sevisStatusId = participant.ParticipantPerson.ParticipantPersonSevisCommStatuses.OrderByDescending(x => x.AddedOn).Select(x => x.SevisCommStatusId).FirstOrDefault();

                    if (participant != null && IndexOfInt(LOCKED_SEVIS_COMM_STATUSES, sevisStatusId) != -1)
                    {
                        var msg = String.Format("An update was attempted on participant with id [{0}] but should have failed validation.",
                                participant.ParticipantId);

                        throw new EcaBusinessException(msg);
                    }
                }
            };
        }

        static int IndexOfInt(int[] arr, int value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }

        #region Get
        /// <summary>
        /// Retrieves the social media dto with the given id.
        /// </summary>
        /// <param name="id">The id of the social media.</param>
        /// <returns>The social media dto.</returns>
        public SocialMediaDTO GetById(int id)
        {
            var dto = SocialMediaQueries.CreateGetSocialMediaDTOByIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the social media dto with the given id [{0}].", id);
            return dto;
        }

        /// <summary>
        /// Retrieves the social media dto with the given id.
        /// </summary>
        /// <param name="id">The id of the social media.</param>
        /// <returns>The social media dto.</returns>
        public async Task<SocialMediaDTO> GetByIdAsync(int id)
        {
            var dto = await SocialMediaQueries.CreateGetSocialMediaDTOByIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the social media dto with the given id [{0}].", id);
            return dto;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a new social media in the ECA system.
        /// </summary>
        /// <typeparam name="T">The ISocialable entity type.</typeparam>
        /// <param name="socialMedia">The social media.</param>
        /// <returns>The created social media entity.</returns>
        public SocialMedia Create<T>(SocialMediaPresence<T> socialMedia) where T : class, ISocialable
        {
            var socialable = this.Context.Set<T>().Find(socialMedia.GetSocialableEntityId());
            return DoCreate(socialMedia, socialable);
        }

        /// <summary>
        /// Creates a new social media in the ECA system.
        /// </summary>
        /// <typeparam name="T">The ISocialable entity type.</typeparam>
        /// <param name="socialMedia">The social media.</param>
        /// <returns>The created social media entity.</returns>
        public async Task<SocialMedia> CreateAsync<T>(SocialMediaPresence<T> socialMedia) where T : class, ISocialable
        {
            var socialable = await this.Context.Set<T>().FindAsync(socialMedia.GetSocialableEntityId());
            return DoCreate(socialMedia, socialable);
        }

        private SocialMedia DoCreate<T>(SocialMediaPresence<T> socialMedia, ISocialable socialable) where T : class, ISocialable
        {
            throwIfSocialableEntityNotFound(socialable, socialMedia.GetSocialableEntityId());            
            return socialMedia.AddSocialMediaPresence(socialable);
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates the ECA system's social media data with the given updated social media.
        /// </summary>
        /// <param name="updatedSocialMedia">The updated social media.</param>
        public void Update(UpdatedSocialMediaPresence updatedSocialMedia)
        {
            var socialMedia = Context.SocialMedias.Find(updatedSocialMedia.Id);
            Participant participant = null;
            if (socialMedia != null && socialMedia.PersonId.HasValue)
            {
                participant = GetParticipantByPersonId((int)socialMedia.PersonId);
            }
            DoUpdate(updatedSocialMedia, socialMedia, participant);
        }

        /// <summary>
        /// Updates the ECA system's social media data with the given updated social media.
        /// </summary>
        /// <param name="updatedSocialMedia">The updated social media.</param>
        public async Task UpdateAsync(UpdatedSocialMediaPresence updatedSocialMedia)
        {
            var socialMedia = await Context.SocialMedias.FindAsync(updatedSocialMedia.Id);
            Participant participant = null;
            if (socialMedia != null && socialMedia.PersonId.HasValue)
            {
                participant = GetParticipantByPersonId((int)socialMedia.PersonId);
            }
            DoUpdate(updatedSocialMedia, socialMedia, participant);
        }

        private void DoUpdate(UpdatedSocialMediaPresence updatedSocialMedia, SocialMedia modelToUpdate, Participant participant)
        {
            Contract.Requires(updatedSocialMedia != null, "The updatedSocialMedia must not be null.");
            throwIfSocialMediaNotFound(modelToUpdate, updatedSocialMedia.Id);
            if (participant != null)
            {
                throwValidationErrorIfParticipantSevisInfoIsLocked(participant);
            }
            modelToUpdate.SocialMediaTypeId = updatedSocialMedia.SocialMediaTypeId;
            modelToUpdate.SocialMediaValue = updatedSocialMedia.Value;
            updatedSocialMedia.Update.SetHistory(modelToUpdate);
        }

        #endregion

        #region Delete
        /// <summary>
        /// Deletes the social media entry with the given id.
        /// </summary>
        /// <param name="socialMediaId">The id of the social media to delete.</param>
        public void Delete(int socialMediaId)
        {
            var socialMedia = Context.SocialMedias.Find(socialMediaId);
            DoDelete(socialMedia);
        }

        /// <summary>
        /// Deletes the social media entry with the given id.
        /// </summary>
        /// <param name="socialMediaId">The id of the social media to delete.</param>
        public async Task DeleteAsync(int socialMediaId)
        {
            var socialMedia = await Context.SocialMedias.FindAsync(socialMediaId);
            DoDelete(socialMedia);
        }

        private void DoDelete(SocialMedia socialMediaToDelete)
        {
            if (socialMediaToDelete != null)
            {
                Context.SocialMedias.Remove(socialMediaToDelete);
            }
        }
        #endregion
    }
}
