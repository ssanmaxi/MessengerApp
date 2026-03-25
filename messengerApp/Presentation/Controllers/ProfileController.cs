using messengerApp.Application.Profile.GetProfileByUserId;
using messengerApp.Application.Profile.UploadAvatar;
using Microsoft.AspNetCore.Mvc;
namespace messengerApp.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly GetProfileByUserIdHandler _ph;
    private readonly UploadAvatarHandler _ah;

    public ProfileController(GetProfileByUserIdHandler ph, UploadAvatarHandler ah)
    {
        _ph = ph;
        _ah = ah;
    }
    [HttpGet("{UserId}")]
    public async Task<IActionResult> ProfileUserId(int UserId)
    {
        var result = await _ph.Handle(UserId);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost("{UserId}/avatar")]
    public async Task<IActionResult> ProfileAvatarUrl(int UserId, [FromForm] IFormFile File)
    {
        var result = await _ah.Handle(new UploadAvatarCommand(UserId, File));

        return result != null ? Ok(result) : NotFound();
    }
}
