using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersData.Dto.User;

[ExcludeFromCodeCoverage]
public class ChangePasswordModel
{
    public string Email { get; set; }

    public string ResetToken { get; set; }

    public string NewPassword { get; set; }
}