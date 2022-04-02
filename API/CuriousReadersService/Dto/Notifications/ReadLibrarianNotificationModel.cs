namespace CuriousReadersService.Dto.Notifications
{
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public class ReadLibrarianNotificationModel
    {
        public string BookTitle { get; set; }

        public DateTime ReturnDate { get; set; }

        public string Date { get; set; }

        public string ReserveeUser { get; set; }

        public string ReserveeNumber { get; set; }
    }
}
