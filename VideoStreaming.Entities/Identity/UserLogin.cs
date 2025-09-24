using Microsoft.AspNetCore.Identity;
using System;

namespace VideoStreaming.Entities.Identity;

public class UserLogin : IdentityUserLogin<Guid>
{
    public virtual User User { get; set; }
}
