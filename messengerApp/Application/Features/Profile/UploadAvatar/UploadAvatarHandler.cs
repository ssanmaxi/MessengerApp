using messengerApp.Application.Interfaces;
using messengerApp.Application.DTO;

namespace messengerApp.Application.Profile.UploadAvatar;

public class UploadAvatarHandler
{
    private readonly IUserRepository _ur;
    private readonly IWebHostEnvironment _wer;
    private readonly ILogger<UploadAvatarHandler> _logger;

    public UploadAvatarHandler(IUserRepository ur, IWebHostEnvironment wer, ILogger<UploadAvatarHandler> logger)
    {
        _ur = ur;
        _wer = wer;
        _logger = logger;
    }

    public async Task<ProfileDTO> Handle(UploadAvatarCommand command)
    {
        _logger.LogInformation("Uploading avatar!");
        var user = await _ur.GetByIDAsync(command.UserId);

        if (user == null || user.Profile == null)
        {
            _logger.LogError("Profile was not found!");
            throw new Exception("Profile not found");
        }

        var avatarsDir = Path.Combine(_wer.WebRootPath, "avatars");
        Directory.CreateDirectory(avatarsDir);

        var ext = Path.GetExtension(command.File.FileName);
        var fileName = $"{command.UserId}_{Guid.NewGuid():N}{ext}";
        var savePath = Path.Combine(avatarsDir, fileName);

        await using (var stream = new FileStream(savePath, FileMode.Create))
        {
            await command.File.CopyToAsync(stream);
        }

        var url = $"/avatars/{fileName}";
        await _ur.SetAvatarUrlAsync(command.UserId, url);
        
        return new ProfileDTO
        {
            ProfileName = user.Profile.Name,
            Age = user.Profile.Age,
            AvatarUrl = url
        };
    }
}