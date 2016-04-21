using Microsoft.WindowsAzure.Storage.Blob;

namespace ECA.Business.Storage
{
    public interface IBlobStorageSettings
    {
        CloudBlobContainer BlobContainer();
        CloudBlockBlob CreateBlockBlob(string contentType, string blobName);
    }
}