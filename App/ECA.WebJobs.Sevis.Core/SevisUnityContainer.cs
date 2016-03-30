using ECA.Business.Service.Persons;
using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using ECA.Data;
using Microsoft.Practices.Unity;
using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.WebJobs.Sevis.Core
{
    public class DummyCloudStorage : IDummyCloudStorage
    {
        public string SaveFile(string fileName, byte[] contents, string contentType)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveFileAsync(string fileName, byte[] contents, string contentType)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The SevisUnityContainer is a unity container that can be used across multiple webjobs dealing with SEVIS.
    /// </summary>
    public class SevisUnityContainer : UnityContainer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        public SevisUnityContainer(AppSettings appSettings)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            this.RegisterInstance<AppSettings>(appSettings);
            this.RegisterType<ZipArchiveDS2019FileProvider>();

            var connectionString = GetConnectionString(appSettings);
            this.RegisterType<ISevisBatchProcessingNotificationService, TextWriterSevisBatchProcessingNotificationService>();
            this.RegisterType<IDummyCloudStorage, DummyCloudStorage>(); 
            //Register ECA Context
            this.RegisterType<EcaContext>(new InjectionConstructor(connectionString));
            this.RegisterType<DbContext, EcaContext>(new InjectionConstructor(connectionString));

            this.RegisterType<IExchangeVisitorService>(new InjectionFactory((c) =>
            {
                var service = new ExchangeVisitorService(context: c.Resolve<EcaContext>(), appSettings: appSettings, saveActions: null);
                return service;
            }));

            this.RegisterType<IExchangeVisitorValidationService>(new InjectionFactory((c) =>
            {
                var context = c.Resolve<EcaContext>();
                var service = new ExchangeVisitorValidationService(
                    context: c.Resolve<EcaContext>(),
                    exchangeVisitorService: c.Resolve<IExchangeVisitorService>(),
                    exchangeVisitorValidator: null, 
                    saveActions: null
                    );
                return service;
            }));
            
            //Register the SEVIS Batch Processing service
            this.RegisterType<ISevisBatchProcessingService>(new InjectionFactory((c) =>
            {
                var context = c.Resolve<EcaContext>();
                var service = new SevisBatchProcessingService(
                    context: c.Resolve<EcaContext>(),
                    numberOfDaysToKeepProcessedBatches: Double.Parse(appSettings.NumberOfDaysToKeepProcessedSevisBatchRecords),
                    cloudStorageService: c.Resolve<IDummyCloudStorage>(),
                    exchangeVisitorService: c.Resolve<IExchangeVisitorService>(),
                    notificationService: c.Resolve<ISevisBatchProcessingNotificationService>(),
                    exchangeVisitorValidationService: c.Resolve<IExchangeVisitorValidationService>(),
                    sevisOrgId: appSettings.SevisOrgId,
                    maxCreateExchangeVisitorRecordsPerBatch: Int32.Parse(appSettings.MaxCreateExchangeVisitorRecordsPerBatch),
                    maxUpdateExchangeVisitorRecordsPerBatch: Int32.Parse(appSettings.MaxUpdateExchangeVisitorRecordsPerBatch),
                    saveActions: null
                    );
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
