using messengerApp.Application.Interfaces;
using messengerApp.Application.DTO;
using messengerApp.Domain.Entities;
namespace messengerApp.Application.Auth.Register;

public class RegisterHandler
{
    private readonly IUserRepository _ur;
    private readonly IPasswordHasher _ph;
    private readonly ITokenService _ts;

    public RegisterHandler(IUserRepository UserRepository, IPasswordHasher PasswordHasher, ITokenService TokenService)
    {
        _ur = UserRepository;
        _ph = PasswordHasher;
        _ts = TokenService; 
    }

    public async Task<UserResponseDTO> Handle(RegisterCommand command)
    {
        var existing = await _ur.GetByEmailAsync(command.Email);
        if (existing != null)
        {
            throw new Exception("Email already exists!");
        }

        var hash = _ph.Hash(command.Password);
        //Profile profile = new Domain.Entities.Profile { Name = command.ProfileName, Age = command.Age };
        var user = new User(command.Name, command.Email, hash);
        // {
        //     Name = command.Name,
        //     Email = command.Email,
        //     PasswordHash = hash,
        //     Profile = new Domain.Entities.Profile {Name = command.ProfileName, Age = command.Age}
        // };

        await _ur.AddAsync(user);
        await _ur.SaveChangesAsync();

        var saved = await _ur.GetByEmailAsync(command.Email);
        if (saved == null) throw new Exception("BAKA");
        
        var token = _ts.GenerateToken(user);

        return new UserResponseDTO
        {
            Id = saved.Id,
            Name = saved.Name,
            Email = saved.Email,
            Token = token,
            ProfileName = saved.Profile.Name,
            Age = saved.Profile.Age,
            AvatarUrl = saved.Profile.AvatarUrl
        };
    }
    
    public async Task<UserResponseDTO> Handle(RegisterCommand command, string Provider, string ProviderId)
    {
        var existing = await _ur.GetByEmailAsync(command.Email);
        if (existing != null)
        {
            throw new Exception("Email already exists!");
        }
        
        var user = User.CreateFromOauth(command.Name, command.Email, Provider, ProviderId);
        // {
        //     Name = command.Name,
        //     Email = command.Email,
        //     PasswordHash = hash,
        //     Profile = new Domain.Entities.Profile {Name = command.ProfileName, Age = command.Age},
        //     Provider = Provider,
        //     ProviderUserId = ProviderId
        // };

        await _ur.AddAsync(user);
        await _ur.SaveChangesAsync();

        var saved = await _ur.GetByEmailAsync(command.Email);
        if (saved == null) throw new Exception("BAKA");
        
        var token = _ts.GenerateToken(user);

        return new UserResponseDTO
        {
            Id = saved.Id,
            Name = saved.Name,
            Email = saved.Email,
            Token = token,
            ProfileName = saved.Profile.Name,
            Age = saved.Profile.Age,
            AvatarUrl = saved.Profile.AvatarUrl
        };
    }
}