using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Services;
using Moq;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.UserDto;
using MercerStore.Web.Application.Requests.Users;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.Models.Orders;
using CloudinaryDotNet.Actions;
using MercerStore.Web.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;

namespace MercerStore.Tests.Services
{

    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRedisCacheService> _redisCacheServiceMock;
        private readonly Mock<IPhotoService> _photoServiceMock;
        private readonly Mock<IRequestContextService> _requestContextServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _redisCacheServiceMock = new Mock<IRedisCacheService>();
            _photoServiceMock = new Mock<IPhotoService>();
            _requestContextServiceMock = new Mock<IRequestContextService>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _redisCacheServiceMock.Object,
                _photoServiceMock.Object,
                _userRepositoryMock.Object,
                _requestContextServiceMock.Object
            );
        }
        [Fact]
        public async Task GetFilteredUsers_ShouldReturnCachedData()
        {
            // Arrange
            var request = new UserFilterRequest(1, 30, null, null, null, "");
            var cachedResult = new PaginatedResultDto<UserDto>(
                new List<UserDto> { new() { Id = "1", Email = "user.example@gmail.com" } },
                totalItems: 1,
                pageSize: 30
            );

            _redisCacheServiceMock
                .Setup(s => s.TryGetOrSetCacheAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<PaginatedResultDto<UserDto>>>>(),
                    true,
                    TimeSpan.FromMinutes(10)
                ))
                .ReturnsAsync(cachedResult);

            // Act
            var result = await _userService.GetFilteredUsers(request);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal("1", result.Items.First().Id);

            _redisCacheServiceMock.Verify(s => s.TryGetOrSetCacheAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<PaginatedResultDto<UserDto>>>>(),
                true,
                TimeSpan.FromMinutes(10)), Times.Once);
        }
        [Fact]
        public async Task GetUserProfile_ShouldReturnUserProfile_WhenUserExists()
        {
            // Arrange
            var userId = "123";
            var user = new AppUser
            {
                Id = userId,
                UserName = "TestUser",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                UserImgUrl = "https://example.com/image.jpg",
                Address = "Test Address",
                Orders = new List<Order>(),
                Reviews = new List<Review>(),
                DateCreated = DateTime.UtcNow
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserProfile(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("TestUser", result.UserName);
            Assert.Equal("test@example.com", result.EmailAddress);
        }

        [Fact]
        public async Task GetUserProfile_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((AppUser)null);

            // Act
            var result = await _userService.GetUserProfile("unknown");

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task UpdateUserProfile_ShouldUpdateUserProfile_WhenUserExists()
        {
            // Arrange
            var userId = "123";
            var user = new AppUser
            {
                Id = userId,
                UserName = "OldUser",
                Email = "old@example.com",
                UserImgUrl = "https://example.com/old.jpg"
            };

            var updateUserProfile = new UserProfileViewModel
            {
                Id = userId,
                UserName = "NewUser",
                EmailAddress = "new@example.com",
                UserImage = new FormFile(null, 0, 0, null, "image.jpg")
            };

            var photoResult = new ImageUploadResult { Url = new Uri("https://example.com/new.jpg") };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsyncNoTracking(userId))
                .ReturnsAsync(user);

            _photoServiceMock
                .Setup(photo => photo.AddPhotoAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(photoResult);

            // Act
            var result = await _userService.UpdateUserProfile(updateUserProfile);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(userId, result.Data);
            _userRepositoryMock.Verify(repo => repo.UpdateUserProfile(It.IsAny<AppUser>()), Times.Once);
        }
        [Fact]
        public async Task GetUpdateUserProfileViewModel_ShouldReturnViewModel_WhenUserExists()
        {
            // Arrange
            var userId = "123";
            var user = new AppUser
            {
                Id = userId,
                UserName = "TestUser",
                Email = "test@example.com",
                Address = "Test Address",
                Orders = new List<Order> { new() },
                Reviews = new List<Review> { new() },
                DateCreated = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUpdateUserProfileViewModel(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("TestUser", result.UserName);
            Assert.Single(result.Orders);
            Assert.Single(result.Reviews);
        }
    }
}
