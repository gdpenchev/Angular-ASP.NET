namespace CuriousReaders.Test.Controllers;

using System;
using System.Threading.Tasks;

using CuriousReadersAPI.Controllers;
using CuriousReadersData.Dto.User;
using CuriousReadersService.Services.User;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Xunit;

public class UsersControllerTest
{
    private IUserService userServiceMock = A.Fake<IUserService>();

    [Fact]
    public void Login_Returns_OkIfLoginSucceeded()
    {
        //Arrange
        LoginModel loginUser = new LoginModel
        {
            Email = "Testemail@abv.bg",
            Password = "!1234Password"
        };

        var signInResult = Microsoft.AspNetCore.Identity.SignInResult.Success;

        A.CallTo(() => this.userServiceMock.Login(A<LoginModel>.Ignored))
                .Returns(signInResult);

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.Login(loginUser);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void Login_Returns_BadRequestIfLoginNotSucceeded()
    {
        //Arrange
        LoginModel loginUser = new LoginModel
        {
            Email = "Testemail@abv.bg",
            Password = "!1234Password"
        };

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.Login(loginUser);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void Register_Returns_BadRequest_IfModel_IsNull()
    {
        //Arrange
        RegisterModel registerUser = null;

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.Register(registerUser);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void Register_Returns_Message_IfResult_NotSuccess()
    {
        //Arrange
        RegisterModel registerUser = new RegisterModel
        {
            FirstName = "FirstTest",
            LastName = "LastTest",
            Email = "TestMail@abv.bg",
            Password = "!1234Testpass",
            RepeatPassword = "!1234Testpass",
            PhoneNumber = "+359888888888",
            Country = "SomeCountry",
            City = "SomeCity",
            Street = "SomeStreet",
            StreetNumber = "SomeNumber",
            ApartmentNumber = "SomeNumber",
            BuildingNumber = "SomeNumber",
            AdditionalInfo = "SomeInfo"
        };

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.Register(registerUser);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<AggregateException>(result.Exception);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void AllUsers_Returns_OkObjectResult()
    {
        //Arrange
        int page = 1;
        int limit = 20;
        bool isActive = false;

        A.CallTo(() => this.userServiceMock.GetAllUsers(page, limit, isActive))
          .Returns(new PaginatedUsersModel());

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.AllUsers(page, limit, isActive);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<ActionResult<PaginatedUsersModel>>(result);
    }

    [Fact]
    public void AllUsers_Returns_NotFound_IfNull()
    {
        //Arrange
        int page = 1;
        int limit = 20;
        bool isActive = false;

        A.CallTo(() => this.userServiceMock.GetAllUsers(page, limit, isActive))
          .Returns(null);

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.AllUsers(page, limit, isActive);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
        Assert.IsType<ActionResult<PaginatedUsersModel>>(result);
    }

    [Fact]
    public void ApprovaUser_Returns_OkIfUserIsFound()
    {
        //Arrange
        A.CallTo(() => this.userServiceMock.ApproveUser(A<string>.Ignored))
                .Returns(true);

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.ApproveUser("1");

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void ApprovaUser_Returns_NotFoundIfUserIsNotFound()
    {
        //Arrange
        A.CallTo(() => this.userServiceMock.ApproveUser(A<string>.Ignored))
                .Returns(false);

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.ApproveUser("1");

        //Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void SendForgotPasswordEmail_Returns_OkResult_If_UserExists()
    {
        //Arrange
        A.CallTo(() => this.userServiceMock.SendForgotPasswordEmail(A<string>.Ignored))
                .Returns(true);
        var email = "Testemail@abv.bg";

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.SendForgotPasswordEmail(email);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void SendForgotPasswordEmail_Returns_NotFound_If_UserWithTheSpecifiedEmailIsNotFound()
    {
        //Arrange
        A.CallTo(() => this.userServiceMock.SendForgotPasswordEmail(A<string>.Ignored))
                .Returns(false);
        var email = "Testemail@abv.bg";

        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.SendForgotPasswordEmail(email);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void ChangePassword_Returns_NotFound_If_ResetPasswordResultIsNull()
    {
        //Arrange
        var changePasswordResult = new IdentityResult();

        A.CallTo(() => this.userServiceMock.ChangePassword(A<ChangePasswordModel>.Ignored))
                .Returns(changePasswordResult = null);

        var usersController = new UsersController(userServiceMock);
        //Act
        var result = usersController.ChangePassword(new ChangePasswordModel());

        //Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void ChangePassword_Returns_BadRequest_If_ResetPasswordTokenIsInvalid()
    {
        //Arrange
        IdentityResult changePasswordResult = IdentityResult.Failed(new IdentityError { Code = "InvalidToken" });
        
        A.CallTo(() => this.userServiceMock.ChangePassword(A<ChangePasswordModel>.Ignored))
                .Returns(changePasswordResult);

        var usersController = new UsersController(userServiceMock);
        //Act
        var result = usersController.ChangePassword(new ChangePasswordModel());

        //Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void ChangePassword_Returns_Ok_If_ResetPasswordResultIsSuccessful()
    {
        //Arrange
        IdentityResult changePasswordResult = IdentityResult.Success;

        A.CallTo(() => this.userServiceMock.ChangePassword(A<ChangePasswordModel>.Ignored))
                .Returns(changePasswordResult);

        var usersController = new UsersController(userServiceMock);
        //Act
        var result = usersController.ChangePassword(new ChangePasswordModel());

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void ChangePassword_Returns_ForbidResult_If_ResetPasswordNotSuccessful()
    {
        //Arrange
        var usersController = new UsersController(userServiceMock);

        //Act
        var result = usersController.ChangePassword(new ChangePasswordModel());

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ForbidResult>(result.Result);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public void GetAllUsersCount_Returns_AllNotActiveUsersCount()
    {
        //Arrange
        A.CallTo(() => this.userServiceMock.GetAllUsersCount(A<bool>.Ignored))
          .Returns(new int());

        var usersController = new UsersController(userServiceMock);

        //Act
        bool isActive = false;
        var result = usersController.GetAllUsersCount(isActive);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ActionResult<int>>(result);
    }

    [Fact]
    public void GetAllUsersCount_Returns_AllActiveUsersCount()
    {
        //Arrange
        A.CallTo(() => this.userServiceMock.GetAllUsersCount(A<bool>.Ignored))
          .Returns(new int());

        var usersController = new UsersController(userServiceMock);

        //Act
        bool isActive = true;
        var result = usersController.GetAllUsersCount(isActive);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ActionResult<int>>(result);
    }
}
