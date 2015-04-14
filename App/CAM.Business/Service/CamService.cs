using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using CAM.Data;
using NLog.Interface;
using System.Data.Entity;
using System.Diagnostics;



namespace CAM.Business.Service
{
    class CamService: IDisposable
    {
        private CamModel cam = new CamModel();
        private readonly ILogger logger = new LoggerAdapter(NLog.LogManager.GetCurrentClassLogger());

        protected UserAccount GetUserAccountById(int userId)
        {
            var stopwatch = Stopwatch.StartNew();
            var userAccount = cam.UserAccounts.Find(userId);
            stopwatch.Stop();
            logger.Trace("Time Elapsed {0}", stopwatch.Elapsed);
            return userAccount;
        }

        /// <summary>
        /// Returns the UserAccount with the given id.
        /// </summary>
        /// <param name="userId">The focus id.</param>
        /// <returns>The UserAccount.</returns>
        protected async Task<UserAccount> GetUserAccountByIdAsync(int userId)
        {
            var stopwatch = Stopwatch.StartNew();
            var userAccount = await cam.UserAccounts.FindAsync(userId);
            stopwatch.Stop();
            logger.Trace("Time Elapsed: {0}", stopwatch.Elapsed);
            return userAccount;
        }

        public void Dispose()
        {
            cam.Dispose();
            cam = null;
        }
    }
}