using System.ComponentModel.DataAnnotations;
using AutoMapper;
using VideoStreaming.Data;
using Microsoft.EntityFrameworkCore;
using VideoStreaming.Common.Enums;
using VideoStreaming.Common.Helpers;
using VideoStreaming.Common.Models.Stream;
using VideoStreaming.Common.Models.User;
using VideoStreaming.Core.Hubs;
using VideoStreaming.Core.Models;
using VideoStreaming.Data.Migrations;
using VideoStreaming.Entities;
using Meet = VideoStreaming.Entities.Meet;
using MeetMember = VideoStreaming.Entities.MeetMember;

namespace VideoStreaming.Core;

public class StreamManager
{
    private readonly VideoStreamingDbContext db;
    private readonly IMapper mapper;
    private readonly SignalRManager signalRManager;

    public StreamManager(VideoStreamingDbContext db, IMapper mapper, SignalRManager signalRManager)
    {
        this.db = db;
        this.mapper = mapper;
        this.signalRManager = signalRManager;
    }

    public async Task<StreamModel> CreateStream(StreamCreateModel model, Guid userId)
    {
        var meet = new Meet
        {
            StreamType = model.Type,
            Password = model.Password,
            UserOwnerId = userId
        };

        await db.Meets.AddAsync(meet);
        await db.SaveChangesAsync();

        var meetMember = new MeetMember
        {
            UserId = userId,
            MeetId = meet.Id
        };

        await db.MeetMembers.AddAsync(meetMember);
        await db.SaveChangesAsync();

        return new StreamModel
        {
            Id = meet.Id,
            Type = model.Type,
            UserOwnerId = userId
        };
    }

    public async Task JoinStream(Guid streamId, Guid userId, bool makeMember, string password)
    {
        var streamExists = await db.Meets.AnyAsync(s => s.Id == streamId);
        ValidationHelper.MustExist<Meet>(streamExists);

        var stream = await db.Meets.Where(s => s.Id == streamId).Select(s => new { s.UserOwnerId, s.Id, s.Password })
            .FirstOrDefaultAsync();

        if (stream.UserOwnerId != userId && stream.Password != password)
        {
            throw new Common.Exceptions.ValidationException(ErrorCode.WrongPassword);
        }

        var meetMemberExists = await db.MeetMembers.AnyAsync(m => m.UserId == userId && m.MeetId == streamId);

        if (!meetMemberExists && makeMember)
        {
            var meetMember = new MeetMember
            {
                UserId = userId,
                MeetId = stream.Id
            };

            await db.MeetMembers.AddAsync(meetMember);
            await db.SaveChangesAsync();
        }

        if (stream.UserOwnerId != userId && !makeMember)
        {
            await signalRManager.NotifyUser(stream.UserOwnerId, NotificationMethod.UserJoinedTheStream, userId);
        }
    }

    public async Task<MeetMemberModel> GetMeetMember(Guid streamId, Guid userId)
    {
        var meetMember = await db.MeetMembers.FirstOrDefaultAsync(m => m.MeetId == streamId && m.UserId == userId);
        ValidationHelper.MustExist(meetMember);

        return new MeetMemberModel
        {
            Id = meetMember.Id,
            MeetId = meetMember.Id,
            UserId = userId
        };
    }

    public async Task CreateOffer(PeerSignal offer, Guid streamId, Guid userId)
    {
        var streamMemberExists = await db.MeetMembers.AnyAsync(m => m.MeetId == streamId && m.UserId == userId);
        ValidationHelper.MustExist<MeetMember>(streamMemberExists);

        await signalRManager.NotifyUser(offer.ToId, NotificationMethod.BroadcasterCreatedOffer, offer,
            userId.ToString());
    }

    public async Task RespondToOffer(PeerSignal answer, Guid streamId, Guid userId)
    {
        var streamMemberExists = await db.MeetMembers.AnyAsync(m => m.MeetId == streamId && m.UserId == userId);
        ValidationHelper.MustExist<MeetMember>(streamMemberExists);

        await signalRManager.NotifyUser(answer.ToId, NotificationMethod.ViewerRespondedToAnOffer, answer,
            userId.ToString());
    }

    public async Task SendIceCandidate(PeerSignal ice, Guid streamId, Guid userId)
    {
        var streamMemberExists = await db.MeetMembers.AnyAsync(m => m.MeetId == streamId && m.UserId == userId);
        ValidationHelper.MustExist<MeetMember>(streamMemberExists);

        var streamOwnerId =
            await db.Meets.Where(m => m.Id == streamId).Select(s => s.UserOwnerId).FirstOrDefaultAsync();

        if (streamOwnerId == userId)
        {
            var streamMemberIds = await db.MeetMembers
                .Where(mm => mm.MeetId == streamId && mm.UserId != userId)
                .Select(mm => mm.UserId)
                .ToListAsync();

            await signalRManager.NotifyUsers(streamMemberIds, NotificationMethod.IceCandidateReceived, ice,
                userId.ToString());
        }
        else
        {
            await signalRManager.NotifyUser(ice.ToId, NotificationMethod.IceCandidateReceived, ice);
        }
    }

    public async Task LeaveStream(Guid streamId, Guid userId)
    {
        var streamMemberExists = await db.MeetMembers.AnyAsync(m => m.MeetId == streamId && m.UserId == userId);
        ValidationHelper.MustExist<MeetMember>(streamMemberExists);

        var streamMemberIds = await db.MeetMembers
            .Where(mm => mm.MeetId == streamId && mm.UserId != userId)
            .Select(mm => mm.UserId)
            .ToListAsync();

        await signalRManager.NotifyUsers(streamMemberIds, NotificationMethod.UserLeftTheStream, userId);

        await db.MeetMembers
            .Where(m => m.MeetId == streamId && m.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task UnlockStream(StreamUnlockModel model, Guid streamId)
    {
        var stream = await db.Meets.FirstOrDefaultAsync(s => s.Id == streamId);
        ValidationHelper.MustExist(stream);

        if (stream.Password != model.Password)
        {
            throw new ValidationException("Invalid password");
        }
    }

    public async Task<StreamTypeModel> GetStreamType(Guid id)
    {
        var streamExists = await db.Meets.AnyAsync(s => s.Id == id);
        ValidationHelper.MustExist<Meet>(streamExists);

        var streamType = await db.Meets.Where(m => m.Id == id).Select(m => new { m.UserOwnerId, m.StreamType })
            .FirstOrDefaultAsync();

        return new StreamTypeModel
        {
            Type = streamType.StreamType,
            UserOwnerId = streamType.UserOwnerId
        };
    }
}
