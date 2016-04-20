using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Sevis
{
    public interface IDummyCloudStorage
    {
        string SaveFile(string fileName, Stream stream, string contentType);

        Task<string> SaveFileAsync(string fileName, Stream stream, string contentType);
    }
}
