using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebJobs.Sevis.Comm
{
    public class Functions : IDisposable
    {







        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    foreach (var service in this.documentServices)
            //    {
            //        if (service is IDisposable)
            //        {
            //            Console.WriteLine("Disposing of service " + service.GetType());
            //            ((IDisposable)service).Dispose();
            //        }
            //    }
            //}
        }

        #endregion
    }
}
