using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ThreadAlert.Entities;
using ThreadAlert.Persistance;

namespace WebApplication6.Hubs;

public class NotificationHub: Hub
{
    private readonly AppDbContextFactory _dbContextFactory;
    private readonly AppDbContext _dbContext;


    private readonly UserManager<AppUser> _userManager;

    public NotificationHub()
    {
        _dbContextFactory = new AppDbContextFactory();
        _dbContext = _dbContextFactory.CreateDbContext(Array.Empty<string>());
    }

    public async Task SendNotificationToClient(Message message)
    {
        var hubConnections = _dbContext.HubConnections
            .Include(a => a.User)
            .ThenInclude(u => u.DangerousObjects)
            .Where(con => con.User.DangerousObjects
                            .Select(a => a.Id)
                            .Contains(message.DangerousObjectId))
            .ToList();

        foreach (var hubConnection in hubConnections)
        {
            await Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedNotification", message.Title);
        }
    }

    public override Task OnConnectedAsync()
    {
        Clients.Caller.SendAsync("OnConnected");
        return base.OnConnectedAsync();
    }

    public async Task SaveUserConnection(string userid)
    {
        if (userid == null)
            return;

        var connectionId = Context.ConnectionId;
        HubConnection hubConnection = new HubConnection
        {
            ConnectionId = connectionId,
            UserId = userid
        };

        _dbContext.HubConnections.Add(hubConnection);
        await _dbContext.SaveChangesAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var hubConnection = _dbContext.HubConnections.FirstOrDefault(con => con.ConnectionId == Context.ConnectionId);
        if (hubConnection != null)
        {
            _dbContext.HubConnections.Remove(hubConnection);
            _dbContext.SaveChangesAsync();
        }

        return base.OnDisconnectedAsync(exception);
    }
}
