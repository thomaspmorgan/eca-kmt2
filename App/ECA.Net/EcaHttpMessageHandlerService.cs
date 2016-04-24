using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace ECA.Net
{
    public class EcaHttpMessageHandlerService : IEcaHttpMessageHandlerService
    {
        /// <summary>
        /// Method to add the personal certificate to the request.  This certificate is tied to the Program that is uploading the data
        /// The certificate has to have been previously uploaded via the Real Time interface
        /// For Azure, this cert has to be added to the website.
        /// </summary>
        /// <returns>The WebRequestHandler with the certficate added.</returns>
        public HttpMessageHandler GetHttpMessageHandler(string thumbprint)
        {
            var webRequestHandler = new WebRequestHandler();
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            if (certCollection.Count == 0)
            {
                throw new Exception(string.Format("Certificate not found for Thumbprint [{0}]", thumbprint));
            }

            var cert = certCollection[0];
            webRequestHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            webRequestHandler.ClientCertificates.Add(cert);
            return webRequestHandler;

        }
    }
}
