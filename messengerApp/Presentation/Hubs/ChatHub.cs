using System.Security.Claims;
using messengerApp.Application.Interfaces;
using messengerApp.Application.SaveMessage.Command;
using messengerApp.Application.SaveMessage.Handler;
using messengerApp.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace messengerApp.Presentation.Hubs;
[Authorize]
public class ChatHub : Hub
{
    private readonly SaveMessageHandler _mh;
    private readonly ILobbyStore _ls;
    private readonly ILobbyMemberRepository _lmr;
    private readonly IUnitOfWork _uow;

    public ChatHub(SaveMessageHandler handler, ILobbyStore ls, ILobbyMemberRepository lmr, IUnitOfWork uow)
    {
        _mh = handler;
        _ls = ls;
        _lmr = lmr;
        _uow = uow;
    }
    
    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        //await Clients.Group(chatId).SendAsync("Notify", $"User {Context.ConnectionId} joined {chatId}");
    }

    public async Task SendToChat(SaveMessageCommand command)
    {
        await _mh.Handle(command);

        await Clients.Group(command.roomname).SendAsync("ReceiveMessage", command);
    }

    public async Task<Lobby> CreateLobby()
    {
        //user id is found from the jwt via Context.User
        var userId = int.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var lobby = await _ls.CreateLobby(userId);
        //connecting to lobby using Context = current user and lobby code
        await Groups.AddToGroupAsync(Context.ConnectionId, lobby.Code);
        await _lmr.AddAsync(lobby.Code, userId);
        await _uow.SaveChangesAsync();

        return lobby;
    }

    public async Task<bool> JoinLobby(string lobbyCode)
    {
        lobbyCode = lobbyCode.Trim();
        var userId = int.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userName = Context.User!.FindFirstValue(ClaimTypes.Name);
        var res = await _ls.JoinLobby(lobbyCode, userId);

        if (!res)
        {
            return false;
        }
        await _lmr.AddAsync(lobbyCode, userId);
        await _uow.SaveChangesAsync();

        await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
        await Clients.Group(lobbyCode).SendAsync("Notify", $"{userName} joined");
        return true;
    }

    public async Task<bool> LeaveLobby(string lobbyCode)
    {
        lobbyCode = lobbyCode.Trim();
        var userId = int.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userName = Context.User!.FindFirstValue(ClaimTypes.Name);
        var res = await _ls.LeaveLobby(lobbyCode, userId);

        if (!res)
        {
            return false;
        }

        await Clients.Group(lobbyCode).SendAsync("Notify", $"{userName} left");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyCode);
        return true;
    }
}