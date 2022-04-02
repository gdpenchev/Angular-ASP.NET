using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersData.Dto.User;

[ExcludeFromCodeCoverage]
public class UserModel
{
    public string Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public bool isActive { get; set; }

}
