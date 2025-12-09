using Carter;
using Elastic.Clients.Elasticsearch;
using Mentorly.SearchService.Entities;

namespace Mentorly.SearchService.Endpoints;

public class UpdateUserProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("UpdateProfile", async (string userId, string email, ElasticsearchClient client) =>
        {
            SearchResponse<UserProfileEntityModel> userprofile =
                await client.SearchAsync<UserProfileEntityModel>(m => m.Query(q => q.Match(c =>
                    c.Field(x => x.Email).Query(email))));

            var hit = userprofile.Hits.FirstOrDefault();

            if (hit is null)
                return Results.NotFound();

            await client.UpdateAsync<UserProfileEntityModel, object>(
                index: UserProfileEntityModel.IndexName,
                id: hit.Id,
                descriptor => descriptor.Doc(new
                {
                    UserId = userId,
                }));

            return Results.Ok();

        });
    }
}
