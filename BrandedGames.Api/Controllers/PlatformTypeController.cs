using BrandedGames.Common.Models;
using BrandedGames.Core;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

[Route("api/platform-types")]
[ApiController]
public class PlatformTypeController: BaseController
{
    private readonly PlatformTypeManager platformTypeManager;

    public PlatformTypeController(PlatformTypeManager platformTypeManager)
    {
        this.platformTypeManager = platformTypeManager;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlatformTypeModel>))]
    public async Task<IActionResult> GetPlatforms()
    {
        var result = await platformTypeManager.GetPlatforms();
        return Ok(result);
    }
}
