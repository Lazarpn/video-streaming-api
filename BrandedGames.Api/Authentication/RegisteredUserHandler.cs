using BrandedGames.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BrandedGames.Api.Authentication;

public class RegisteredUserHandler : AuthorizationHandler<RegisteredUserRequirement>
{
    private readonly BrandedGamesDbContext db;

    public RegisteredUserHandler(BrandedGamesDbContext db)
    {
        this.db = db;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RegisteredUserRequirement requirement)
    {
        var isUserRegistered = await CheckIfUserIsRegistered(context);

        if (isUserRegistered)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    public async Task<bool> CheckIfUserIsRegistered(AuthorizationHandlerContext context)
    {
        if (!AuthorizationHelper.TryParseUserId(context, out var userId))
        {
            return false;
        }

        return await db.Users.AnyAsync(u => u.Id == userId);
    }
}

public class RegisteredUserRequirement : IAuthorizationRequirement
{
}
