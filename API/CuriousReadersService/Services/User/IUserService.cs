namespace CuriousReadersService.Services.User;

using CuriousReadersData.Dto.User;
using Microsoft.AspNetCore.Identity;

public interface IUserService
{
    PaginatedUsersModel GetAllUsers(int page, int itemsPerpage, bool isActive);

    List<UserModel> MapAllUsers(int page, int itemsPerpage, bool isActive);

    int GetAllUsersCount(bool isActive);

    Task<IdentityResult> Register(RegisterModel model);

    Task<SignInResult> Login(LoginModel model);

    Task<bool> ApproveUser(string id);

    Task<bool> SendForgotPasswordEmail(string userEmail);

    Task<IdentityResult> ChangePassword(ChangePasswordModel model);

    Task<string> GenerateJwtToken(string email);
}
