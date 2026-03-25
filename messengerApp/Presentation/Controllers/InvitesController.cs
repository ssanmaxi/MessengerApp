using messengerApp.Application.Features.Invite.AcceptInvite;
using messengerApp.Application.Features.Invite.CreateInvite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace messengerApp.Controllers;

[ApiController]
[Route("api/invites")]
public class InvitesController : ControllerBase
{
    private readonly AcceptInviteHandler _aih;
    private readonly CreateInviteHandler _cih;

    public InvitesController(AcceptInviteHandler aih, CreateInviteHandler cih)
    {
        _aih = aih;
        _cih = cih;
    }

    [Authorize]
    [HttpPost("accept")]
    public async Task<IActionResult> AcceptInvite([FromBody] AcceptInviteCommand command)
    {
        var lobbyId = await _aih.Handle(command);
        return Ok(new { lobbyId });
    }
    
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateInviteCommand command)
    {
        var link = await _cih.Handle(command);
        return Ok(new { link });
    }
    
    [AllowAnonymous]
    [HttpGet("accept/{token}")]
    public IActionResult AcceptInvite([FromRoute] string token)
    {
        return Redirect($"http://127.0.0.1:5500/index.html?invite={token}");
    }
}