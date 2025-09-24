using AutoMapper;
using VideoStreaming.Core.Hubs;
using VideoStreaming.Data;
using Microsoft.AspNetCore.SignalR;

namespace VideoStreaming.Core;

public class SignalRManager
{
    private readonly VideoStreamingDbContext db;
    private readonly IMapper mapper;
    private readonly IHubContext<NotificationHub> hubContext;

    public SignalRManager(
        VideoStreamingDbContext db,
        IMapper mapper,
        IHubContext<NotificationHub> hubContext)
    {
        this.db = db;
        this.mapper = mapper;
        this.hubContext = hubContext;
    }

    public async Task NotifyUser<T>(Guid userId, string messageKey, T payload, string excludedConnectionId = null)
    {
        if (string.IsNullOrWhiteSpace(excludedConnectionId))
        {
            await hubContext.Clients.Group(userId.ToString()).SendAsync(messageKey, payload);
        }
        else
        {
            await hubContext.Clients.GroupExcept(userId.ToString(), excludedConnectionId)
                .SendAsync(messageKey, payload);
        }
    }

    public async Task NotifyUsers<T>(List<Guid> userIds, string messageKey, T payload,
        string excludedConnectionId = null)
    {
        foreach (var userId in userIds)
        {
            await NotifyUser(userId, messageKey, payload, excludedConnectionId);
        }
    }
}
