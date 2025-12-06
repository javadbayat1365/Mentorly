using Carter;
using Mentorly.ProfileService.EntityModels;
using MongoDB.Driver;

namespace Mentorly.ProfileService.Endpoints
{
    public class EditProfileEndpoint : ICarterModule
    {
        public class EditProfileRequest
        {
            public string UserId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Bio { get; set; }
        }
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/Profile", async (IMongoDatabase db, EditProfileRequest model) =>
            {
                var collection = db.GetCollection<ProfileEntity>(ProfileEntity.CollectionName);
                var filter = Builders<ProfileEntity>.Filter.Eq(x => x.UserId, model.UserId);
                var update = Builders<ProfileEntity>.Update
                .Set(c => c.Email, model.Email)
                .Set(c => c.FullName, model.FullName)
                .Set(c => c.Bio, model.Bio);

                var updateResult = await collection.UpdateOneAsync(filter, update);
                if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
                    return Results.Ok();

                return Results.NotFound();
            });
        }
    }
}
