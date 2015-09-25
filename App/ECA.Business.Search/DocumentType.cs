using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public class DocumentType
    {
        private const string ALL_DOCUMENTS_INDEX_NAME = "ecadocs";

        private const int PROGRAM_DOCUMENT_TYPE_ID = 1;
        private const string PROGRAM_DOCUMENT_TYPE_NAME = "Program";
        private const string PROGRAM_INDEX_NAME = ALL_DOCUMENTS_INDEX_NAME;

        private const int PROJECT_DOCUMENT_TYPE_ID = 2;
        private const string PROJECT_DOCUMENT_TYPE_NAME = "Project";
        private const string PROJECT_INDEX_NAME = ALL_DOCUMENTS_INDEX_NAME;
        public static DocumentType Program
        {
            get
            {
                return new DocumentType(
                    PROGRAM_DOCUMENT_TYPE_ID,
                    PROGRAM_DOCUMENT_TYPE_NAME,
                    PROGRAM_INDEX_NAME);
            }
        }

        public static DocumentType Project
        {
            get
            {
                return new DocumentType(PROJECT_DOCUMENT_TYPE_ID, PROJECT_DOCUMENT_TYPE_NAME, PROJECT_INDEX_NAME);
            }
        }

        private DocumentType(int id, string name, string indexName)
        {
            this.Id = id;
            this.Name = name;
            this.IndexName = indexName;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string IndexName { get; private set; }

        /// <summary>
        /// Returns a string of this document type.
        /// </summary>
        /// <returns>A string of this comparison type.</returns>
        public override string ToString()
        {
            return this.Name;
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
            var otherType = obj as DocumentType;
            if (otherType == null)
            {
                return false;
            }
            return this.Id == otherType.Id;

        }

        /// <summary>
        /// Returns a hash of this object.
        /// </summary>
        /// <returns>A hash of this object.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Returns true if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>True if the given instances are equal.</returns>
        public static bool operator ==(DocumentType a, DocumentType b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Id == b.Id;
        }

        /// <summary>
        /// Returns false if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>False if the given instances are equal.</returns>
        public static bool operator !=(DocumentType a, DocumentType b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a comparison type whose values is equal to the given string.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>The document type type.</returns>
        public static DocumentType ToDocumentType(int documentTypeId)
        {
            var dictionary = new Dictionary<int, DocumentType>();
            dictionary.Add(Program.Id, DocumentType.Program);
            dictionary.Add(Project.Id, DocumentType.Project);
            Contract.Assert(dictionary.ContainsKey(documentTypeId), String.Format("The document type id [{0}] is not recognized.", documentTypeId));
            return dictionary[documentTypeId];
        }
    }
}
