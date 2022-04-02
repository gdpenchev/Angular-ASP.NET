namespace CuriousReadersService.Services.User;

using AutoMapper;
using CuriousReadersData.Dto.User;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using CuriousReadersService.Services.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using static CuriousReadersData.DataConstants;

public class UserService : IUserService
{
    private readonly IUserQueries userQueries;
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly IMailService mailService;
    private IConfiguration configuration;

    public UserService(IUserQueries userQueries,
        IMapper mapper,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMailService mailService,
        IConfiguration configuration)
    {
        this.userQueries = userQueries;
        this.mapper = mapper;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.mailService = mailService;
        this.configuration = configuration;
    }

    public PaginatedUsersModel GetAllUsers(int page, int itemsPerpage, bool isActive)
    {
        var userList = MapAllUsers(page, itemsPerpage, isActive);

        var count = userList.Count();

        var result = new PaginatedUsersModel
        {
            UsersCount = count,
            CurrentPage = page,
            Users = userList
        };

        return result;
    }

    public List<UserModel> MapAllUsers(int page, int itemsPerpage, bool isActive)
    {
        var query = userQueries.GetAllUsers(page, itemsPerpage, isActive);

        var users = mapper.Map<IQueryable<User>, List<UserModel>>(query);

        return users;
    }

    public int GetAllUsersCount(bool isActive)
    {
        return userQueries.GetAllUsersCount(isActive);
    }

    public async Task<IdentityResult> Register(RegisterModel model)
    {

        var mappedUser = mapper.Map<RegisterModel, User>(model);

        return await userManager.CreateAsync(mappedUser, model.Password);
    }

    public async Task<bool> ApproveUser(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user is null)
        {
            return false;
        }

        await this.mailService.SendUserApprovalEmail(user.Email, user.FirstName);

        user.IsActive = true;

        await userManager.AddToRoleAsync(user, "Reader");
        await userManager.UpdateAsync(user);

        return true;
    }

    public async Task<bool> SendForgotPasswordEmail(string userEmail)
    {
        var user = await userManager.FindByEmailAsync(userEmail);

        if (user is null)
        {
            return false;
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var tokenQueryParam = new Dictionary<string, string>
        {
            {"token", token },
            {"email", user.Email }
        };

        var tokenUrl = QueryHelpers.AddQueryString($"{WebsiteBaseUrl}account-new-password", tokenQueryParam);

        await this.mailService.SendForgotPasswordEmail(user.FirstName, user.Email, tokenUrl);

        return true;
    }

    public async Task<IdentityResult> ChangePassword(ChangePasswordModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            return null;
        }

        return await userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);
    }
    public async Task<SignInResult> Login(LoginModel model)
    {
        return await signInManager
            .PasswordSignInAsync(model.Email, model.Password, false, false);
    }

    public async Task<string> GenerateJwtToken(string email)
    {
        var jwtSecret = configuration.GetSection("ApplicationSettings").GetSection("JWT_Secret").Value;
        var claims = new List<Claim>() { new Claim("Email", email) };
        var user = await userManager.FindByEmailAsync(email);
        var roles = await userManager.GetRolesAsync(user);
        claims.Add(new Claim("FullName", $"{user.FirstName} {user.LastName}"));
        claims.Add(new Claim("UserIsActive", $"{user.IsActive}"));

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("Id", user.Id));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return token;
    }
}
