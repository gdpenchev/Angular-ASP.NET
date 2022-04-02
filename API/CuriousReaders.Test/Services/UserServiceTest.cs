using AutoMapper;
using CuriousReadersData.Dto.User;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using CuriousReadersService.Profiles;
using CuriousReadersService.Services.Mail;
using CuriousReadersService.Services.User;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CuriousReaders.Test.Services
{
    public class UserServiceTest
    {
        private readonly IUserQueries userQueryMock = A.Fake<IUserQueries>();
        private UserService userService;
        private readonly UserManager<User> userManager = A.Fake<UserManager<User>>();
        private readonly SignInManager<User> signInManager = A.Fake<SignInManager<User>>();
        private readonly IQueryable<User> usersQueryMock = A.Fake<IQueryable<User>>();
        private readonly RegisterModel registerModelMock = A.Fake<RegisterModel>();
        private readonly User userMock = A.Fake<User>();
        private readonly List<UserModel> listUserModelMock = A.Fake<List<UserModel>>();
        private readonly LoginModel loginModelMock = A.Fake<LoginModel>();
        private readonly ChangePasswordModel changePasswordModelMock = A.Fake<ChangePasswordModel>();
        private readonly IMapper mapper;
        private readonly IMailService mailService = A.Fake<IMailService>();
        private readonly IConfiguration configuration = A.Fake<IConfiguration>();

        public UserServiceTest()
        {
            var config = new MapperConfiguration(config =>
            {
                config.AddProfile(new UserProfile());
            });

            this.mapper = new Mapper(config);
        }

        private void SetupService()
        {
            userService = new UserService(userQueryMock, mapper, userManager, signInManager, mailService, configuration);
        }

        [Fact]
        public void MapAllUsers_Returns_List_OfAllUserModel()
        {
            //Arrange
            int page = 1;
            int size = 20;
            bool isActive = false;
            SetupService();

            A.CallTo(() => userQueryMock.GetAllUsers(A<int>.Ignored, A<int>.Ignored, A<bool>.Ignored))
            .Returns(usersQueryMock);

            //Act
            var result = userService.MapAllUsers(page, size, isActive);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(listUserModelMock, result);
            A.CallTo(() => userQueryMock.GetAllUsers(page, size, isActive))
            .MustHaveHappenedOnceExactly();

        }

        [Fact]
        public void Register_Returns_SuccessResult()
        {
            //Arrange
            SetupService();
            //Act
            var result = userService.Register(registerModelMock);

            //Assert

            Assert.NotNull(result.Result);
            A.CallTo(() => userManager.CreateAsync(userMock))
            .Returns<Task<IdentityResult>>(result);

        }

        [Fact]
        public void Login_Returns_SuccessResult()
        {
            //Arrange
            var email = "test@abv.bg";
            var password = "testPassword";
            SetupService();
            A.CallTo(() => signInManager.PasswordSignInAsync(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored, A<bool>.Ignored))
            .Returns(new SignInResult());

            //Act
            var result = userService.Login(loginModelMock);

            //Assert

            Assert.NotNull(result.Result);
            A.CallTo(() => signInManager.PasswordSignInAsync(email, password, false, false))
            .Returns<Task<SignInResult>>(result);

        }

        [Fact]
        public void GetAllUsersCount_Returns_TheCountOfTheUsers()
        {
            //Arrange
            SetupService();
            A.CallTo(() => userQueryMock.GetAllUsersCount(A<bool>.Ignored))
            .Returns(5);

            //Act
            var result = userService.GetAllUsersCount(false);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(5, result);
            A.CallTo(() => userQueryMock.GetAllUsersCount(A<bool>.Ignored))
            .MustHaveHappenedOnceExactly();

        }

        [Fact]
        public void ApproveUser_ShouldMarkItAsActive()
        {
            //Arrange
            var userId = "test";
            SetupService();
            A.CallTo(() => userManager.FindByIdAsync(A<string>.Ignored))
            .Returns(userMock);

            //Act
            var result = userService.ApproveUser(userId);

            //Assert

            Assert.NotNull(result);
            A.CallTo(() => userManager.UpdateAsync(userMock))
            .MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.AddToRoleAsync(userMock, "Reader"))
            .MustHaveHappenedOnceExactly();
            Assert.IsType<Task<bool>>(result);

        }

        [Fact]
        public void ChangePassword_ShouldResetPasswordForTheUser()
        {
            //Arrange
            SetupService();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>.Ignored))
            .Returns(userMock);

            //Act
            var result = userService.ChangePassword(changePasswordModelMock);

            //Assert

            Assert.NotNull(result);
            A.CallTo(() => userManager.ResetPasswordAsync(userMock, changePasswordModelMock.ResetToken, changePasswordModelMock.NewPassword))
            .MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.ResetPasswordAsync(userMock, changePasswordModelMock.ResetToken, changePasswordModelMock.NewPassword))
            .Returns<Task<IdentityResult>>(result);
            Assert.IsType<Task<IdentityResult>>(result);

        }

        [Fact]
        public void SendForgotPasswordEmail_ShouldReturnTrueIfSuccess()
        {
            //Arrange
            SetupService();
            var email = "test@abv.bg";
            A.CallTo(() => userManager.FindByEmailAsync(A<string>.Ignored))
            .Returns(userMock);

            //Act
            var result = userService.SendForgotPasswordEmail(email);

            //Assert

            Assert.NotNull(result);
            A.CallTo(() => userManager.FindByEmailAsync(email))
            .MustHaveHappenedOnceExactly();
            Assert.IsType<Task<bool>>(result);
        }

        [Fact]
        public void GenerateJwtToken_ShouldCreateToken()
        {
            //Arrange
            SetupService();
            var email = "test@abv.bg";
            //Act
            var result = userService.GenerateJwtToken(email);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<Task<string>>(result);
        }

        [Fact]
        public void GetAllUsers_ShouldReturnAllUsersList()
        {
            //Arrange
            SetupService();

            int page = 1;
            int size = 20;
            bool isActive = false;

            //Act
            var result = userService.GetAllUsers(page, size, isActive);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<PaginatedUsersModel>(result);
        }
    }
}
