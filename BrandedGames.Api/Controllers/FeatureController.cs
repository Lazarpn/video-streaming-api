using BrandedGames.Common.Models;
using BrandedGames.Core;
using BrandedGames.Data;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

[Route("api/features")]
[ApiController]
public class FeatureController: BaseController
{
    private readonly FeatureManager featureManager;

    public FeatureController(FeatureManager featureManager)
    {
        this.featureManager = featureManager;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FeatureModel>))]
    public async Task<IActionResult> GetFeatures()
    {
        var result = await featureManager.GetFeatures();
        return Ok(result);
    }
}
