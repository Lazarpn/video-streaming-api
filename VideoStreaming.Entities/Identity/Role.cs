using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace VideoStreaming.Entities.Identity;

public class Role : IdentityRole<Guid>
{
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<RoleClaim> RoleClaims { get; set; }
}
