using Carter;
using Mentorly.ProfileService.EntityModels;
using MongoDB.Driver;

namespace Mentorly.ProfileService.Endpoints
{
    public class RemoveSkillsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/Profile/RemoveSkillRoute/{userId}/{skillId}", async (IMongoDatabase db, string userId, Guid skillId) => {
                var collection = db.GetCollection<ProfileEntity>(ProfileEntity.CollectionName);
                var filter = Builders<ProfileEntity>.Filter.Eq(x => x.UserId, userId);

                var update = Builders<ProfileEntity>.Update.PullFilter(x => x.Skills,s=>s.Id == skillId);

                await collection.UpdateOneAsync(filter, update);

                return Results.Ok();
            });
        }
    }
}
