using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersService.Dto.Notifications;

[ExcludeFromCodeCoverage]
public class CreateNotificationModel
{
    public string Email { get; set; }

    public int BookId { get; set; }
}
