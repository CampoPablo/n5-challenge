using System;
using System.Linq.Expressions; 
using Nest;

namespace n5.Infrastructure.ElasticSearch;

public class ElasticSearchService : IElasticSearchService
{
    private readonly ElasticClient _elasticClient;

    public ElasticSearchService(string elasticsearchUri)
    {
        var uri = new Uri(elasticsearchUri);
        var connectionSettings = new ConnectionSettings(uri).DefaultIndex("n5-indice"); 
        _elasticClient = new ElasticClient(connectionSettings);
    }
    public async Task<bool> IndexExistsAsync(string indexName)
    {
        var indexExistsResponse = await _elasticClient.Indices.ExistsAsync(indexName);
        return indexExistsResponse.Exists;
    }

    public async Task<bool> CreateIndexAsync(string indexName)
    {
        var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName);
        return createIndexResponse.IsValid;
    }

    public async Task<bool> DeleteIndexAsync(string indexName)
    {
        var deleteIndexResponse = await _elasticClient.Indices.DeleteAsync(indexName);
        return deleteIndexResponse.IsValid;
    }

    public async Task<bool> DeleteDocumentAsync(string indexName, string documentId)
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetDocumentAsync<T>(string indexName, string documentId) where T : class
    {
        var getResponse = await _elasticClient.GetAsync<T>(documentId, g => g.Index(indexName));
        return getResponse.Source;
    }

    public async Task<bool> IndexDocumentAsync<T>(string indexName, string documentId, T document) where T : class
    {
        if (!await IndexExistsAsync(indexName))
        {
            await CreateIndexAsync(indexName);
        }

        var indexResponse = await _elasticClient.IndexAsync(document, idx => idx
            .Index(indexName)
            .Id(documentId));

        return indexResponse.IsValid;
    }

    public async Task<bool> UpdateDocumentAsync<T>(string indexName, string documentId, T document) where T : class
    {
        var updateResponse = await _elasticClient.UpdateAsync<T>(documentId, u => u
            .Index(indexName)
            .Doc(document));

        return updateResponse.IsValid;
    }   
}
