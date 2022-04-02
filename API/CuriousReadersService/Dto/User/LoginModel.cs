using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersData.Dto.User;

[ExcludeFromCodeCoverage]
public class LoginModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}
