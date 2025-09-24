using VideoStreaming.Common.Models.Stream;

namespace VideoStreaming.Common.Models.User;

public class UserMeModel : UserInitialsModel
{
    public Guid Id { get; set; }

    public DateTime DateVerificationCodeExpires { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<StreamModel> Streams { get; set; }
}
