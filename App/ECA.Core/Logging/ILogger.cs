using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Logging
{
    /// <summary>
    /// An ILogger is a simple interface to define a logging class that can log messages in the system.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The information message.</param>
        void Information(string message); 

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The variables to output in the formatted string.</param>
        void Information(string fmt, params object[] vars); 

        /// <summary>
        /// Logs an information exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The variables to output in the formatted string.</param>
        void Information(Exception exception, string fmt, params object[] vars); 
        
        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message.</param>
        void Warning(string message); 

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The objects to log.</param>
        void Warning(string fmt, params object[] vars);

        /// <summary>
        /// Logs a warning exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The variables to output in the formatted string.</param>
        void Warning(Exception exception, string fmt, params object[] vars); 
        
        /// <summary>
        /// Logs an Error.
        /// </summary>
        /// <param name="message">The error.</param>
        void Error(string message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The objects to log.</param>
        void Error(string fmt, params object[] vars);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="fmt">The string to format output with.</param>
        /// <param name="vars">The variables to output in the formatted string.</param>
        void Error(Exception exception, string fmt, params object[] vars); 

        /// <summary>
        /// Logs a trace method from a component.
        /// </summary>
        /// <param name="componentName">The name of the component.  Typically this value is found with typeof(Object).FullName.</param>
        /// <param name="timespan">The timespan the method required to execute.</param>
        /// <param name="method">The name of the method.  This value is set automatically.</param>
        void TraceApi(string componentName, TimeSpan timespan, [CallerMemberName] string method = "");

        /// <summary>
        /// Logs a trace method from a component.
        /// </summary>
        /// <param name="parameters">The parameters that were given to the method.</param>
        /// <param name="componentName">The name of the component.  Typically this value is found with typeof(Object).FullName.</param>
        /// <param name="timespan">The timespan the method required to execute.</param>
        /// <param name="method">The name of the method.  This value is set automatically.</param>
        void TraceApi(string componentName, TimeSpan timespan, IDictionary<string, object> parameters, [CallerMemberName] string method = "");
    }
}
