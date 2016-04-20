using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ECA.Core.Settings;
using System.IO;

namespace ECA.Business.Storage
{
    public class FileStorageService : IFileStorageService
    {
        private AppSettings settings;
        private IBlobStorageSettings blobSettings;
        BlobValidation blobValidation = new BlobValidation();

        public FileStorageService(AppSettings appSettings, IBlobStorageSettings blobStorageSettings)
        {
            this.settings = appSettings;
            this.blobSettings = blobStorageSettings;
        }

        //<summary>
        //Uploads a byte array. Returns a string of the file's upload location, or reason why the call failed validation.
        //</summary>
        public string UploadBlob(byte[] blobData, string contentType, string blobName)
        {
            if (!blobValidation.IsValidArray(blobData))
            {
                return "Invalid Array";
            }

            if (!blobValidation.IsValidContent(contentType))
            {
                return "Invalid Content Type";
            }

            CloudBlockBlob blob = blobSettings.CreateBlockBlob(contentType, blobName);

            blob.UploadFromByteArray(blobData, 0, blobData.Length);

            return blob.Uri.ToString();
        }

        //<summary>
        //Uploads a byte array asyncronously. Returns a string of the file's upload location, or reason why the call failed validation.
        //</summary>
        public async Task<string> UploadBlobAsync(byte[] blobData, string contentType, string blobName)
        {
            if (!blobValidation.IsValidArray(blobData))
            {
                return "Invalid Array";
            }

            if (!blobValidation.IsValidContent(contentType))
            {
                return "Invalid Content Type";
            }

            CloudBlockBlob blob = blobSettings.CreateBlockBlob(contentType, blobName);

            await blob.UploadFromByteArrayAsync(blobData, 0, blobData.Length);

            return blob.Uri.ToString();
        }

        //<summary>
        //Uploads a stream. Returns a string of the file's upload location, or reason why the call failed validation.
        //</summary>
        public string UploadBlob(Stream blobData, string contentType, string blobName)
        {
            if (!blobValidation.IsValidContent(contentType))
            {
                return "Invalid Content Type";
            }

            CloudBlockBlob blob = blobSettings.CreateBlockBlob(contentType, blobName);

            blob.UploadFromStream(blobData);

            return blob.Uri.ToString();
        }

        //<summary>
        //Uploads a stream asyncronously. Returns a string of the file's upload location, or reason why the call failed validation.
        //</summary>
        public async Task<string> UploadBlobAsync(Stream blobData, string contentType, string blobName)
        {
            if (!blobValidation.IsValidContent(contentType))
            {
                return "Invalid Content Type";
            }

            CloudBlockBlob blob = blobSettings.CreateBlockBlob(contentType, blobName);

            await blob.UploadFromStreamAsync(blobData);

            return blob.Uri.ToString();
        }

        //<summary>
        //Returns the entire CloudBlockBlob object
        //</summary>
        public CloudBlockBlob GetBlob(string blobName)
        {

            var container = blobSettings.BlobContainer();

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (blob != null && blob.Exists())
            {
                return blob;
            }
            else
            {
                return null;
            }
        }

        //<summary>
        //Returns the blob uri as a string
        //</summary>
        public string GetBlobLocation(string blobName)
        {
            CloudBlockBlob blob = blobSettings.BlobContainer().GetBlockBlobReference(blobName);

            if (blob != null && blob.Exists())
            {
                return blob.Uri.ToString();
            }
            else
            {
                return null;
            }

        }

        //<summary>
        //Returns true if the delete succeeded or false if the delete failed
        //</summary>
        public void DeleteBlob(string blobName)
        {
            CloudBlockBlob blob = blobSettings.BlobContainer().GetBlockBlobReference(blobName);

            blob.Delete();
        }

        //<summary>
        //Returns true if the delete succeeded or false if the delete failed
        //</summary>
        public async Task DeleteBlobAsync(string blobName)
        {
            CloudBlockBlob blob = blobSettings.BlobContainer().GetBlockBlobReference(blobName);

            await blob.DeleteAsync();
        }
    }

    public class BlobStorageSettings : IBlobStorageSettings
    {
        private AppSettings settings;

        public BlobStorageSettings(AppSettings appSettings)
        {
            this.settings = appSettings;
        }

        private static string cacheControls = "true, max-age=86400";

        //<summary>
        //Gets the container name from configuration
        //</summary>
        public string BlobContainerName()
        {
            return settings.DS2019FileStorageContainer;
        }

        //<summary>
        //Gets the CloudBlobContainer object using configuration settings
        //</summary>
        public CloudBlobContainer BlobContainer()
        {
            string blobContainerName = settings.DS2019FileStorageContainer;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(settings.DS2019FileStorageConnectionString.ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(BlobContainerName());
        }

        //<summary>
        //Creates a new CloudBlockBlob object
        //</summary>
        public CloudBlockBlob CreateBlockBlob(string contentType, string blobName)
        {
            CloudBlockBlob blob = BlobContainer().GetBlockBlobReference(blobName);
            blob.Properties.ContentType = contentType;
            blob.Properties.CacheControl = cacheControls;
            //blob.SetProperties();

            return blob;
        }
    }

    public class BlobValidation
    {
        //<summary>
        //Checks to make sure the MIME type is formatted correctly
        //</summary>
        public bool IsValidContent(string contentType)
        {
            if (!contentType.Contains("/") || contentType.Count(x => x == '/') > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //<summary>
        //Checks to make sure the array passed isn't null or empty
        //</summary>
        public bool IsValidArray(byte[] blobData)
        {
            if (blobData == null || blobData.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
