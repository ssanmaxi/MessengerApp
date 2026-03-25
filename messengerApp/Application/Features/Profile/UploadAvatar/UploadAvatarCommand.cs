
namespace messengerApp.Application.Profile.UploadAvatar;

public record UploadAvatarCommand(int UserId, IFormFile File);
