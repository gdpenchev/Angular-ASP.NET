namespace CuriousReadersData.Queries;

using CuriousReadersData.Entities;

public class UserQueries : IUserQueries
{
    private readonly LibraryDbContext libraryDbContext;

    public UserQueries(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }

    public IQueryable<User> GetAllUsers(int page, int itemsPerpage, bool isActive)
    {
        var pageNumber = page == 0 ? 1 : page;

        var query = libraryDbContext.Users
            .Where(u=>u.IsActive == isActive)
            .OrderByDescending(u => u.RegistrationDate)
            .Skip((pageNumber - 1) * itemsPerpage)
            .Take(itemsPerpage)
            .AsQueryable();

        return query;
    }

    public int GetAllUsersCount(bool isActive)
    {
        var usersCount = libraryDbContext.Users
            .Where(u => u.IsActive == isActive)
                .Count();

        return usersCount;
    }
}
