using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Search.Models;

namespace ECA.Business.Search
{
    public interface IIndexService
    {
        //Task CreateIndexAsync(IDocumentable documentable);

        //void CreateIndex(IDocumentable documentable);

        IDocumentConfiguration GetDocumentConfiguration<T>();

        void CreateIndex<T>() where T : class;

        Task CreateIndexAsync<T>() where T : class;


        Task DeleteIndexAsync(DocumentType documentType);

        void DeleteIndex(DocumentType documentType);


        //Task<Document> GetDocumentByIdAsync(DocumentType documentType, int id);

        //Document GetDocumentById(DocumentType documentType, int id);

        //Task<IndexGetStatisticsResponse> GetStatsAsync(DocumentType documentType);

        //IndexGetStatisticsResponse GetStats(DocumentType documentType);

        Task<DocumentIndexResponse> HandleDocumentsAsync<T>(List<T> documents) where T : class;

        DocumentIndexResponse HandleDocuments<T>(List<T> documents) where T : class;

        //Task<List<DocumentIndexResponse>> HandleDocumentsAsync(List<IDocumentable> documents);

        //List<DocumentIndexResponse> HandleDocuments(List<IDocumentable> documents);

        Task<DocumentSearchResponse> SearchAsync(DocumentType documentType, string search, List<DocumentKey> allowedDocumentKeys);

        DocumentSearchResponse Search(DocumentType documentType, string search, List<DocumentKey> allowedDocumentKeys);
        
    }
}