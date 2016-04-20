using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ECA.Business.Storage
{
    public interface IFileStorageService
    {
        void DeleteBlob(string blobName);
        Task DeleteBlobAsync(string blobName);
        CloudBlockBlob GetBlob(string blobName);
        string GetBlobLocation(string blobName);
        string UploadBlob(byte[] blobData, string contentType, string blobName);
        Task<string> UploadBlobAsync(byte[] blobData, string contentType, string blobName);
    }
}