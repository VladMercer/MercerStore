using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Carts;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Services;
using Moq;

namespace MercerStore.Tests.Services 
{
    public class CartServiceTests
    {
        private readonly CartService _cartService;
        private readonly Mock<ICartProductRepository> _cartProductRepositoryMock;
        private readonly Mock<IRequestContextService> _requestContextServiceMock;
        private readonly Mock<IUserIdentifierService> _userIdentifierServiceMock;

        public CartServiceTests()
        {
            _cartProductRepositoryMock = new Mock<ICartProductRepository>();
            _requestContextServiceMock = new Mock<IRequestContextService>();
            _userIdentifierServiceMock = new Mock<IUserIdentifierService>();

            _cartService = new CartService(
                _cartProductRepositoryMock.Object,
                _requestContextServiceMock.Object,
                _userIdentifierServiceMock.Object);
        }
        [Fact]
        public async Task GetCartViewModel_ReturnsCorrectData()
        {
            // Arrange
            var cartItems = new List<CartProduct>
            {
                new()
                {
                    ProductId = 1,
                    Quantity = 2,
                    Product = new Product
                    {
                        Name = "Test Product",
                        MainImageUrl = "image.jpg",
                        ProductPricing = new ProductPricing { OriginalPrice = 100, FixedDiscountPrice = 80 }
                    }
                }
            };

            _cartProductRepositoryMock
                .Setup(repo => repo.GetCartItems(It.IsAny<string>()))
                .ReturnsAsync(cartItems);

            // Act
            var result = await _cartService.GetCartViewModel();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.CartItems);
            Assert.Equal(2, result.CartItemCount);
            Assert.Equal(160, result.CartTotalPrice);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public async Task AddToCart_CallsRepositoryWithCorrectProductId(int productId)
        {
            // Arrange
            _cartProductRepositoryMock
                .Setup(repo => repo.AddToCartProduct(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            await _cartService.AddToCart(productId);

            // Assert
            _requestContextServiceMock.Verify(service => service.SetLogDetails(It.IsAny<object>()), Times.Once);
            _cartProductRepositoryMock.Verify(repo => repo.AddToCartProduct(productId, It.IsAny<string>(), 1), Times.Once);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public async Task RemoveFromCart_CorrectRemoveProductById(int productId)
        {
            //Arrange
            var userId = "test-user-123";
            _userIdentifierServiceMock.Setup(u => u.GetCurrentIdentifier()).Returns(userId);

            _cartProductRepositoryMock
                .Setup(repo => repo.RemoveFromCartProduct(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            //Act 
            await _cartService.RemoveFromCart(productId);

            //Assert
            _requestContextServiceMock.Verify(service => service.SetLogDetails(It.IsAny<object>()), Times.Once);
            _cartProductRepositoryMock.Verify(repo => repo.RemoveFromCartProduct(productId, userId), Times.Once);

        }
    }
}