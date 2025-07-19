using Mentorly.ProfileService.MongoDb.Configuration;
using MongoDB.Driver;

namespace Mentorly.ProfileService.Extensions
{
    public static class MongoDbConfigurationExtensions
    {
        public static WebApplicationBuilder ConfigureMongoDb(this WebApplicationBuilder builder)
        {
            var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));

            var mongoDatabase = mongoClient.GetDatabase("Mentorly-ProfileService");

            builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);
            return builder;
        }

        public static WebApplicationBuilder ConfigureMongoDbEntities(this WebApplicationBuilder builder)
        {
            var configurationTypeServices = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IMongoDbEntityConfiguration).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
                .Select(x => new ServiceDescriptor(typeof(IMongoDbEntityConfiguration), x, ServiceLifetime.Singleton));

            foreach (var type in configurationTypeServices)
            {
                builder.Services.AddSingleton(type);
            }

            return builder;
        }

        public static async Task UseMongoDbEntitiesAsync(this WebApplication app)
        {
            var mongoDb = app.Services.GetRequiredService<IMongoDatabase>();
            var configurations = app.Services.GetServices<IMongoDbEntityConfiguration>();

            foreach (var configuration in configurations)
            {
                await configuration.ConfigureAsync(mongoDb);
            }
        }
    }
}
