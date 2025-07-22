using Mentorly.ProfileService.EntityModels;
using Mentorly.ProfileService.MongoDb.Configuration;
using MongoDB.Driver;

namespace Mentorly.ProfileService.EntityConfigurations
{
    public class ProfileEntityConfiguration : IMongoDbEntityConfiguration
    {
        public async Task ConfigureAsync(IMongoDatabase database)
        {
            var indexModel = new CreateIndexModel<ProfileEntity>(Builders<ProfileEntity>.IndexKeys.Text(x => x.UserId));

            var colletion = database.GetCollection<ProfileEntity>(ProfileEntity.CollectionName);

            await colletion.Indexes.CreateOneAsync(indexModel);
        }
    }
}
