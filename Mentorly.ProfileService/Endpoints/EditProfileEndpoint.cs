using Carter;
using Mentorly.ProfileService.EntityModels;
using MongoDB.Bson;
using MongoDB.Driver;
using static Mentorly.ProfileService.Endpoints.AddSkillEndpoint;
using static Mentorly.ProfileService.EntityModels.ProfileEntity;

namespace Mentorly.ProfileService.Endpoints;

public class EditProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/EditProfile", async (string userId, EditProfileApiModel model, IMongoDatabase db) =>
        {
            var collection = db.GetCollection<ProfileEntity>(ProfileEntity.CollectionName);
            var filter = Builders<ProfileEntity>.Filter.Eq(x => x.UserId, userId);
            var update = Builders<ProfileEntity>.Update.Push(x => x.Skills, new ProfileEntity()
            {
                Id = Guid.NewGuid(),
                Name = model.SkillName,
                ProficiencyLevel = model.ProficiencyLevel
            });

            var result = await collection.UpdateOneAsync(filter, update);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
                return Results.Ok();

            return Results.NotFound();
        });
    }

    public class EditProfileApiModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
    }
}
