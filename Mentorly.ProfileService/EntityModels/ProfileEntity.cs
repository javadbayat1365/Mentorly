using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Mentorly.ProfileService.EntityModels
{
    public class ProfileEntity
    {
        [NotMapped]
        [BsonIgnore]
        public const string CollectionName="UserProfiles";

        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string TimeZone { get; set; }

        public List<Skill> Skills { get; set; } = [];
        public List<Exprience> Expriences { get; set; } = [];
        public int MyProperty { get; set; }
    }

    public class Skill
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ProficiencyLevel { get; set; }
    }

    public class Exprience
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id{ get; set; }
        public string Title{ get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    
    public class SocialLink
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
    }
}
