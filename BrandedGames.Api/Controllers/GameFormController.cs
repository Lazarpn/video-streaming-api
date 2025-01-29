using BrandedGames.Common.Models;
using BrandedGames.Core;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

[Route("api/customer-games")]
[ApiController]
public class GameFormController : BaseController
{
    private readonly GameFormManager gameFormManager;

    public GameFormController(GameFormManager gameFormManager)
    {
        this.gameFormManager = gameFormManager;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateGame([FromForm] GameFormCreateModel model)
    {
        model.Files = model.Files.Any() ? model.Files : Request.Form.Files.ToList();
        await gameFormManager.Create(model);
        return NoContent();
    }
}
