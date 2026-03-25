using System.Security.Claims;
using messengerApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace messengerApp.Infrastructure.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http)
    {
        _http = http;
    }

    public int UserId
    {
        get
        {
            var idStr = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(idStr)) return 0;
            return int.TryParse(idStr, out var id) ? id : 0;
        }
    }
}