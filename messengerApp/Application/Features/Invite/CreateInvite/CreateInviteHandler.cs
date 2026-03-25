using messengerApp.Application.Interfaces;

namespace messengerApp.Application.Features.Invite.CreateInvite;

public class CreateInviteHandler
{
    private readonly ICurrentUser _cu;
    private readonly ILobbyMemberRepository _lmr;
    private readonly IInviteRepository _ir;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CreateInviteHandler> _logger;

    public CreateInviteHandler(ICurrentUser cu, ILobbyMemberRepository lmr, IInviteRepository ir, IUnitOfWork uow, ILogger<CreateInviteHandler> logger)
    {
        _cu = cu;
        _lmr = lmr;
        _ir = ir;
        _uow = uow;
        _logger = logger;
    }

    public async Task<string> Handle(CreateInviteCommand command)
    {
        _logger.LogInformation("Handling Create Invite");
        var userId = _cu.UserId;

        var isMember = await _lmr.IsMemberAsync(command.LobbyId, userId);
        _logger.LogInformation("Checking if the user is a member of the lobby");
        if (!isMember) throw new Exception("You are not a member of this lobby");
        _logger.LogError("User is not member of this lobby!");
        
        var token = Guid.NewGuid().ToString("N"); 
        var expiresAt = DateTime.UtcNow.AddHours(24);

        var invite = new messengerApp.Domain.Entities.Invite
        {
            Token = token,
            LobbyId = command.LobbyId,
            ExpiresAt = expiresAt,
            Revoked = false
        };

        await _ir.AddAsync(invite);
        await _uow.SaveChangesAsync();
        
        return $"http://127.0.0.1:5500/index.html?invite={token}";
    }
}