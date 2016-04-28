using ECA.Core.Settings;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using System;
using System.Diagnostics.Contracts;

namespace ECA.WebJobs.Sevis.Comm
{
    /// <summary>
    /// The SevisCommSchedule allows the developers to specify the sevis comm schedule in the app settings.
    /// </summary>
    public class SevisCommSchedule : TimerSchedule
    {
        /// <summary>
        /// Creates a new instance with a default AppSettings.
        /// </summary>
        public SevisCommSchedule() : this(new AppSettings())
        {

        }

        /// <summary>
        /// Creates a new instance with the given app settings.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        public SevisCommSchedule(AppSettings appSettings)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            this.AppSettings = appSettings;
        }

        /// <summary>
        /// Gets the app settings.
        /// </summary>
        public AppSettings AppSettings { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public override DateTime GetNextOccurrence(DateTime now)
        {
            //A neat site to help figure out cron expressions - just remember that azure expects a seconds value to be first
            //http://crontab.guru/#*_*_*_*_*

            //says run every fiftenn minutes from 6pm to 6am i.e. off business hours
            //"0 0/15 0,1,2,3,4,5,6,18,19,20,21,22,23 * * *"

            //says run every 20 seconds
            //"0/20 * * * * *"

            var instance = new CronSchedule(this.AppSettings.SevisCommCronSchedule);
            return instance.GetNextOccurrence(now);
        }
   }
}
