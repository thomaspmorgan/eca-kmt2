﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using ECA.Core.Settings;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net.Http.Headers;
using System.IO.Compression;

namespace ECA.Business.Sevis
{
    public class SevisComm
    {

        private Uri UploadUri;

        private Uri DownloadUri;

        private string UserId;

        private string OrgId;

        private string Thumbprint;

        private string Passphrase;

        public SevisComm(AppSettings appSettings)
        {
            UploadUri = new Uri(appSettings.SevisUploadUri);
            UserId = appSettings.SevisUserId;
            OrgId = appSettings.SevisOrgId;
            Thumbprint = appSettings.SevisThumbprint;
            Passphrase = appSettings.SevisPassphrase;
        }

        public async Task<HttpResponseMessage> UploadAsync(XElement xml, string batchId)
        {
            using (var httpClient = new HttpClient(GetWebRequestHandler()))
            {
                using (var content = new MultipartFormDataContent())
                {
                    var formValues = new[]
                    {
                        new KeyValuePair<string, string>("batchid", batchId),
                        new KeyValuePair<string, string>("orgid", OrgId),
                        new KeyValuePair<string, string>("userid",UserId)
                    };
                    foreach (var keyValuePair in formValues)
                    {
                        content.Add(new StringContent(keyValuePair.Value, Encoding.UTF8), "\"" + keyValuePair.Key + "\"");
                        content.Headers.Remove("Content-Type");
                    }
                    var xmlValuePair = new KeyValuePair<string, string>("xml", xml.ToString());
                    var xmlContent = new StringContent(xmlValuePair.Value, Encoding.UTF8);
                    xmlContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "\"xml\"", FileName = "\"batchfile.xml\"" };
                    xmlContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                    content.Add(xmlContent, "\"" + xmlValuePair.Key + "\"");

                var response = await httpClient.PostAsync(UploadUri, content);

                return response;
            }
        }
        }

        public async Task<SevisDownload> DownloadAsync(XElement xml, string batchId)
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
