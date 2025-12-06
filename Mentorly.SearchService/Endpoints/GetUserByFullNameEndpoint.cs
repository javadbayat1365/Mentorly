using Carter;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Mentorly.SearchService.Entities;

namespace Mentorly.SearchService.Endpoints;

public class GetUserByFullNameEndpoint : ICarterModule
{
    class GetUserByFullNameResponse
    {
        public string FullName { get; set; }
        public string[] Skills { get; set; }
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

        app.MapGet("/GetProfileBySkills", async (GetUserBySkillsRequest model, ElasticsearchClient client) =>
        {
            var searchResult = await client.SearchAsync<UserProfileEntityModel>(s =>
            {

                s.Indices(UserProfileEntityModel.IndexName)
                 .Query(q => q.Terms(m => m.Field(f => f.Skills)).Terms(new TermsQueryField(model.Skill)));

                //just these two properties 
                s.Source(x => x.Filter(f => f.Includes(
                    i => i.FullName, i => i.Skills
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

        

    }
    public class GetUserBySkillsRequest
    {
        public string Skill { get; set; }
    }
}
