using Carter;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Mentorly.SearchService.Entities;

namespace Mentorly.SearchService.Endpoints;

public class GetUserEndpoint : ICarterModule
{
    class GetUserByFullNameResponse
    {
        public string FullName { get; set; }
        public string[] Skills { get; set; }
        public string UserId { get; set; } = null!;
    }
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetUserByFullName", async (string fullName, ElasticsearchClient client) =>
        {
            var searchResult = await client.SearchAsync<UserProfileEntityModel>(s =>
            {

                s.Indices(UserProfileEntityModel.IndexName)
                 .Query(q => q.Match(m => m.Field(f => f.FullName).Query(fullName)));

                //just these two properties 
                s.Source(x => x.Filter(f => f.Includes(
                    i => i.FullName,i => i.Skills
                    )));
            });

            if (searchResult.IsSuccess())
            {
                return Results.Ok(
                    searchResult.Documents.Select(x => new GetUserByFullNameResponse
                    {
                        FullName = x.FullName,
                        Skills = x.Skills
                    }).ToList()
                );
            }

            return Results.NotFound();
        });

        app.MapPost("/GetProfileBySkills", async (GetUserBySkillsRequest model, ElasticsearchClient client) =>
        {
            var searchResponse = await client.SearchAsync<UserProfileEntityModel>(s =>
            {
                s.Indices(UserProfileEntityModel.IndexName)
                    .Query(q =>
                        q.Terms(t =>
                            t.Field(f => f.Skills)
                                .Terms(new TermsQueryField(model.Skills.Select(FieldValue.String).ToList())))
                    );

                s.Source(src =>
                    src.Filter(f => f.Includes(
                        c => c.FullName, c => c.Skills, c => c.UserId)));
            });


            if (searchResponse.IsSuccess())
            {
                return Results.Ok(searchResponse.Documents.Select(c => new GetUserByFullNameResponse
                {
                    FullName = c.FullName,
                    Skills = c.Skills,
                    UserId = c.UserId
                }).ToList());
            }

            return Results.NotFound();
        });

        app.MapPost("/GetProfileByFullNameOrSkills",async (GetUserBySkillOrFullNamesRequest model, ElasticsearchClient client) =>
            {
                var searchResponse = await client.SearchAsync<UserProfileEntityModel>(s =>
                {
                    s.Indices(UserProfileEntityModel.IndexName)
                        .Query(q =>
                            q.Bool(b =>
                                b.Should(bs =>
                                        bs.Match(t => t.Field(m => m.FullName).Query(model.FullName)),
                                    bs => bs.Terms(t =>
                                        t.Field(f => f.Skills)
                                            .Terms(new TermsQueryField(model.Skills.Select(FieldValue.String).ToList())))
                        )));

                    s.Source(src =>
                        src.Filter(f => f.Includes(
                            c => c.FullName, c => c.Skills, c => c.UserId)));
                });


                if (searchResponse.IsSuccess())
                {
                    return Results.Ok(searchResponse.Documents.Select(c => new GetUserByFullNameResponse
                    {
                        FullName = c.FullName,
                        Skills = c.Skills,
                        UserId = c.UserId
                    }).ToList());
                }

                return Results.NotFound();
            });

        app.MapGet("/GetUserByBio", async (string bio, ElasticsearchClient client) =>
        {
            var searchResponse = await client.SearchAsync<UserProfileEntityModel>(s =>
            {
                s.Indices(UserProfileEntityModel.IndexName)
                    .Query(q =>
                        q.Match(m => m.Field(f => f.Bio)
                            .Query(bio)));

                s.Source(src =>
                    src.Filter(f => f.Includes(
                        c => c.FullName, c => c.Skills, c => c.UserId)));
            });


            if (searchResponse.IsSuccess())
            {
                return Results.Ok(searchResponse.Documents.Select(c => new GetUserByFullNameResponse
                {
                    FullName = c.FullName,
                    Skills = c.Skills,
                    UserId = c.UserId
                }).ToList());
            }

            return Results.NotFound();
        });

    }
    class GetUserBySkillsRequest
    {
        public string[] Skills { get; set; } = null!;
    }

    private class GetUserBySkillOrFullNamesRequest
    {
        public string FullName { get; set; }
        public string[] Skills { get; set; } = null!;
    }
}
