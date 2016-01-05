using ECA.Core.Settings;
using ECA.Data;
using Microsoft.Practices.Unity;
using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;

namespace ECA.WebJobs.Sevis.Core
{
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
            var apiKey = GetAzureSevisApiKey(appSettings);
            var postServiceName = GetSevisPostServiceName(appSettings);
            var getServiceName = GetSevisGetServiceName(appSettings);
            var connectionString = GetConnectionString(appSettings);
            
            //Register ECA Context
            this.RegisterType<EcaContext>(new InjectionConstructor(connectionString));
            this.RegisterType<DbContext, EcaContext>(new InjectionConstructor(connectionString));
        }

        /// <summary>
        /// Returns the azure sevis api key.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <returns>The azure sevis api key.</returns>
        private object GetAzureSevisApiKey(AppSettings appSettings)
        {
            var apiKey = appSettings.SevisApiKey;
            LogMessage(String.Format("The azure sevis api key is [{0}].", apiKey));
            return apiKey;
        }

        /// <summary>
        /// Returns the name of the azure sevis post service.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <returns>The azure sevis post service name.</returns>
        private object GetSevisPostServiceName(AppSettings appSettings)
        {
            var postServiceName = appSettings.SevisPostServiceName;
            LogMessage(String.Format("The azure sevis POST service name is [{0}].", postServiceName));
            return postServiceName;
        }

        /// <summary>
        /// Returns the name of the azure sevis get service.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <returns>The azure sevis get service name.</returns>
        private object GetSevisGetServiceName(AppSettings appSettings)
        {
            var getServiceName = appSettings.SevisGetServiceName;
            LogMessage(String.Format("The azure sevis GET service name is [{0}].", getServiceName));
            return getServiceName;
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
