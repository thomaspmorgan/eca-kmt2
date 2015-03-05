using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Logging
{
    public interface ILogger
    {
        void Information(string message); 
        void Information(string fmt, params object[] vars); 
        void Information(Exception exception, string fmt, params object[] vars); 
        
        void Warning(string message); 
        void Warning(string fmt, params object[] vars); 
        void Warning(Exception exception, string fmt, params object[] vars); 
        
        void Error(string message); 
        void Error(string fmt, params object[] vars); 
        void Error(Exception exception, string fmt, params object[] vars); 
        
        void TraceApi(string componentName, TimeSpan timespan, [CallerMemberName] string method = "");
        void TraceApi(string componentName, TimeSpan timespan, string properties, [CallerMemberName] string method = ""); 
        void TraceApi(string componentName, TimeSpan timespan, string fmt, [CallerMemberName] string method = "", params object[] vars);
    }
}
