using ECA.Core.Settings;
using ECA.WebJobs.Core;
using ECA.WebJobs.Sevis.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;
using ECA.Business.Service.Sevis;

namespace ECA.WebJobs.Sevis.Comm
{
    class Program
    {
        static void Main()
        {
            var unityContainer = new SevisUnityContainer(new AppSettings());
            var config = new JobHostConfiguration
            {
                JobActivator = new UnityWebJobActivator(unityContainer),
            };
#if DEBUG
            config.UseDevelopmentSettings();
#endif
            config.UseTimers();
            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
