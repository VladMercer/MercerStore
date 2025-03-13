using Moq;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MercerStore.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IJwtProvider> _jwtProviderMock;
        private readonly Mock<ILogService> _logServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _jwtProviderMock = new Mock<IJwtProvider>();
            _logServiceMock = new Mock<ILogService>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["JwtOptions:ExpiresDays"]).Returns("30");

            _authService = new AuthService(
                _jwtProviderMock.Object,
                _configurationMock.Object,
                _logServiceMock.Object
            );
        }

        [Fact]
        public void GenerateGuestToken_ShouldGenerateTokenAndLogAction()
        {
            // Arrange
            var expectedToken = "generated_token";
            _jwtProviderMock.Setup(j => j.GenerateJwtToken(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                            .Returns(expectedToken);

            // Act
            var result = _authService.GenerateGuestToken();

            // Assert
            Assert.Equal(expectedToken, result);

            _jwtProviderMock
                .Verify(j => j.GenerateJwtToken(
                    It.IsAny<string>(),
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>()), Times.Once);

            _logServiceMock
                .Verify(l => l.LogUserAction(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<object>()), Times.Once);
        }
    }
}