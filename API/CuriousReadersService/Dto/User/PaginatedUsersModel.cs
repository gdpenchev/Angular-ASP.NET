using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersData.Dto.User;

[ExcludeFromCodeCoverage]
public class PaginatedUsersModel
{
    public IEnumerable<UserModel> Users { get; set; }

    public int CurrentPage { get; set; }

    public int UsersCount { get; set; }
}
