using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Query
{
    /// <summary>
    /// A CustomConverter for Json.Net to convert strings to a filter binding model.
    /// </summary>
    public class FilterBindingModelConverter : CustomCreationConverter<FilterBindingModel>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override FilterBindingModel Create(Type objectType)
        {
            return new FilterBindingModel();
        }



        /// <summary>
        /// Gets false.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var model = new FilterBindingModel();
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, jObject);
            using (JsonTextReader newReader = new JsonTextReader(new StringReader(writer.ToString())))
            {
                newReader.Culture = reader.Culture;
                newReader.DateParseHandling = reader.DateParseHandling;
                newReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
                newReader.FloatParseHandling = reader.FloatParseHandling;
                serializer.Populate(newReader, model);
            }

            if (model.Value is JArray)
            {
                model.Value = ToList(model.Value as JArray);
            }
            return model;

        }

        /// <summary>
        /// Returns an IList of objects based on the JTokenType in the array.  If the array of tokens are not all the same type
        /// an exception will be thrown.  If the token type is not supported an exception will be thrown.
        /// </summary>
        /// <param name="jArray">The JArray.</param>
        /// <returns>The list of objects to filter with.</returns>
        public IList ToList(JArray jArray)
        {
            Contract.Requires(jArray != null, "The jArray must not be null.");
            var tokenTypes = jArray.Select(x => x.Type).Distinct().ToList();
            if (tokenTypes.Count != 1)
            {
                throw new NotSupportedException("The filter token types must all be the same.");
            }
            var listDictionary = new Dictionary<JTokenType, Func<IList>>();
            listDictionary.Add(JTokenType.Boolean, () => jArray.ToObject<List<bool>>());
            listDictionary.Add(JTokenType.Date, () => jArray.ToObject<List<DateTime>>());
            listDictionary.Add(JTokenType.Float, () => jArray.ToObject<List<double>>());
            listDictionary.Add(JTokenType.Integer, () => jArray.ToObject<List<long>>());
            listDictionary.Add(JTokenType.String, () => jArray.ToObject<List<string>>());

            var tokenType = tokenTypes.First();
            if (!listDictionary.ContainsKey(tokenTypes.First()))
            {
                throw new NotSupportedException(String.Format("The json token type [{0}] is not a supported filter type.", tokenType));
            }
            return (IList)listDictionary[tokenType]();
        }
    }
}