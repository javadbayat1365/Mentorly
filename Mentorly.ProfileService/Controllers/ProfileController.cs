using Mentorly.ProfileService.EntityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static Mentorly.ProfileService.Endpoints.CreateProfileEndpoint;

namespace Mentorly.ProfileService.Controllers
{
    [NonController]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController(IMongoDatabase db) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateProfile1([FromQuery]string UserId)
        {
            var apiModel = new CreateProfileApiModel();
            var collection = db.GetCollection<ProfileEntity>(ProfileEntity.CollectionName);
            var existUserProfile = Builders<ProfileEntity>.Filter.Eq(x => x.UserId, UserId);
            if (await collection.Find(existUserProfile).AnyAsync())
                return BadRequest("profile already exists");

            var entity = apiModel.ToEntity(UserId);
            await collection.InsertOneAsync(entity);
            return Ok();
        }
    }
}
