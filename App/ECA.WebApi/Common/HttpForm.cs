using System.Collections.Generic;
using System.Net.Http;

namespace ECA.WebApi.Common
{
    public class HttpForm
    {
        private IList<KeyValuePair<string, string>> _fields = new List<KeyValuePair<string, string>>();

        public HttpForm Add(string name, string value)
        {
            _fields.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }

        public HttpContent Content
        {
            get
            {
                return new FormUrlEncodedContent(_fields);
            }
        }
    }
}