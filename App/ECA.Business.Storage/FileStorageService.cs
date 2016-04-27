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
        //Uploads a byte array. Returns a Uri of the file's upload location, or null if the call failed validation.
        //</summary>
        public Uri UploadBlob(byte[] blobData, string contentType, string blobName, string containerName)
        {
            if (!blobValidation.IsValidArray(blobData))
            {
                return null;
            }

            if (!blobValidation.IsValidContent(contentType))
            {
                return null;
            }

            if (!blobValidation.IsValidContainer(containerName))
            {
                return null;
            }

            CloudBlockBlob blob = blobSettings.CreateBlockBlob(contentType, blobName, containerName);

            blob.UploadFromByteArray(blobData, 0, blobData.Length);

            return blob.Uri;
        }

        //<summary>
        //Uploads a byte array asyncronously. Returns a Uri of the file's upload location, or null if the call failed validation.
        //</summary>
        public async Task<Uri> UploadBlobAsync(byte[] blobData, string contentType, string blobName, string containerName)
        {
            if (!blobValidation.IsValidArray(blobData))
            {
                return null;
            }

            if (!blobValidation.IsValidContent(contentType))
            {
                return null;
            }

            if (!blobValidation.IsValidContainer(containerName))
            {
                return null;
            }

            CloudBlockBlob blob = blobSettings.CreateBlockBlob(contentType, blobName, containerName);

            await blob.UploadFromByteArrayAsync(blobData, 0, blobData.Length);

            return blob.Uri;
        }

        //<summary>
        //Uploads a stream. Returns a Uri of the file's upload location, or null if the call failed validation.
        //</summary>
        public Uri UploadBlob(Stream blobData, string contentType, string blobName, string containerName)
        {
            if (!blobValidation.IsValidContent(contentType))
            {
                return null;
            }

            if (!blobValidation.IsValidContainer(containerName))
            {
                return null;
            }

            CloudBlockBlob blob = blobSettings.CreateBlockBlob(contentType, blobName, containerName);

            blob.UploadFromStream(blobData);

            return blob.Uri;
        }

        //<summary>
        //Uploads a stream asyncronously. Returns a Uri of the file's upload location, or null if the call failed validation.
        //</summary>
        public async Task<Uri> UploadBlobAsync(Stream blobData, string contentType, string blobName, string containerName)
        {
            if (!blobValidation.IsValidContent(contentType))
            {
                return null;
            }

            if (!blobValidation.IsValidContainer(containerName))
            {
                return null;
            }

            CloudBlockBlob blob = blobSettings.CreateBlockBlob(contentType, blobName, containerName);

            await blob.UploadFromStreamAsync(blobData);

            return blob.Uri;
        }

        //<summary>
        //Returns the entire CloudBlockBlob object
        //</summary>
        public CloudBlockBlob GetBlob(string blobName, string containerName)
        {
            if (!blobValidation.IsValidContainer(containerName))
            {
                return null;
            }

            var container = blobSettings.BlobContainer(containerName);

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
        public Uri GetBlobLocation(string blobName, string containerName)
        {
            if (!blobValidation.IsValidContainer(containerName))
            {
                return null;
            }

            CloudBlockBlob blob = blobSettings.BlobContainer(containerName).GetBlockBlobReference(blobName);

            if (blob != null && blob.Exists())
            {
                return blob.Uri;
            }
            else
            {
                return null;
            }

        }

        //<summary>
        //Returns true if the delete succeeded or false if the delete failed
        //</summary>
        public void DeleteBlob(string blobName, string containerName)
        {
            if (!blobValidation.IsValidContainer(containerName))
            {
                return;
            }

            CloudBlockBlob blob = blobSettings.BlobContainer(containerName).GetBlockBlobReference(blobName);

            blob.Delete();
        }

        //<summary>
        //Returns true if the delete succeeded or false if the delete failed
        //</summary>
        public async Task DeleteBlobAsync(string blobName, string containerName)
        {
            if (!blobValidation.IsValidContainer(containerName))
            {
                return;
            }

            CloudBlockBlob blob = blobSettings.BlobContainer(containerName).GetBlockBlobReference(blobName);

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
        //Gets the CloudBlobContainer object using configuration settings
        //</summary>
        public CloudBlobContainer BlobContainer(string containerName)
        {
            string blobContainerName = containerName;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(settings.DS2019FileStorageConnectionString.ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(containerName);
        }

        //<summary>
        //Creates a new CloudBlockBlob object
        //</summary>
        public CloudBlockBlob CreateBlockBlob(string contentType, string blobName, string containerName)
        {
            CloudBlockBlob blob = BlobContainer(containerName).GetBlockBlobReference(blobName);
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

        //<summary>
        //Checks to make sure the container passed isn't empty
        //</summary>
        public bool IsValidContainer(string containerName)
        {
            if (containerName == "")
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
