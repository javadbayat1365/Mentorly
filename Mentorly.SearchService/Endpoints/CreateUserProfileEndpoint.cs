 using Carter;
using Elastic.Clients.Elasticsearch;
using Mentorly.SearchService.Entities;

namespace Mentorly.SearchService.Endpoints
{
    public class CreateUserProfileEndpoint : ICarterModule
    {
        public class CreateUserProfileIndexRequest
        {
            public string FullName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Bio { get; set; } = null!;
            public string[] Skills { get; set; }
        }
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/CreateUserProfile", async (CreateUserProfileIndexRequest model, ElasticsearchClient client) =>
            {
                await client.IndexAsync(new UserProfileEntityModel()
                {
                    Bio = model.Bio,
                    Email = model.Email,
                    Skills = model.Skills,
                    FullName = model.FullName,
                }, descriptor => descriptor.Index(UserProfileEntityModel.IndexName));

                return Results.Created();
            });
        }
    }
}
