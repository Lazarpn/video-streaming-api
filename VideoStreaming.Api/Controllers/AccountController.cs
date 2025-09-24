using VideoStreaming.Api.Authentication;
using VideoStreaming.Common.Enums;
using VideoStreaming.Common.Models.User;
using VideoStreaming.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoStreaming.Api.Authentication;
using VideoStreaming.Api.Controllers;
using VideoStreaming.Common.Models.User;
using VideoStreaming.Core;

namespace IdealWedding.Api.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : BaseController
{
    private readonly AccountManager accountManager;

    public AccountController(AccountManager accountManager)
    {
        this.accountManager = accountManager;
    }

    /// <summary>
    /// Gets current user's data
    /// </summary>
    /// <returns>Current user's information</returns>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserMeModel))]
    public async Task<IActionResult> GetMyUserInfo()
    {
        var userData = await accountManager.GetMyUserInfo(GetCurrentUserId().Value);
        return Ok(userData);
    }

    /// <summary>
    /// Registers and logins a new user using passed credentials
    /// </summary>
    /// <param name="model">User registration info</param>
    /// <returns>Login information</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseModel))]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
    {
        var authResponse = await accountManager.Register(model);
        return Ok(authResponse);
    }

    /// <summary>
    /// User login
    /// </summary>
    /// <param name="model">User's login credentials</param>
    /// <returns>Login information</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseModel))]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        var authResponse = await accountManager.Login(model);
        return Ok(authResponse);
    }
}
