using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using ECA.Core.Settings;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;
using System.IO.Compression;
using System.Text;

namespace ECA.Business.Sevis
{
    public class SevisComm
    {

        private Uri UploadUri;

        private Uri DownloadUri;

        private string Thumbprint;

        private static string boundary = "----ECAKMTBoundary" + DateTime.Now.Ticks.ToString("x");

        public SevisComm(AppSettings appSettings)
        {
            DownloadUri = new Uri(appSettings.SevisDownloadUri);
            UploadUri = new Uri(appSettings.SevisUploadUri);
            Thumbprint = appSettings.SevisThumbprint;
        }

        public async Task<HttpResponseMessage> UploadAsync(XElement xml, string batchId, string OrgId, string UserId)
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
                        new KeyValuePair<string, string>("batchid", batchId),
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

        public async Task<SevisDownload> DownloadAsync(XElement xml, string batchId, string OrgId, string UserId)
        {
            using (var httpClient = new HttpClient(GetWebRequestHandler()))
            {
                var content = new StringContent(String.Format("batchid={0}&orgid={1}&userid={2}", batchId, OrgId, UserId));
                var response = await httpClient.PostAsync(DownloadUri, content);
                if (response.IsSuccessStatusCode)
                {
                    var zipStream = await response.Content.ReadAsStreamAsync();

                    ZipArchive archive = new ZipArchive(zipStream);

                    ZipArchiveEntry entry = archive.Entries[0];

                    var sevisDownload = new SevisDownload();
                    sevisDownload.TransactionLog = XElement.Parse("<root></root>");
                    sevisDownload.Zipfile = null;
                    return sevisDownload;
                }
                else
                    return null;
            }
 
        }

        private WebRequestHandler GetWebRequestHandler()
        {
            var webRequestHandler = new WebRequestHandler();
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, Thumbprint, false);
            var cert = certCollection[0];

            if (cert == null) throw new Exception(string.Format("Certificate not found for Thumbprint [{0}]", Thumbprint));

            webRequestHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            webRequestHandler.ClientCertificates.Add(cert);
            return webRequestHandler;
            
        }



    }
}
