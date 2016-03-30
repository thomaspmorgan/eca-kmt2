using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Sevis
{
    public interface IDummyCloudStorage
    {
        string SaveFile(string fileName, byte[] contents, string contentType);

        Task<string> SaveFileAsync(string fileName, byte[] contents, string contentType);
    }
}
