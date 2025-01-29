using BrandedGames.Common.Models;
using BrandedGames.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

[Route("api/game-types")]
[ApiController]
public class GameTypeController: BaseController
{
    private readonly GameTypeManager gameTypeManager;

    public GameTypeController(GameTypeManager gameTypeManager)
    {
        this.gameTypeManager = gameTypeManager;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GameTypeModel>))]
    public async Task<IActionResult> CreateGame()
    {
        var result = await gameTypeManager.GetTypes();
        return Ok(result);
    }
}
