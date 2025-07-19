using MongoDB.Driver;

namespace Mentorly.ProfileService.MongoDb.Configuration
{
    public interface IMongoDbEntityConfiguration
    {
        Task ConfigureAsync(IMongoDatabase database);
    }
}
