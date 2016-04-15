using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// A CamRoleChangeMonitor is used to monitor cached users when a role has been modified.  If a role has been modified
    /// then cached user's permissions will be out of date and therefore should be refreshed on the next request.  
    /// 
    /// By attaching this change monitor a cached user, the system can clear all cached users.
    /// </summary>
    //http://stackoverflow.com/questions/4183270/how-to-clear-the-net-4-memorycache/22388943#comment34789210_22388943
    public class CamRoleChangeMonitor : ChangeMonitor
    {
        private string uniqueId;

        /// <summary>
        /// 
        /// </summary>
        // Shared across all SignaledChangeMonitors in the AppDomain
        public static event EventHandler<CamRoleChangeMonitorEventArgs> RoleChanged;

        /// <summary>
        /// Creates a new CamRoleChangeMonitor.
        /// </summary>
        public CamRoleChangeMonitor()
        {
            uniqueId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            // Register instance with the shared event
            CamRoleChangeMonitor.RoleChanged += OnRoleChanged;
            base.InitializationComplete();
        }

        /// <summary>
        /// Gets the unique id.
        /// </summary>
        public override string UniqueId
        {
            get
            {
                return uniqueId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            CamRoleChangeMonitor.RoleChanged -= OnRoleChanged;
        }

        private void OnRoleChanged(object sender, CamRoleChangeMonitorEventArgs e)
        {
            base.OnChanged(null);
        }

        /// <summary>
        /// The method to call when a role has changed in Cam.  This method will cause all cached users to become expired.
        /// </summary>
        public static void Changed()
        {
            if (RoleChanged != null)
            {
                // Raise shared event to notify all subscribers
                RoleChanged(null, new CamRoleChangeMonitorEventArgs());
            }
        }
    }

    /// <summary>
    /// The event args.
    /// </summary>
    public class CamRoleChangeMonitorEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public CamRoleChangeMonitorEventArgs() { }
    }
}