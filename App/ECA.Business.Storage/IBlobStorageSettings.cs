using Microsoft.WindowsAzure.Storage.Blob;

namespace ECA.Business.Storage
{
    public interface IBlobStorageSettings
    {
        CloudBlobContainer BlobContainer(string containerName);
        CloudBlockBlob CreateBlockBlob(string contentType, string blobName, string containerName);
    }
}