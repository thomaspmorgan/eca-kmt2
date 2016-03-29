using ECA.Business.Service.Sevis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ECA.Business.Validation.Sevis;

namespace ECA.WebJobs.Sevis.Core
{
    /// <summary>
    /// Writes to the console sevis batch processing notification messages.
    /// </summary>
    public class TextWriterSevisBatchProcessingNotificationService : ISevisBatchProcessingNotificationService
    {
        private Stopwatch stagingStopwatch;

        /// <summary>
        /// Writes to the console the invalid exchange visitor that failed validation.
        /// </summary>
        /// <param name="exchangeVisitor">The exchange visitor that failed validation.</param>
        public void NotifyInvalidExchangeVisitor(ExchangeVisitor exchangeVisitor)
        {
            var participantId = exchangeVisitor != null && exchangeVisitor.Person != null ? exchangeVisitor.Person.ParticipantId : -1;
            var personId = exchangeVisitor != null && exchangeVisitor.Person != null ? exchangeVisitor.Person.PersonId : -1;
            Console.WriteLine(String.Format("ExchangeVisitor with participant id [{0}] and person id [{1}] failed validation.", participantId, personId));
        }

        /// <summary>
        /// Writes to the console the number of participants that will be staged to sevis batches.
        /// </summary>
        /// <param name="numberOfParticipants">The number of participants.</param>
        public void NotifyNumberOfParticipantsToStage(int numberOfParticipants)
        {
            stagingStopwatch = Stopwatch.StartNew();
            Console.WriteLine(String.Format("Found [{0}] participants to stage for sevis.", numberOfParticipants));
        }

        /// <summary>
        /// Writes to the console the new batch that was created.
        /// </summary>
        /// <param name="batch">The new batch.</param>
        public void NotifyStagedSevisBatchCreated(StagedSevisBatch batch)
        {
            Console.WriteLine(String.Format("A new sevis batch with id [{0}] was created.", batch.BatchId));
        }

        /// <summary>
        /// Writes to the console the number of batches that were successfully staged.
        /// </summary>
        /// <param name="batches">The number of batches.</param>
        public void NotifyStagedSevisBatchesFinished(List<StagedSevisBatch> batches)
        {
            stagingStopwatch.Stop();
            Console.WriteLine(String.Format("Finished staging [{0}] sevis batches in [{1}].", batches.Count(), stagingStopwatch.Elapsed));
        }
    }
}
