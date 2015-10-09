﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{

    public enum DocumentKeyType
    {
        String,
        Int,
        Long,
        Guid
    }

    public class DocumentKey
    {
        public const char DOCUMENT_KEY_SPLIT_CHAR = '_';

        public DocumentKey(string key)
        {
            Contract.Requires(key != null, "The key must not be null.");
            ParseKey(key);
        }

        public DocumentKey(Guid documentTypeId, object id)
        {
            Contract.Requires(id != null, "The id must not be null.");
            this.DocumentTypeId = documentTypeId;
            this.Value = id;
            this.KeyType = GetDocumentKeyType(id);
        }

        public DocumentKeyType KeyType { get; set; }

        public Guid DocumentTypeId { get; set; }

        public object Value { get; set; }

        public DocumentKeyType GetDocumentKeyType(object id)
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
    }
}