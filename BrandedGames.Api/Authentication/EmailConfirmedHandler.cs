using BrandedGames.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BrandedGames.Api.Authentication;

public class EmailConfirmedHandler : AuthorizationHandler<EmailConfirmedRequirement>
{
    private readonly BrandedGamesDbContext db;

    public EmailConfirmedHandler(BrandedGamesDbContext db)
    {
        this.db = db;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailConfirmedRequirement requirement)
    {
        var hasEmailConfirmed = await CheckEmailConfirmed(context);

        if (hasEmailConfirmed)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    public async Task<bool> CheckEmailConfirmed(AuthorizationHandlerContext context)
    {
        if (!AuthorizationHelper.TryParseUserId(context, out var userId))
        {
            return false;
        }

        var userInfo = await db.Users
            .Where(u => u.Id == userId)
            .Select(u => new { Id = u.Id, EmailConfirmed = u.EmailConfirmed })
            .FirstOrDefaultAsync();

        return userInfo != null && userInfo.EmailConfirmed;
    }
}

public class EmailConfirmedRequirement : IAuthorizationRequirement
{
}
