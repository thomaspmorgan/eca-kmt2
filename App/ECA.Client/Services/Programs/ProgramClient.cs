using PortableRest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Client.Services.Programs
{
    public class ProgramClient : RestClient
    {
        public ProgramClient(string baseUrl)
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException("baseUrl");
            }
            this.BaseUrl = baseUrl;
        }

        public async Task<ContentResponse> Get(int start, int limit)
        {
            var result = await SendAsync<ContentResponse>(request);
            
        }


    }
}
