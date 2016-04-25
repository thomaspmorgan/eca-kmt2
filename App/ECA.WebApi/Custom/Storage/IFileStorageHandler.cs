using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ECA.WebApi.Custom.Storage
{
    public interface IFileStorageHandler
    {
        Task<bool> BlobExistsAsync(string fileName);

        Task<HttpResponseMessage> GetFileAsync(string fileName);
    }
}
