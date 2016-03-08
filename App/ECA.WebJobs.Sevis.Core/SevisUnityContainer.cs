using ECA.Core.Settings;
using ECA.Data;
using Microsoft.Practices.Unity;
using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using ECA.Business.Service.Sevis;
using ECA.Business.Service.Persons;
using ECA.Business.Validation;

namespace ECA.WebJobs.Sevis.Core
{
    /// <summary>
    /// The SevisUnityContainer is a unity container that can be used across multiple webjobs dealing with SEVIS.
    /// </summary>
    public class SevisUnityContainer : UnityContainer
    {
        private IBusinessValidator<Object, UpdatedParticipantPersonValidationEntity> participantPersonValidator;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        public SevisUnityContainer(AppSettings appSettings)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            var connectionString = GetConnectionString(appSettings);
            
            //Register ECA Context
            this.RegisterType<EcaContext>(new InjectionConstructor(connectionString));
            this.RegisterType<DbContext, EcaContext>(new InjectionConstructor(connectionString));

            this.RegisterType<ParticipantService>(new InjectionFactory((c) =>
            {
                var context = c.Resolve<EcaContext>();
                var service = new ParticipantService(context, null);
                return service;
            }));

            this.RegisterType<IExchangeVisitorService>(new InjectionFactory((c) =>
            {
                var context = c.Resolve<EcaContext>();
                var service = new ExchangeVisitorService(context, null);
                return service;
            }));
            
            //Register the SEVIS Batch Processing service
            this.RegisterType<ISevisBatchProcessingService>(new InjectionFactory((c) =>
            {
                var context = c.Resolve<EcaContext>();
                var exchangeVisitorService = c.Resolve<IExchangeVisitorService>();
                var participantService = c.Resolve<ParticipantService>();
                var participantPersonSevisService = c.Resolve<ParticipantPersonsSevisService>();
                var service = new SevisBatchProcessingService(context, exchangeVisitorService, participantService, participantPersonSevisService, null);
                return service;
            }));
        }

        /// <summary>
        /// Returns the eca connection string.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <returns>The connection string.</returns>
        private object GetConnectionString(AppSettings appSettings)
        {
            var connectionString = appSettings.EcaContextConnectionString.ConnectionString;
            LogMessage(String.Format("Using the connection string [{0}] to retrieve entities for documentation.", connectionString));
            return connectionString;
        }
        
        /// <summary>
        /// Writes the given message to the console.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
