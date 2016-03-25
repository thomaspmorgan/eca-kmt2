using ECA.Business.Validation.Sevis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// An ISevisBatchProcessingNotificationService is used to track progress for various sevis batching processes.
    /// </summary>
    public interface ISevisBatchProcessingNotificationService
    {
        /// <summary>
        /// Executed when the number of participants that are ready to be sent to sevis has been determined.
        /// </summary>
        /// <param name="numberOfParticipants">The number of participants to stage for sevis.</param>
        void NotifyNumberOfParticipantsToStage(int numberOfParticipants);

        /// <summary>
        /// Executed when a batch has been created.
        /// </summary>
        /// <param name="batch">The created batch.</param>
        void NotifyStagedSevisBatchCreated(StagedSevisBatch batch);

        /// <summary>
        /// Executed when the staging process has completed all batches.
        /// </summary>
        /// <param name="batches">The completed batches.</param>
        void NotifyStagedSevisBatchesFinished(List<StagedSevisBatch> batches);

        /// <summary>
        /// Executed when the given exchange visitor fails validation before being added to a batch.
        /// </summary>
        /// <param name="exchangeVisitor">The invalid exchange visitor.</param>
        void NotifyInvalidExchangeVisitor(ExchangeVisitor exchangeVisitor);
    }
}
