using Mentorly.ProfileService.SearchServices.ApiModels;
using Refit;

namespace Mentorly.ProfileService.SearchServices.Interfaces;

public interface ISearchService
{
    [Post("/CreateUserProfile")]
    Task CreateUserProfileAsync(AddUserProfileSearchApiModel model);
}
