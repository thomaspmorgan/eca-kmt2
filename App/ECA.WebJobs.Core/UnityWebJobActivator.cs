using Microsoft.Azure.WebJobs.Host;
using Microsoft.Practices.Unity;
using System.Diagnostics.Contracts;

namespace ECA.WebJobs.Core
{
    /// <summary>
    /// The UnityWebJobActivator is a simple class to implement to the azure IJobActivator and provide Unity dependency injection support.
    /// </summary>
    public class UnityWebJobActivator : IJobActivator
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Creates a default instance.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public UnityWebJobActivator(IUnityContainer container)
        {
            Contract.Requires(container != null, "The container must not be null.");
            this.container = container;
        }

        /// <summary>
        /// Returns an instance of T.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <returns>An instance of T.</returns>
        public T CreateInstance<T>()
        {
            return container.Resolve<T>();
        }
    }
}
