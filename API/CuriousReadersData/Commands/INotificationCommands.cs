namespace CuriousReadersData.Commands
{
    using CuriousReadersData.Entities;
    public interface INotificationCommands
    {
        Notification Create(Notification notification);
    }
}
