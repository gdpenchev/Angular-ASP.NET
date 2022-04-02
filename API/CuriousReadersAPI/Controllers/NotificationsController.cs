namespace CuriousReadersAPI.Controllers;

using CuriousReadersService.Dto.Notifications;
using CuriousReadersService.Services.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService notificationsService;

    public NotificationsController(INotificationService notificationsService)
    {
        this.notificationsService = notificationsService;
    }

    [HttpPost("Send")]
    [Authorize]
    public async Task<IActionResult> Create(CreateNotificationModel model)
    {
        var result = await this.notificationsService.Create(model);

        if (result is null)
        {
            return BadRequest("Notification was not sent!");
        }

        return Ok(model);
    }

    [HttpGet("Unread")]
    [Authorize]
    public ActionResult<int> Count(string userId)
    {
        var count = this.notificationsService.CountUnread(userId);

        return Ok(count);
    }

    [HttpGet("All")]
    [Authorize]
    public IActionResult All(string userId, int skip, int notificationsPerPage)
    {
        var result = this.notificationsService.All(userId,skip,notificationsPerPage);

        return Ok(result);
    }
    [HttpGet("BooksNotOnTime")]
    public IActionResult BooksNotOnTimeNotification(int skip, int notificationsPerPage)
    {
        var result = this.notificationsService.BooksNotOnTimeNotification(skip, notificationsPerPage);

        return Ok(result);
    }
}
