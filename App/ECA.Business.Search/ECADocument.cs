using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public class ECADocument : Document
    {   
        public const string ID_KEY = "id";
        public const string TITLE_KEY = "title";
        public const string SUBTITLE_KEY = "subtitle";
        public const string DESCRIPTION_KEY = "desc";
        public const string DOCUMENT_TYPE_ID_KEY = "typeId";

        public ECADocument(IDocumentable documentable)
        {
            Contract.Requires(documentable != null, "The documentable object must not be null.");
            this.Key = new DocumentKey(documentable);
            SetKeyValues(documentable);
            this.AdditionalFields = documentable.GetDocumentFields();
            this.DocumentType = documentable.GetDocumentType();
        }

        public IEnumerable<string> AdditionalFields { get; private set; }

        public DocumentKey Key { get; private set; }

        public DocumentType DocumentType { get; private set; }

        private void SetKeyValues(IDocumentable documentable)
        {
            var keys = documentable.GetDocumentFields();
            foreach (var key in keys)
            {
                this.Add(key, documentable.GetValue(key));
            }
            this.Add(ID_KEY, this.Key.ToString());
            this.Add(TITLE_KEY, documentable.GetTitle());
            this.Add(DESCRIPTION_KEY, documentable.GetDescription());
            this.Add(SUBTITLE_KEY, documentable.GetSubtitle());
            this.Add(DOCUMENT_TYPE_ID_KEY, (documentable.GetDocumentType().Id).ToString());
        }
        

        public Index GetIndex()
        {
            var index = new Index
            {
                Name = this.DocumentType.IndexName,

            };
            foreach (var field in AdditionalFields.ToList())
            {
                index.Fields.Add(new Field
                {
                    Name = field,
                    Type = DataType.String,
                    IsSearchable = true,
                });
            }
            index.Fields.Add(new Field
            {
                IsKey = true,
                Name = ECADocument.ID_KEY,
                Type = DataType.String
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.TITLE_KEY,
                Type = DataType.String,
                IsSearchable = false
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.SUBTITLE_KEY,
                Type = DataType.String,
                IsSearchable = false
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.DESCRIPTION_KEY,
                Type = DataType.String,
                IsSearchable = false
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.DOCUMENT_TYPE_ID_KEY,
                Type = DataType.String,
                IsSearchable = false,
                IsFacetable = true,
                IsFilterable = true
            });
            return index;
        }
    }
}
