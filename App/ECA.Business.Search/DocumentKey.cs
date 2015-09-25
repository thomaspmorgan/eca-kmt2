using System;
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
        public const char DOCUMENT_KEY_SPLIT_CHAR = '-';

        public DocumentKey(string key)
        {
            Contract.Requires(key != null, "The key must not be null.");
            ParseKey(key);
        }

        public DocumentKey(IDocumentable documentable) : this(documentable.GetDocumentType(), documentable.GetId())
        {
            Contract.Requires(documentable != null, "The documentable object must not be null.");
        }

        public DocumentKey(DocumentType documentType, object id)
        {
            Contract.Requires(id != null, "The id must not be null.");
            this.DocumentType = documentType;
            this.Value = id;
            this.KeyType = GetDocumentKeyType(id);
        }

        public DocumentKeyType KeyType { get; set; }

        public DocumentType DocumentType { get; set; }

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
                this.DocumentType.Id,
                DOCUMENT_KEY_SPLIT_CHAR,
                this.Value.ToString(),
                (int)this.KeyType
                );
        }

        private void ParseKey(string key)
        {
            var splitStrings = key.Split(new char[] { DOCUMENT_KEY_SPLIT_CHAR }, StringSplitOptions.RemoveEmptyEntries);
            var documentTypeString = splitStrings[0];
            var idString = splitStrings[1];
            var documentTypeIdString = splitStrings[2];

            var documentTypeId = Int32.Parse(documentTypeIdString);
            var documentType = DocumentType.ToDocumentType(documentTypeId);
            this.DocumentType = (DocumentType)documentType;

            var documentKeyType = Enum.Parse(typeof(DocumentKeyType), documentTypeIdString);
            if (documentKeyType == null)
            {
                throw new NotSupportedException(String.Format("The document key type id [{0}] is not supported.", documentTypeIdString));
            }
            else
            {
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
    }
}
