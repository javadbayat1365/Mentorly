using Carter;
using Mentorly.ProfileService.EntityModels;
using MongoDB.Bson;
using MongoDB.Driver;
using static Mentorly.ProfileService.EntityModels.ProfileEntity;

namespace Mentorly.ProfileService.Endpoints
{
    public class CreateProfileEndpoint:ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/profile", async (CreateProfileApiModel apiModel, IMongoDatabase db) => {
                var collection = db.GetCollection<ProfileEntity>(ProfileEntity.CollectionName);
                var existUserProfile = Builders<ProfileEntity>.Filter.Eq(x => x.UserId,apiModel.UserId);
                if (await collection.Find(existUserProfile).AnyAsync())
                    return Results.Problem("profile already exists",statusCode:StatusCodes.Status409Conflict);

                var entity = apiModel.ToEntity(apiModel.UserId);
                await collection.InsertOneAsync(entity);
                return Results.Created($"/profiles/{entity.Id}",entity);
            }).WithMetadata().WithName("CreateProfile").WithOpenApi();
        }

        public class CreateProfileApiModel
        {
            public string UserId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Bio { get; set; }
            public string TimeZone { get; set; }

            public List<SkillModel> Skills { get; set; } = [];
            public List<ExprienceModel> Expriences { get; set; } = [];
            public List<SocialLinkModel> SocialLinks { get; set; } = [];


            public class SkillModel
            {
                public string Name { get; set; }
                public int ProficiencyLevel { get; set; }
            }

            public class ExprienceModel
            {
                public string Title { get; set; }
                public string Company { get; set; }
                public string Description { get; set; }
                public DateTime From { get; set; }
                public DateTime To { get; set; }
            }

            public class SocialLinkModel
            {
                public string Platform { get; set; }
                public string Url { get; set; }
            }

            public ProfileEntity ToEntity(string userId)
            {
                return new ProfileEntity()
                {
                    Id = new ObjectId(),
                    UserId = userId,
                    Bio = this.Bio,
                    Email = this.Email,
                    FullName = this.FullName,
                    TimeZone = this.TimeZone,
                    Skills = this.Skills.Select(x => new Skill()
                    {
                        Name = x.Name,
                        ProficiencyLevel = x.ProficiencyLevel,
                    }).ToList(),
                    SocialLinks = this.SocialLinks.Select(s => new SocialLink() { 
                        Platform = s.Platform, 
                        Url = s.Url 
                    }).ToList(),
                    Expriences = this.Expriences.Select(x => new Exprience()
                    {
                        Title = x.Title,
                        Company = x.Company,
                        Description = x.Description,
                        From = x.From,
                        To = x.To,

                    }).ToList()


                };
            }
        }
    }
}
