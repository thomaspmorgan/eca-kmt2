﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Search.Models;

namespace ECA.Business.Search
{
    public interface IIndexService
    {
        IDocumentConfiguration GetDocumentConfiguration<T>();

        void CreateIndex<T>() where T : class;

        Task CreateIndexAsync<T>() where T : class;

        Task DeleteIndexAsync(DocumentType documentType);

        void DeleteIndex(DocumentType documentType);

        Task<ECADocument> GetDocumentByIdAsync(string key);

        ECADocument GetDocumentById(string key);

        Task<ECADocument> GetDocumentByIdAsync(DocumentKey key);

        ECADocument GetDocumentById(DocumentKey key);

        Task<DocumentIndexResponse> HandleDocumentsAsync<T>(List<T> documents) where T : class;

        DocumentIndexResponse HandleDocuments<T>(List<T> documents) where T : class;

        Task<DocumentSearchResponse<ECADocument>> SearchAsync(ECASearchParameters searchParameters, List<DocumentKey> allowedDocumentKeys);

        DocumentSearchResponse<ECADocument> Search(ECASearchParameters searchParameters, List<DocumentKey> allowedDocumentKeys);
        
    }
}