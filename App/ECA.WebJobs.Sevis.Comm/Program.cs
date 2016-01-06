using ECA.Core.Settings;
using ECA.WebJobs.Core;
using ECA.WebJobs.Sevis.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;

namespace ECA.WebJobs.Sevis.Comm
{
    class Program
    {
        static void Main()
        {
            var unityContainer = new UnityContainer();
            var config = new JobHostConfiguration
            {
                JobActivator = new UnityWebJobActivator(new SevisUnityContainer(new AppSettings()))
            };
            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
