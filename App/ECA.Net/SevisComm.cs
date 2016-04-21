using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using ECA.Core.Settings;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;
using System.Text;

namespace ECA.Net
{
    public class SevisComm
    {

        private Uri UploadUri;

        private Uri DownloadUri;

        private string Thumbprint;

        private static string boundary = "----ECAKMTBoundary" + DateTime.Now.Ticks.ToString("x");

        /// <summary>
        /// Constructor for the SEVIS Comm object, must be passed the application settings object to have the upload/download URLS and Cert thumbprint
        /// </summary>
        /// <param name="appSettings">The application settings object</param>
        public SevisComm(AppSettings appSettings)
        {
            DownloadUri = new Uri(appSettings.SevisDownloadUri);
            UploadUri = new Uri(appSettings.SevisUploadUri);
            Thumbprint = appSettings.SevisThumbprint;
        }

        /// <summary>
        /// This method uploads an XML file for a batch of users that need to be issued J-1 Visas by the SEVIS system
        /// </summary>
        /// <param name="xml">The xml file passed as an XElement</param>
        /// <param name="BatchId">The BatchID of the upload, to be used to download the results later</param>
        /// <param name="OrgId">The Program ID in SEVIS</param>
        /// <param name="UserId">The SEVIS user name performing the upload</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> UploadAsync(XElement xml, string BatchId, string OrgId, string UserId)
        {
            using (var httpClient = new HttpClient(GetWebRequestHandler()))
            {
                httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("curl/7.46.0");
                httpClient.DefaultRequestHeaders.Expect.ParseAdd("100-continue");
                using (var content = new MultipartFormDataContent(boundary))
                {
                    content.Headers.Remove("Content-Type");
                    content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);

                    var formValues = new[]
                    {
                        new KeyValuePair<string, string>("orgid", OrgId),
                        new KeyValuePair<string, string>("userid",UserId),
                        new KeyValuePair<string, string>("batchid", BatchId),
                    };
                    foreach (var keyValuePair in formValues)
                    {
                        var newContent = new StringContent(keyValuePair.Value, Encoding.UTF8);
                        newContent.Headers.Remove("Content-Type");
                        content.Add(newContent,"\"" + keyValuePair.Key + "\"");

                    }
                    var xmlValuePair = new KeyValuePair<string, string>("xml", xml.ToString());
                    var xmlContent = new StringContent(xmlValuePair.Value + "\r\n", Encoding.UTF8);
                    xmlContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "\"xml\"", FileName = "\"batchfile3.xml\"" };
                    xmlContent.Headers.Remove("Content-Type");

                    content.Add(xmlContent, "\"" + xmlValuePair.Key + "\"");

                    var response = await httpClient.PostAsync(UploadUri, content);

                    return response;
                }
            }
        }

        /// <summary>
        /// Downloads the results of a batch upload from SEVIS
        /// </summary>
        /// <param name="BatchId">The BatchID to download</param>
        /// <param name="OrgId">The Program ID</param>
        /// <param name="UserId">The SEVIS User that is performing the download</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DownloadAsync(string BatchId, string OrgId, string UserId)
        {
            using (var httpClient = new HttpClient(GetWebRequestHandler()))
            {
                httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("curl/7.46.0");
                httpClient.DefaultRequestHeaders.Expect.ParseAdd("100-continue");
                using (var content = new MultipartFormDataContent(boundary))
                {
                    content.Headers.Remove("Content-Type");
                    content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);

                    var formValues = new[]
                    {
                        new KeyValuePair<string, string>("orgid", OrgId),
                        new KeyValuePair<string, string>("userid",UserId),
                        new KeyValuePair<string, string>("batchid", BatchId),
                    };
                    foreach (var keyValuePair in formValues)
                    {
                        var newContent = new StringContent(keyValuePair.Value, Encoding.UTF8);
                        newContent.Headers.Remove("Content-Type");
                        content.Add(newContent, "\"" + keyValuePair.Key + "\"");
                    }
                    var response = await httpClient.PostAsync(DownloadUri, content);

                    return response;
                }
            }
 
        }

        /// <summary>
        /// Method to add the personal certificate to the request.  This certificate is tied to the Program that is uploading the data
        /// The certificate has to have been previously uploaded via the Real Time interface
        /// For Azure, this cert has to be added to the website.
        /// </summary>
        /// <returns>The WebRequestHandler with the certficate added.</returns>
        private WebRequestHandler GetWebRequestHandler()
        {
            var webRequestHandler = new WebRequestHandler();
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, Thumbprint, false);
            if(certCollection.Count == 0)
            {
                throw new Exception(string.Format("Certificate not found for Thumbprint [{0}]", Thumbprint));
            }

            var cert = certCollection[0];
            webRequestHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            webRequestHandler.ClientCertificates.Add(cert);
            return webRequestHandler;
            
        }



    }
}
