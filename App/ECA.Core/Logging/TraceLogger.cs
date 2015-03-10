using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ECA.Core.Logging
{
    /// <summary>
    /// A simple ILogger that logs to the Trace object.  This is intended to be used with Azure.
    /// </summary>
    public class TraceLogger : ILogger
    {
        #region Information

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The information message.</param>
        public void Information(string message)
        {
            Trace.TraceInformation(message);
        }

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The variables to output in the formatted string.</param>
        public void Information(string fmt, params object[] vars)
        {
            Trace.TraceInformation(fmt, vars);
        }

        /// <summary>
        /// Logs an information exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The variables to output in the formatted string.</param>
        public void Information(Exception exception, string fmt, params object[] vars)
        {
            var message = String.Format(fmt, vars);
            Trace.TraceInformation(string.Format(fmt, vars) + ";Exception Details={0}", exception.ToString());
        }

        #endregion

        #region Warnging

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message.</param>
        public void Warning(string message)
        {
            Trace.TraceWarning(message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The objects to log.</param>
        public void Warning(string fmt, params object[] vars)
        {
            Trace.TraceWarning(fmt, vars);
        }

        /// <summary>
        /// Logs a warning exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The variables to output in the formatted string.</param>
        public void Warning(Exception exception, string fmt, params object[] vars)
        {
            var message = String.Format(fmt, vars);
            Trace.TraceWarning(string.Format(fmt, vars) + ";Exception Details={0}", exception.ToString());
        }

        #endregion

        #region Error

        /// <summary>
        /// Logs an Error.
        /// </summary>
        /// <param name="message">The error.</param>
        public void Error(string message)
        {
            Trace.TraceError(message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The objects to log.</param>
        public void Error(string fmt, params object[] vars)
        {
            Trace.TraceError(fmt, vars);
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The variables to output in the formatted string.</param>
        public void Error(Exception exception, string fmt, params object[] vars)
        {
            var message = String.Format(fmt, vars);
            Trace.TraceError(string.Format(fmt, vars) + ";Exception Details={0}", exception.ToString());
        }

        #endregion

        #region Trace


        /// <summary>
        /// Logs a trace method from a component.
        /// </summary>
        /// <param name="componentName">The name of the component.  Typically this value is found with typeof(Object).FullName.</param>
        /// <param name="timespan">The timespan the method required to execute.</param>
        /// <param name="method">The name of the method.  This value is set automatically.</param>
        public void TraceApi(string componentName, TimeSpan timespan, [CallerMemberName] string method = "")
        {
            string message = String.Format("{0}.{1} Elapsed:  {2}", componentName, method, timespan.ToString());
            Trace.TraceInformation(message);
        }


        /// <summary>
        /// Logs a trace method from a component.
        /// </summary>
        /// <param name="parameters">The parameters that were given to the method.</param>
        /// <param name="componentName">The name of the component.  Typically this value is found with typeof(Object).FullName.</param>
        /// <param name="timespan">The timespan the method required to execute.</param>
        /// <param name="method">The name of the method.  This value is set automatically.</param>
        public void TraceApi(string componentName, TimeSpan timespan, IDictionary<string, object> parameters, [CallerMemberName] string method = "")
        {
            var sb = new StringBuilder();
            foreach(var p in parameters)
            {
                sb.AppendFormat("[{0}:  ({1})],", p.Key, p.Value);
            }
            string message = String.Format("{0}.{1} Elapsed:  {2},  Parameters:  ({3})", componentName, method, timespan.ToString(), sb.ToString().TrimEnd(','));
            Trace.TraceInformation(message);
        }
        #endregion
    }
}
