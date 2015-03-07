using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Logging
{
    public class TraceLogger : ILogger
    {
        #region Information

        public void Information(string message)
        {
            Trace.TraceInformation(message);
        }

        public void Information(string format, params object[] vars)
        {
            Trace.TraceInformation(format, vars);
        }

        public void Information(Exception exception, string format, params object[] vars)
        {
            var message = String.Format(format, vars);
            Trace.TraceInformation(string.Format(format, vars) + ";Exception Details={0}", exception.ToString());
        }

        #endregion

        #region Warnging

        public void Warning(string message)
        {
            Trace.TraceWarning(message);
        }

        public void Warning(string format, params object[] vars)
        {
            Trace.TraceWarning(format, vars);
        }

        public void Warning(Exception exception, string format, params object[] vars)
        {
            var message = String.Format(format, vars);
            Trace.TraceWarning(string.Format(format, vars) + ";Exception Details={0}", exception.ToString());
        }

        #endregion

        #region Error

        public void Error(string message)
        {
            Trace.TraceError(message);
        }

        public void Error(string format, params object[] vars)
        {
            Trace.TraceError(format, vars);
        }

        public void Error(Exception exception, string format, params object[] vars)
        {
            var message = String.Format(format, vars);
            Trace.TraceError(string.Format(format, vars) + ";Exception Details={0}", exception.ToString());
        }

        #endregion

        #region Trace

        public void TraceApi(string componentName, TimeSpan timespan, [CallerMemberName] string method = "")
        {
            string message = String.Format("{0}.{1} Elapsed:  {2}", componentName, method, timespan.ToString());
            Trace.TraceInformation(message);
        }

        public void TraceApi(string componentName, TimeSpan timespan, IDictionary<string, object> parameters, [CallerMemberName] string method = "")
        {
            var sb = new StringBuilder();
            foreach(var p in parameters)
            {
                sb.AppendFormat("[{0}:  ({1})],", p.Key, p.Value);
            }
            string message = String.Format("{0}.{1} Elapsed:  {2},  Parameters:  ({3})", componentName, method, timespan.ToString(), sb.ToString());
            Trace.TraceInformation(message);
        }
        #endregion
    }
}
