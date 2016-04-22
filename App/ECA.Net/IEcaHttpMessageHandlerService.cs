using System.Net.Http;

namespace ECA.Net
{
    public interface IEcaHttpMessageHandlerService
    {
       HttpMessageHandler GetHttpMessageHandler(string thumbprint);
    }
}
