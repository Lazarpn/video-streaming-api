using VideoStreaming.Common.Enums;

namespace VideoStreaming.Common.Models.User;

public class StreamModel
{
    public Guid Id { get; set; }
    public StreamType Type { get; set; }
    public Guid UserOwnerId { get; set; }
}
