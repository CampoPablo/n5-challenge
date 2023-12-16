namespace n5.Infrastructure.ElasticSearch;

public interface IElasticSearchService
{
    Task<bool> IndexDocumentAsync<T>(string indexName, string documentId, T document) where T : class;
    Task<T> GetDocumentAsync<T>(string indexName, string documentId) where T : class;
    Task<bool> UpdateDocumentAsync<T>(string indexName, string documentId, T document) where T : class;
    Task<bool> DeleteDocumentAsync(string indexName, string documentId);
    Task<bool> IndexExistsAsync(string indexName);
    Task<bool> CreateIndexAsync(string indexName);
    Task<bool> DeleteIndexAsync(string indexName);
}
