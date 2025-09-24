using VideoStreaming.Common.Enums;

namespace VideoStreaming.Common.Models.User;

public class StreamCreateModel
{
    public StreamType Type { get; set; }
    public string Password { get; set; }
}
