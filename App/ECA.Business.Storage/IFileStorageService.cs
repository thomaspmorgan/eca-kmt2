using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ECA.Business.Storage
{
    public interface IFileStorageService
    {
        void DeleteBlob(string blobName, string containerName);
        Task DeleteBlobAsync(string blobName, string containerName);
        CloudBlockBlob GetBlob(string blobName, string containerName);
        string GetBlobLocation(string blobName, string containerName);
        Uri UploadBlob(Stream blobData, string contentType, string blobName, string containerName);
        Uri UploadBlob(byte[] blobData, string contentType, string blobName, string containerName);
        Task<Uri> UploadBlobAsync(Stream blobData, string contentType, string blobName, string containerName);
        Task<Uri> UploadBlobAsync(byte[] blobData, string contentType, string blobName, string containerName);
    }
}