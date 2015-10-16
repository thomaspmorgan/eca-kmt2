using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// The DocumentKeyType details the object type of the document id.
    /// </summary>
    public enum DocumentKeyType
    {
        /// <summary>
        /// A String key.
        /// </summary>
        String,
        /// <summary>
        /// An integer key.
        /// </summary>
        Int,

        /// <summary>
        /// An Int64, or Long, key.
        /// </summary>
        Long,

        /// <summary>
        /// A guid key.
        /// </summary>
        Guid
    }

    /// <summary>
    /// The DocumentKey is an object used to keep enough details about an entity id to be parsed later when a document is returned from the search service.  It keeps the value of the
    /// Id and the type of the key.
    /// </summary>
    public class DocumentKey
    {
        /// <summary>
        /// The split character between different segments of a document key.
        /// </summary>
        public const char DOCUMENT_KEY_SPLIT_CHAR = '_';

        /// <summary>
        /// Creates a new DocumentKey instance by parsing the given string.
        /// </summary>
        /// <param name="key">The document key as a string, usually created by callling ToString on another DocumentKey instance.</param>
        public DocumentKey(string key)
        {
            Contract.Requires(key != null, "The key must not be null.");
            ParseKey(key);
        }

        /// <summary>
        /// Creates and initializes a new DocumentKey with the given parameters.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <param name="id">The id.</param>
        public DocumentKey(Guid documentTypeId, object id)
        {
            Contract.Requires(id != null, "The id must not be null.");
            this.DocumentTypeId = documentTypeId;
            this.Value = id;
            this.KeyType = GetDocumentKeyType(id);
        }

        /// <summary>
        /// Gets or sets the document key type.
        /// </summary>
        public DocumentKeyType KeyType { get; set; }

        /// <summary>
        /// Gets or sets the document type id.
        /// </summary>
        public Guid DocumentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the id value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Returns the document key type given id value.
        /// </summary>
        /// <param name="id">The id to retrieve the document key type for.</param>
        /// <returns>The document key type.</returns>
        public static DocumentKeyType GetDocumentKeyType(object id)
        {
            var idType = id.GetType();
            var dictionary = new Dictionary<Type, DocumentKeyType>();
            dictionary.Add(typeof(int), DocumentKeyType.Int);
            dictionary.Add(typeof(long), DocumentKeyType.Long);
            dictionary.Add(typeof(string), DocumentKeyType.String);
            dictionary.Add(typeof(Guid), DocumentKeyType.Guid);
            if (!dictionary.ContainsKey(idType))
            {
                throw new NotSupportedException("The id type is not supported.");
            }
            return dictionary[idType];
        }

        /// <summary>
        /// Returns the formatted document key string.  Use this string when the key must be parsed later.
        /// </summary>
        /// <returns>The document key as a string.</returns>
        public override string ToString()
        {
            return string.Format("{0}{1}{2}{1}{3}",
                this.DocumentTypeId,
                DOCUMENT_KEY_SPLIT_CHAR,
                this.Value.ToString(),
                (int)this.KeyType
                );
        }

        private void ParseKey(string key)
        {
            var splitStrings = key.Split(new char[] { DOCUMENT_KEY_SPLIT_CHAR }, StringSplitOptions.RemoveEmptyEntries);
            var documentTypeIdString = splitStrings[0];
            var idString = splitStrings[1];
            var keyTypeString = splitStrings[2];

            var documentTypeId = Guid.Parse(documentTypeIdString);
            this.DocumentTypeId = documentTypeId;            

            var keyTypeId = Int32.Parse(keyTypeString);
            var isDocumentKeyType = Enum.IsDefined(typeof(DocumentKeyType), keyTypeId);
            
            if (!isDocumentKeyType)
            {
                throw new NotSupportedException(String.Format("The document key type id [{0}] is not supported.", keyTypeString));
            }
            else
            {
                var documentKeyType = Enum.Parse(typeof(DocumentKeyType), keyTypeString);
                this.KeyType = (DocumentKeyType)documentKeyType;
            }
            this.Value = ParseId(this.KeyType, idString);
        }

        /// <summary>
        /// Returns an object instance of the given id.
        /// </summary>
        /// <param name="keyType">The document key type.</param>
        /// <param name="idAsString">The id as a string.</param>
        /// <returns>The object key.</returns>
        public object ParseId(DocumentKeyType keyType, string idAsString)
        {
            if (keyType == DocumentKeyType.String)
            {
                return idAsString;
            }
            else if (keyType == DocumentKeyType.Int)
            {
                return Int32.Parse(idAsString);
            }
            else if (keyType == DocumentKeyType.Long)
            {
                return Int64.Parse(idAsString);
            }
            else if (keyType == DocumentKeyType.Guid)
            {
                return Guid.Parse(idAsString);
            }
            else
            {
                throw new NotSupportedException(String.Format("The document key type [{0}] is not supported.", keyType));
            }
        }


        /// <summary>
        /// Returns true if the given object equals this object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if the given object equals this object.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var otherKey = obj as DocumentKey;
            if (otherKey == null)
            {
                return false;
            }
            return this.DocumentTypeId == otherKey.DocumentTypeId
                && this.KeyType == otherKey.KeyType
                && this.Value.Equals(otherKey.Value);

        }

        /// <summary>
        /// Returns a hash of this object.
        /// </summary>
        /// <returns>A hash of this object.</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + this.KeyType.GetHashCode();
                hash = hash * 23 + this.DocumentTypeId.GetHashCode();
                hash = hash * 23 + this.Value.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>True if the given instances are equal.</returns>
        public static bool operator ==(DocumentKey a, DocumentKey b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.Equals(b);
        }

        /// <summary>
        /// Returns false if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>False if the given instances are equal.</returns>
        public static bool operator !=(DocumentKey a, DocumentKey b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a boolean value indicating whether the given string is a document key and if so, the document key is returned in the out
        /// parameter.
        /// </summary>
        /// <param name="keyAsString">The document key to parse.</param>
        /// <param name="value">The value of the document key to parse.</param>
        /// <returns>True, if the given value is a document key.</returns>
        public static bool TryParse(string value, out DocumentKey key)
        {
            try
            {
                var testKey = new DocumentKey(value);
                key = testKey;
                return true;
            }
            catch(Exception)
            {
                key = null;
                return false;
            }
        }
    }
}
