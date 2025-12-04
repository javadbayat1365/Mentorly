using Carter;
using Mentorly.ProfileService.EntityModels;
using MongoDB.Driver;

namespace Mentorly.ProfileService.Endpoints
{
    public class ProfileSummeryEndpoint : ICarterModule
    {
        
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/ProfileSummery/{userId}", async (IMongoDatabase db, string userId) =>
            {
                var collection = db.GetCollection<ProfileEntity>(ProfileEntity.CollectionName);
                var filter = Builders<ProfileEntity>.Filter.Eq(x => x.UserId, userId);

                var result = await collection.Find(filter)
                .Project(x => new ProfileSummeryRequest()
                {
                    Bio = x.Bio,
                    FullName = x.FullName,
                    ProfileId = x.Id.ToString(),
                    userId = x.UserId,
                    Skills = x.Skills.Select(s => s.Name).ToArray()
                }).FirstOrDefaultAsync();

                return result != null ? Results.Ok(result) : Results.NotFound();
            });
        }

        public class ProfileSummeryRequest
        {
            public string? ProfileId { get; set; }
            public string? FullName { get; set; }
            public string? Bio { get; set; }
            public string? userId { get; set; }
            public string[] Skills { get; set; }
        }
    }
}
