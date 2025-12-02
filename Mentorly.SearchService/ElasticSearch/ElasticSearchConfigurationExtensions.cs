using Elastic.Clients.Elasticsearch;
using Mentorly.SearchService.ElasticSearch.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace Mentorly.SearchService.ElasticSearch
{
    public static class ElasticSearchConfigurationExtensions
    {
        public static WebApplicationBuilder AddElasticSearch(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration.GetSection("ElasticSearch");
            var endpoint = configuration["Uri"];
            ArgumentException.ThrowIfNullOrEmpty(endpoint);

            var settings = new ElasticsearchClientSettings().PingTimeout(TimeSpan.FromSeconds(10));

            var elasticSearchClient = new ElasticsearchClient(settings);
            
            builder.Services.AddSingleton(elasticSearchClient);

            return builder;
        }

        public static WebApplicationBuilder AddElasticSearchConfigurations(this WebApplicationBuilder builder)
        {
            var configurationTypeServices = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IElasticSearchConfigurationBuilder).IsAssignableFrom(x)
                && x is { IsInterface: false, IsAbstract: false })
                .Select(x => new ServiceDescriptor(typeof(IElasticSearchConfigurationBuilder),x,ServiceLifetime.Singleton));

            foreach (var configurationTypeService in configurationTypeServices)
            {
                builder.Services.Add(configurationTypeService);
            }

            return builder;
        }

        public static async Task UseElasticSearchAsync(this WebApplication app)
        {
            var elasticSearch = app.Services.GetRequiredService<ElasticsearchClient>();

            var configurations = app.Services.GetServices<IElasticSearchConfigurationBuilder>();

            foreach (var configuration in configurations)
            {
                await configuration.ConfigureAsync(elasticSearch);
            }
        }
    }
}
