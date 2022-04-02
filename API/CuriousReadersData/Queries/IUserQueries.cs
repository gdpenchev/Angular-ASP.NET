namespace CuriousReadersData.Queries;

using CuriousReadersData.Entities;
public interface IUserQueries
{
    IQueryable<User> GetAllUsers(int page, int itemsPerpage, bool isActive);

    int GetAllUsersCount(bool isActive);
}
