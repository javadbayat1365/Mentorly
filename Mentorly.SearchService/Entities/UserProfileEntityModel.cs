using Elastic.Clients.Elasticsearch;
using Mentorly.SearchService.ElasticSearch.Interfaces;

namespace Mentorly.SearchService.Entities
{
    public class UserProfileEntityModel
    {
        public const string IndexName = "UserProfile";
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public string[] Skills { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }

    public class UserProfileEntityConfiguration : IElasticSearchConfigurationBuilder
    {
        public async Task ConfigureAsync(ElasticsearchClient client)
        {
            await client.Indices.CreateAsync(UserProfileEntityModel.IndexName,x => 
            x.Mappings<UserProfileEntityModel>(m => m
            .Properties(ps => ps.Text(t => t.FullName))//Text => vector search
            .Properties(ps => ps.Text(t => t.Bio))
            .Properties(ps => ps.Keyword(k => k.Email))//Keyword =>exact search
            .Properties(ps => ps.Keyword(k => k.Skills))
            ));
        }
    }
}
