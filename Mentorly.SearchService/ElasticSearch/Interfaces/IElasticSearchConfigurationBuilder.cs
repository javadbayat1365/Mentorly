using Elastic.Clients.Elasticsearch;

namespace Mentorly.SearchService.ElasticSearch.Interfaces
{
    public interface IElasticSearchConfigurationBuilder
    {
        Task ConfigureAsync(ElasticsearchClient client);
    }
}
