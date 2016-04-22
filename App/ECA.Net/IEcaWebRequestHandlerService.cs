using System.Net.Http;

namespace ECA.Net
{
    public interface IEcaWebRequestHandlerService
    {
       WebRequestHandler GetWebRequestHandler(string thumbprint);
    }
}
