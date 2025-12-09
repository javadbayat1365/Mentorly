using Carter;
using Elastic.Clients.Elasticsearch;
using Mentorly.SearchService.Entities;

namespace Mentorly.SearchService.Endpoints;

public class DeleteUserProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/Delete", async (string userId, ElasticsearchClient client) =>
        {
            var searchResult = await client.SearchAsync<UserProfileEntityModel>(s =>
                s.Indices(UserProfileEntityModel.IndexName)
                    .Query(q =>
                        q.Match(m => m.Field(f => f.UserId)
                            .Query(userId))));

            var hit = searchResult.Hits.FirstOrDefault();

            if (hit is not null)
                await client.DeleteAsync<UserProfileEntityModel>(hit.Id, descriptor => descriptor.Index(UserProfileEntityModel.IndexName));
        });
    }
}
