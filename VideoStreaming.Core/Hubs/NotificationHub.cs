using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VideoStreaming.Core.Hubs;

public class NotificationHub : Hub
{
    protected Guid? GetCurrentUserId()
    {
        _ = Guid.TryParse(Context.User?.FindFirst(ClaimTypes.Name)?.Value, out Guid parsedId);
        return parsedId == Guid.Empty ? null : parsedId;
    }

    public async Task SendSignal(string targetConnectionId, object message)
    {
        await Clients.Client(targetConnectionId)
            .SendAsync("ReceiveSignal", Context.ConnectionId, message);
    }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetCurrentUserId().Value.ToString());
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
