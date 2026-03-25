using messengerApp.Application.DTO;
using messengerApp.Application.Interfaces;
namespace messengerApp.Application.Profile.GetProfileByUserId;

public class GetProfileByUserIdHandler
{
    private readonly IUserRepository _ur;
    private readonly ILogger<GetProfileByUserIdHandler> _logger;

    public GetProfileByUserIdHandler(IUserRepository ur, ILogger<GetProfileByUserIdHandler> logger)
    {
        _ur = ur;
        _logger = logger;
    }

    public async Task<ProfileDTO> Handle(int UserId)
    {
        _logger.LogInformation("Searching for a profile by user id");
        var user = await _ur.GetByIDAsync(UserId);
        if (user == null || user.Profile == null)
        {
            _logger.LogError("Profile was not found!");
            throw new Exception("Profile not found");
        }

        if (user.Profile.AvatarUrl == null)
        {
            return new ProfileDTO
            {
                ProfileName = user.Profile.Name,
                Age = user.Profile.Age,
                AvatarUrl = "/avatars/default_pfp.png"
            };
        }
        
        return new ProfileDTO
        {
            ProfileName = user.Profile.Name,
            Age = user.Profile.Age,
            AvatarUrl = user.Profile.AvatarUrl
        };
    }
}