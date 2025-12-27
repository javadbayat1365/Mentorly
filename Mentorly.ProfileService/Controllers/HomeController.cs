using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Mentorly.ProfileService.EntityModels;
using Mentorly.ProfileService.SearchServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static Mentorly.ProfileService.Endpoints.CreateProfileEndpoint;

namespace Mentorly.ProfileService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController(IMongoDatabase db, ElasticsearchClient client, ISearchService searchService) : ControllerBase
    {
        [HttpPost]
        public async Task<IResult> Index(CreateProfileApiModel apiModel)
        {
            var collection = db.GetCollection<ProfileEntity>(ProfileEntity.CollectionName);
            var existUserProfile = Builders<ProfileEntity>.Filter.Eq(x => x.UserId, apiModel.UserId);
            if (await collection.Find(existUserProfile).AnyAsync())
                return Results.Problem("profile already exists", statusCode: StatusCodes.Status409Conflict);

            var entity = apiModel.ToEntity(apiModel.UserId);

            await searchService.CreateUserProfileAsync(new SearchServices.ApiModels.AddUserProfileSearchApiModel(
                  apiModel.FullName,
                  apiModel.Email,
                  apiModel.Bio,
                  apiModel.Skills.Select(s => s.Name).ToArray(),
                  apiModel.UserId
                 ));


            await collection.InsertOneAsync(entity);
            await client.IndexAsync(new
            {
                Bio = "model.Bio",
                Email = "model.Email",
                Skills = "model.Skills",
                FullName = "model.FullName",
            }, descriptor => descriptor.Index("UserProfile"));

            return Results.Created($"/profiles/{entity.Id}", entity);
        }
    }
}
