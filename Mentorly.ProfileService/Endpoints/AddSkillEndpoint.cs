using Carter;
using Mentorly.ProfileService.EntityModels;
using MongoDB.Driver;

namespace Mentorly.ProfileService.Endpoints
{
    public class AddSkillEndpoint : ICarterModule
    {
        public class AddSkillRequest
        {
            public string  Name { get; set; }
            public int ProficiencyLevel { get; set; }
        }
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //app.MapPost("/AddSkill", async (string userId,AddSkillRequest model,IMongoDatabase db) => {
            //    var collection = db.GetCollection<ProfileEntity>(ProfileEntity.CollectionName);
            //    var filter = Builders<ProfileEntity>.Filter.Eq(p => p.UserId,userId);
            //    var update = Builders<ProfileEntity>.Update.Push(s => s.Skills,new ProfileEntity.Skill(){ 
            //     Id = Guid.NewGuid(),
            //     Name = model.Name,
            //     ProficiencyLevel = 1
            //    });


            //});
        }
    }
}
