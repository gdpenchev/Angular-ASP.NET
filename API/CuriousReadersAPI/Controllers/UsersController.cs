namespace CuriousReadersAPI.Controllers;

using CuriousReadersData.Dto.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CuriousReadersService.Services.User;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var result = await _userService.Login(model);

        if (result.Succeeded)
        {
            var token = _userService.GenerateJwtToken(model.Email);

            return Ok(new { token });
        }

        return BadRequest(new { message = "Username or password is incorrect." });
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (model is null)
        {
            return BadRequest();
        }

        var result = await _userService.Register(model);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.First().Description);
        }

        return Ok(result);
    }

    [HttpGet("All")]
    [Authorize(Roles = "Librarian")]
    public ActionResult<PaginatedUsersModel> AllUsers(int page, int itemsPerpage, bool isActive)
    {
        var result = _userService.GetAllUsers(page, itemsPerpage, isActive);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("Count")]
    public ActionResult<int> GetAllUsersCount(bool isActive)
    {
        var result = _userService.GetAllUsersCount(isActive);

        return Ok(result);
    }

    [HttpPatch("ApproveUser")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> ApproveUser(string id)
    {

        var result = await _userService.ApproveUser(id);

        if (!result)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> SendForgotPasswordEmail(string userEmail)
    {
        var result = await _userService.SendForgotPasswordEmail(userEmail);

        if (!result)
        {
            return NotFound("User with this email was not found.");
        }

        return Ok();
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        var result = await _userService.ChangePassword(model);

        if (result is null)
        {
            return NotFound("User with this email was not found.");
        }

        if (!result.Succeeded && result.Errors.Any(e => e.Code == "InvalidToken"))
        {
            return BadRequest("Your password recovery link is already used or expired! Click on login page forgot password to send new one and try again.");
        } 
        else if (!result.Succeeded)
        {
            return Forbid();
        }

        return Ok(result);
    }
}
