using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using System.Diagnostics.Contracts;
using ECA.Business.Service.Persons;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.WebJobs.Sevis.Validation
{
    public class Functions
    {
        private IParticipantPersonsSevisService service;
        private IExchangeVisitorValidationService exchangeVisitorValidationService;
        private AppSettings appSettings;

        /// <summary>
        /// Creates a new instance with the service to use and the app settings.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="appSettings">The app settings.</param>
        public Functions(IParticipantPersonsSevisService service, IExchangeVisitorValidationService exchangeVisitorValidationService, AppSettings appSettings)
        {
            Contract.Requires(service != null, "The service must not be null.");
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            Contract.Requires(exchangeVisitorValidationService != null, "The exchange visitor validation service must not be null.");
            this.service = service;
            this.appSettings = appSettings;
            this.exchangeVisitorValidationService = exchangeVisitorValidationService;
        }

        //https://azure.microsoft.com/en-us/documentation/articles/websites-dotnet-webjobs-sdk/#schedule
        //https://github.com/Azure/azure-webjobs-sdk-extensions

        //run at startup and once every 20 secs in debug or once every 5 mins in release

        /// <summary>
        /// Locates participants that are registered with sevis and whose start date has passed so that the sevis validation process may start.
        /// </summary>
        /// <param name="info">The timer trigger instance.</param>
        /// <returns>The task</returns>
        public async Task ProcessTimer(
#if DEBUG
            [TimerTrigger("00:00:20", RunOnStartup = true)] TimerInfo info
#else
            [TimerTrigger("00:05:00", RunOnStartup = true)] TimerInfo info
#endif   
            )
        {
            Console.WriteLine(String.Format("Starting validation on exchange visitors."));
            var queryOperator = new QueryableOperator<ReadyToValidateParticipantDTO>(0, 25, new ExpressionSorter<ReadyToValidateParticipantDTO>(x => x.ParticipantId, SortDirection.Ascending));
            var results = await service.GetReadyToValidateParticipantsAsync(queryOperator);
            Console.WriteLine(String.Format("Found [{0}] total participants to validate.", results.Total));
            while(results.Total > 0)
            {
                foreach(var participant in results.Results)
                {
                    Console.WriteLine(String.Format("Validating participant [{0}] with sevis id [{1}] for project [{2}].", participant.ParticipantId, participant.SevisId, participant.ProjectId));
                    await RunParticipantSevisValidationAsync(participant.ProjectId, participant.ParticipantId);
                }
                results = await service.GetReadyToValidateParticipantsAsync(queryOperator);
            }
            Console.WriteLine("Finished exchange visitor validation participants.");
            var nextOccurrenceMessage = info.FormatNextOccurrences(1);
            Console.WriteLine(nextOccurrenceMessage);
        }

        public async Task RunParticipantSevisValidationAsync(int projectId, int participantId)
        {
            await exchangeVisitorValidationService.RunParticipantSevisValidationAsync(projectId, participantId);
            await exchangeVisitorValidationService.SaveChangesAsync();
        }

        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.service is IDisposable)
                {
                    Console.WriteLine("Disposing of service " + this.service.GetType());
                    ((IDisposable)this.service).Dispose();
                    this.service = null;
                }
            }
        }

        #endregion
    }
}
