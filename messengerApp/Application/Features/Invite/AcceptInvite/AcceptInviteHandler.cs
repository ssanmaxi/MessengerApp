using messengerApp.Application.Interfaces;

namespace messengerApp.Application.Features.Invite.AcceptInvite;

public class AcceptInviteHandler
{
    private readonly IInviteRepository _ir;
    private readonly ICurrentUser _cu;
    private readonly IUnitOfWork _uow;
    private readonly ILobbyMemberRepository _lmr;
    private readonly ILogger<AcceptInviteHandler> _logger;

    public AcceptInviteHandler(IInviteRepository ir, ICurrentUser cu, IUnitOfWork uow, ILobbyMemberRepository lmr, ILogger<AcceptInviteHandler> logger)
    {
        _ir = ir;
        _cu = cu;
        _uow = uow;
        _lmr = lmr;
        _logger = logger;
    }
    
    public async Task<string> Handle(AcceptInviteCommand command)
    {
        _logger.LogInformation("Handling Accept Invite");
        var userId = _cu.UserId;
        var invite = await _ir.GetByTokenAsync(command.Token);

        if (invite is null) throw new Exception("Invite not found");
        _logger.LogError("Invite was not found!");
        if (invite.Revoked) throw new Exception("Invite is revoked");
        _logger.LogError("Invite was revoked!");
        //if (DateTime.UtcNow >= invite.ExpiresAt) throw new Exception("Invite is expired");

        var isMember = await _lmr.IsMemberAsync(invite.LobbyId, userId);
        if (!isMember) await _lmr.AddAsync(invite.LobbyId, userId);
        _logger.LogInformation("User is in lobby!");
        //await _ir.DeleteByTokenAsync(command.Token);
        await _uow.SaveChangesAsync();

        return invite.LobbyId;
    }
}