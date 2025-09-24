using VideoStreaming.Api.Authentication;
using VideoStreaming.Common.Enums;
using VideoStreaming.Core;
using VideoStreaming.Data;
using VideoStreaming.Entities;
using VideoStreaming.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace VideoStreaming.Api.Controllers;

[Route("api/utilities")]
[ApiController]
public class UtilityController : ControllerBase
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;
    private readonly VideoStreamingDbContext db;

    public UtilityController(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        VideoStreamingDbContext db)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.db = db;
    }

    [HttpPost]
    [Route("seed-data")]
    // [Authorize(Policy = Policies.AdministratorUser)]
    public async Task<IActionResult> SeedData()
    {
        await db.SeedData(userManager, roleManager);
        return NoContent();
    }
}
