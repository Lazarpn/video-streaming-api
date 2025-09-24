using System.ComponentModel.DataAnnotations.Schema;
using VideoStreaming.Entities.Interfaces;

namespace VideoStreaming.Entities;

public class MeetMember : IEntity
{
    public Guid Id { get; set; }
    public Guid MeetId { get; set; }
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(User.MeetMembers))]
    public virtual User User { get; set; }

    [ForeignKey(nameof(MeetId))]
    [InverseProperty(nameof(Meet.MeetMembers))]
    public virtual Meet Meet { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
