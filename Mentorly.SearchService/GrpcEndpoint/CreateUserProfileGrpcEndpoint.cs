using Elastic.Clients.Elasticsearch;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mentorly.SearchService.Entities;
using Mentorly.SearchService.GrpcModels;

namespace Mentorly.SearchService.GrpcEndpoint;

public class CreateUserProfileGrpcEndpoint(ElasticsearchClient client) : UserProfileServices.UserProfileServicesBase
{
    public override async Task<Empty> CreateUserProfile(CreateUserSearchProfileModel request, ServerCallContext context)
    {
        var userProfile = new UserProfileEntityModel()
        {

            Bio = request.Bio,
            Email = request.Email,
            FullName = request.FullName,
            Skills = request.Skills.ToArray(),
            UserId = request.UserId,
        };

        await client.IndexAsync(userProfile,descriptor => descriptor.Index(UserProfileEntityModel.IndexName));
        return new Empty();
    }
}
