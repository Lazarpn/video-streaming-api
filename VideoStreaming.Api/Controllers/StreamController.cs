using VideoStreaming.Api.Authentication;
using VideoStreaming.Common.Enums;
using VideoStreaming.Common.Models.User;
using VideoStreaming.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoStreaming.Api.Authentication;
using VideoStreaming.Api.Controllers;
using VideoStreaming.Common.Models.Stream;
using VideoStreaming.Common.Models.User;
using VideoStreaming.Core;
using VideoStreaming.Core.Models;

namespace VideoStreaming.Api.Controllers;

[Route("api/meets")]
[ApiController]
public class StreamController : BaseController
{
    private readonly StreamManager streamManager;

    public StreamController(StreamManager streamManager)
    {
        this.streamManager = streamManager;
    }

    [HttpGet("types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseModel))]
    public async Task<IActionResult> GetStreamType(Guid id)
    {
        var streamModel = await streamManager.GetStreamType(id);
        return Ok(streamModel);
    }

    [HttpGet("{id:guid}/member")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MeetMemberModel))]
    public async Task<IActionResult> GetMeetMember(Guid id)
    {
        var streamModel = await streamManager.GetMeetMember(id, GetCurrentUserId().Value);
        return Ok(streamModel);
    }

    [HttpPost("{id:guid}/join")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> JoinStream(Guid id, [FromBody] JoinStreamModel model)
    {
        await streamManager.JoinStream(id, GetCurrentUserId().Value, model.MakeMember, model.Password);
        return NoContent();
    }

    [HttpPost("{id:guid}/leave")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> LeaveStream(Guid id)
    {
        await streamManager.LeaveStream(id, GetCurrentUserId().Value);
        return NoContent();
    }

    [HttpPost("{id:guid}/create-an-offer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateOffer([FromBody] PeerSignal model, Guid id)
    {
        await streamManager.CreateOffer(model, id, GetCurrentUserId().Value);
        return NoContent();
    }

    [HttpPost("{id:guid}/respond-to-an-offer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RespontToOffer([FromBody] PeerSignal model, Guid id)
    {
        await streamManager.RespondToOffer(model, id, GetCurrentUserId().Value);
        return NoContent();
    }

    [HttpPost("{id:guid}/ice")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SendIceCandidate([FromBody] PeerSignal model, Guid id)
    {
        await streamManager.SendIceCandidate(model, id, GetCurrentUserId().Value);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StreamModel))]
    public async Task<IActionResult> CreateStream([FromBody] StreamCreateModel model)
    {
        var userId = GetCurrentUserId().Value;
        var streamModel = await streamManager.CreateStream(model, userId);
        return Ok(streamModel);
    }

    [HttpPost("unlock/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UnlockStream([FromBody] StreamUnlockModel model, Guid streamId)
    {
        await streamManager.UnlockStream(model, streamId);
        return NoContent();
    }
}
