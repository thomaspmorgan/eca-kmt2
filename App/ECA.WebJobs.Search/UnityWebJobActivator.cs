using Microsoft.Azure.WebJobs.Host;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebJobs.Search
{
    public class UnityWebJobActivator : IJobActivator
    {
        private readonly IUnityContainer container;

        public UnityWebJobActivator(IUnityContainer container)
        {
            Contract.Requires(container != null, "The container must not be null.");
            this.container = container;
        }

        public T CreateInstance<T>()
        {
            return container.Resolve<T>();
        }
    }
}
