using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Models.Users;
using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Configuration;
using MercerStore.Web.Application.ViewModels.Users;
using Moq;

namespace MercerStore.Tests.Services 
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<IJwtProvider> _jwtProviderMock;
        private readonly Mock<IUserRepository> _userProfileRepositoryMock;
        private readonly Mock<ILogService> _logServiceMock;
        private readonly Mock<IUserIdentifierService> _userIdentifierServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            var store = new Mock<IUserStore<AppUser>>();

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
            { "JwtOptions:ExpiresDays", "30" }
            })
            .Build();

            _userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            _jwtProviderMock = new Mock<IJwtProvider>();
            _userProfileRepositoryMock = new Mock<IUserRepository>();
            _logServiceMock = new Mock<ILogService>();
            _userIdentifierServiceMock = new Mock<IUserIdentifierService>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(c => c["JwtOptions:ExpiresDays"]).Returns("30");

            _accountService = new AccountService(
                _userManagerMock.Object,
                _jwtProviderMock.Object,
                _userProfileRepositoryMock.Object,
                _logServiceMock.Object,
                _userIdentifierServiceMock.Object,
                configuration
            );
        }
        [Fact]
        public async Task LoginAsync_ReturnsToken_WhenCredentialsAreCorrect()
        {
            // Arrange
            var user = new AppUser { Id = "user123", Email = "test@example.com" };
            var loginModel = new LoginViewModel { EmailAddress = "test@example.com", Password = "Password123!" };

            _userManagerMock.Setup(um => um.FindByEmailAsync(loginModel.EmailAddress)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.GetUserIdAsync(user)).ReturnsAsync(user.Id);

            _userIdentifierServiceMock.Setup(service => service.GetUserRoles(user)).Returns(new List<string> { "User" });
            _userProfileRepositoryMock.Setup(repo => repo.GetUserPhotoUrl(user.Id)).ReturnsAsync("profile.jpg");
            _userProfileRepositoryMock.Setup(repo => repo.GetUserCreationDate(user.Id)).ReturnsAsync(System.DateTime.UtcNow);

            _jwtProviderMock.Setup(jwt => jwt.GenerateJwtToken(user.Id, It.IsAny<List<string>>(), "profile.jpg", It.IsAny<System.DateTime>()))
                            .Returns("mocked_token");

            // Act
            var result = await _accountService.LoginAsync(loginModel, "127.0.0.1");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("mocked_token", result.Data);
        }
        [Fact]
        public async Task LoginAsync_ReturnsFailure_WhenEmailNotFound()
        {
            // Arrange
            var loginModel = new LoginViewModel { EmailAddress = "wrong@example.com", Password = "Password123!" };

            _userManagerMock.Setup(um => um.FindByEmailAsync(loginModel.EmailAddress)).ReturnsAsync((AppUser)null);

            // Act
            var result = await _accountService.LoginAsync(loginModel, "127.0.0.1");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Wrong email address", result.ErrorMessage);
        }
        [Fact]
        public async Task LoginAsync_ReturnsFailure_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new AppUser { Id = "user123", Email = "test@example.com" };
            var loginModel = new LoginViewModel { EmailAddress = "test@example.com", Password = "WrongPassword!" };

            _userManagerMock.Setup(um => um.FindByEmailAsync(loginModel.EmailAddress)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(false);

            // Act
            var result = await _accountService.LoginAsync(loginModel, "127.0.0.1");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid password", result.ErrorMessage);
        }
        [Fact]
        public async Task RegisterUserAsync_ReturnsToken_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerModel = new RegisterViewModel { Email = "newuser@example.com", Password = "Password123!" };
            var newUser = new AppUser { Id = "user123", Email = "newuser@example.com" };

            _userManagerMock.Setup(um => um.FindByEmailAsync(registerModel.Email)).ReturnsAsync((AppUser)null);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), registerModel.Password))
                            .ReturnsAsync(IdentityResult.Success);

            _userIdentifierServiceMock.Setup(service => service.AddUserToRoleAsync(It.IsAny<AppUser>(), "User"))
                                      .ReturnsAsync(true);

            _jwtProviderMock.Setup(jwt => jwt.GenerateJwtToken(It.IsAny<string>(), It.IsAny<List<string>>(), null, It.IsAny<System.DateTime>()))
                            .Returns("mocked_token");

            // Act
            var result = await _accountService.RegisterUserAsync(registerModel, "127.0.0.1");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("mocked_token", result.Data);
        }
        [Fact]
        public async Task RegisterUserAsync_ReturnsFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            var registerModel = new RegisterViewModel { Email = "existing@example.com", Password = "Password123!" };
            var existingUser = new AppUser { Id = "user123", Email = "existing@example.com" };

            _userManagerMock.Setup(um => um.FindByEmailAsync(registerModel.Email)).ReturnsAsync(existingUser);

            // Act
            var result = await _accountService.RegisterUserAsync(registerModel, "127.0.0.1");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Email is already in use", result.ErrorMessage);
        }
    }
}
