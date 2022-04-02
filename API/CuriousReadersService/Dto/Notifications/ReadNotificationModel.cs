using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersService.Dto.Notifications
{
    [ExcludeFromCodeCoverage]
    public class ReadNotificationModel
    {
        public string BookTitle { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public bool IsRead { get; set; }
    }
}
