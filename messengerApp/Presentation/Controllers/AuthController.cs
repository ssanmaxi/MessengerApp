using Microsoft.AspNetCore.Mvc;
using messengerApp.Application.Auth.Register;
using messengerApp.Application.Auth.Login;
using messengerApp.Application.Auth.IssueTokenForOtp;
using messengerApp.Application.Features.TwoFactorAuth;
using messengerApp.Application.Features.TwoFactorAuth.EnableTwoFactorConfirmed;
using messengerApp.Application.Features.TwoFactorAuth.EnableTwoFactorStart;
using messengerApp.Application.Interfaces;
using messengerApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using messengerApp.Presentation.Configurations;
using Microsoft.Extensions.Options;
using System.Text.Json;
using messengerApp.Domain.Entities;

namespace messengerApp.Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly RegisterHandler _rh;
    private readonly LoginHandler _lh;
    private readonly IssueTokenForOtpHandler _th;
    private readonly IOtpStore _os;
    private readonly CodeGenerateService _gs;
    private readonly IUserRepository _ur;
    private readonly EnableTFChandler _tfh;
    private readonly EnableTFShandler _tsh;
    private readonly ITotpService _ts;
    private readonly ILoginChallengeStore _lcs;
    private readonly ITokenService _its;
    private readonly GitHubOptions _opts;

    public AuthController(RegisterHandler rh, LoginHandler lh, IssueTokenForOtpHandler th, IOtpStore os, CodeGenerateService gs, IUserRepository ur, EnableTFChandler tfh, EnableTFShandler tsh, ITotpService ts, ILoginChallengeStore lcs, ITokenService its, IOptions<GitHubOptions> opts)
    {
        _rh = rh;
        _lh = lh;
        _th = th;
        _os = os;
        _gs = gs;
        _ur = ur;
        _tfh = tfh;
        _tsh = tsh;
        _ts = ts;
        _lcs = lcs;
        _its = its;
        _opts = opts.Value;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    { 
        var result = await _rh.Handle(command);
        return Ok(new{token = result.Token, userId = result.Id});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _lh.Handle(command);
        var user = await _ur.GetByEmailAsync(command.email);

        if (user == null) return BadRequest("User not found");

        if (user.TwoFactorEnabled)
        {
            var challengeId = Guid.NewGuid().ToString("N");
            await _lcs.SetAsync(challengeId, user.Id, TimeSpan.FromMinutes(5));
            return Ok(new { requires2fa = true, challengeId });
        }
        
        return Ok(new {token = result, userId = user.Id});
    }

    [HttpPost("login-2fa")]
    public async Task<IActionResult> Login2fa(Login2faCommand command)
    {
        var userId = await _lcs.GetAsync(command.challengeId);
        if (userId == null) return BadRequest("sgl");

        var user = await _ur.GetByIDAsync(userId.Value);
        if (user == null) return BadRequest("sgl");

        if (!user.TwoFactorEnabled || user.TwoFactorSecret == null) return BadRequest("sgl");

        var result = _ts.VerifyCode(user.TwoFactorSecret, command.code);

        if (!result) return BadRequest("sgl");

        await _lcs.DeleteAsync(command.challengeId);
        var token = _its.GenerateToken(user);
        return Ok(new { token, userId = user.Id });
    }
    
    [Authorize]
    [HttpPost("2fa/start")]
    public async Task<IActionResult> Start2fa()
    {
        var secret = await _tsh.Handle();
        return Ok(new { secret });
    }
    
    [Authorize]
    [HttpPost("2fa/confirm")]
    public async Task<IActionResult> Confirm2fa([FromBody] EnableTFCcommand command)
    {
        await _tfh.Handle(command);
        return Ok();
    }
    
    [HttpPost("otp/send")]
    public async Task<IActionResult> SendOtp([FromQuery] string email)
    {
        var code = await _gs.code(email);
        return Ok(new { email, code, ttlSeconds = 60 });
    }

    [HttpPost("otp/verify")]
    public async Task<IActionResult> VerifyOtp([FromQuery] string email, [FromQuery] string code)
    {
        var token = await _gs.AlikhanSEX(email, code);
        if (token.Equals("1"))
        {
            return BadRequest("User not found");
        }

        if (token.Equals("2"))
        {
            return BadRequest("Passcodes does not match");
        }
        
        var user = await _ur.GetByEmailAsync(email);
        if (user == null || user.Profile == null)
        {
            return BadRequest("sgllww");
        }
        
        return Ok(new{token, userId = user.Id});
    }

    [HttpGet("github/login")]
    public IActionResult LoginGitHub()
    {
        var clientId = _opts.ClientId;
        var baseUrl = "https://github.com/login/oauth/authorize";
        var redirectUri = "http://localhost:5278/api/auth/github/callback";
        var scope = "read:user user:email";
        
        var authUrl = $"{baseUrl}?client_id={clientId}&redirect_uri={redirectUri}&scope={scope}";

        return Redirect(authUrl);
    }

    [HttpGet("github/callback")]
    public async Task<IActionResult> Callback(string code)
    {
        if (code == null) throw new Exception("NO CODE");

        var url = "https://github.com/login/oauth/access_token";

        var client = new HttpClient();

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _opts.ClientId),
            new KeyValuePair<string, string>("client_secret", _opts.ClientSecret),
            new KeyValuePair<string, string>("code", code)
        });

        client.DefaultRequestHeaders.Add("Accept", "application/json");
        
        var result = await client.PostAsync(url, content);
        var body = await result.Content.ReadAsStringAsync();
        
        var json = JsonDocument.Parse(body);
        var accessToken = json.RootElement.GetProperty("access_token").GetString();
        
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        client.DefaultRequestHeaders.Add("User-Agent", "messengerApp");
        
        var userResponse = await client.GetAsync("https://api.github.com/user");
        var userBody = await userResponse.Content.ReadAsStringAsync();
       
        var json1 = JsonDocument.Parse(userBody);
        var githubId = json1.RootElement.GetProperty("id").GetInt32().ToString();
        var githubLogin = json1.RootElement.GetProperty("login").GetString();
        var githubName = json1.RootElement.GetProperty("name").GetString();

        //RegisterCommand command = new RegisterCommand { Email = "", Password = "", Name = githubName, ProfileName = githubName, Age = 21 };

        var user1 = await _ur.GetByGithubUserIdAsync(githubId);
        if (user1 == null)
        {
            var user = User.CreateFromOauth(githubName, githubLogin, "Github", githubId);
            // {
            //     Name = githubName,
            //     Email = "",
            //     PasswordHash = "hash",
            //     Profile = new Domain.Entities.Profile {Name = githubLogin, Age = 21},
            //     Provider = "Github",
            //     ProviderUserId = githubId
            // };
            await _ur.AddAsync(user);
        }
        var token = _its.GenerateToken(user1);
        return Redirect($"http://127.0.0.1:5500/index.html?token={token}&userId={user1.Id}&name={Uri.EscapeDataString(user1.Name ?? "github-user")}");
    }
}