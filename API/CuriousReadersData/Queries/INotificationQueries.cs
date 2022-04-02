namespace CuriousReadersData.Queries
{
    using CuriousReadersData.Entities;
    public interface INotificationQueries
    {
        int CountUnread(string userId);

        IEnumerable<Notification> All(string userId, int skip, int notificationsPerPage);

        void ReadAllNotification(string userId);
    }
}
