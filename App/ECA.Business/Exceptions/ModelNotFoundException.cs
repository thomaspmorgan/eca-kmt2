using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Exceptions
{
    [Serializable]
    public class ModelNotFoundException : System.Exception
    {
        public ModelNotFoundException()
            : base() { }

        public ModelNotFoundException(string message)
            : base(message) { }

        public ModelNotFoundException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public ModelNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public ModelNotFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected ModelNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
