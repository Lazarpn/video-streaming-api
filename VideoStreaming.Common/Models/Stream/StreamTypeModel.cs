using VideoStreaming.Common.Enums;

namespace VideoStreaming.Common.Models.User;

public class StreamTypeModel
{
    public StreamType Type { get; set; }
    public Guid UserOwnerId { get; set; }
}
