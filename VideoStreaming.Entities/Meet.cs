using System.ComponentModel.DataAnnotations.Schema;
using VideoStreaming.Common.Enums;
using VideoStreaming.Entities.Interfaces;

namespace VideoStreaming.Entities;

public class Meet : IEntity
{
    public Guid Id { get; set; }
    public Guid UserOwnerId { get; set; }
    public StreamType StreamType { get; set; }
    public string Password { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    [ForeignKey(nameof(UserOwnerId))]
    [InverseProperty(nameof(User.Meets))]
    public virtual User User { get; set; }

    [InverseProperty(nameof(MeetMember.Meet))]
    public virtual ICollection<MeetMember> MeetMembers { get; set; } = new HashSet<MeetMember>();
}
