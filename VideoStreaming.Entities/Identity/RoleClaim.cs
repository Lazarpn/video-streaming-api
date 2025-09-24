using Microsoft.AspNetCore.Identity;
using System;

namespace VideoStreaming.Entities.Identity;

public class RoleClaim : IdentityRoleClaim<Guid>
{
    public virtual Role Role { get; set; }
}
