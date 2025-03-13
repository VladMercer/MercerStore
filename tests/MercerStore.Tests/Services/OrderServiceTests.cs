using MercerStore.Web.Application.Dtos.OrderDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Application.ViewModels.Carts;
using MercerStore.Web.Infrastructure.Data.Enum.Order;
using Moq;

namespace MercerStore.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IRequestContextService> _requestContextServiceMock;
        private readonly Mock<IRedisCacheService> _redisCacheServiceMock;
        private readonly Mock<IUserIdentifierService> _userIdentifierServiceMock;
        private readonly Mock<ICartProductRepository> _cartProductRepositoryMock;
        private readonly Mock<IUserRepository> _userProfileRepositoryMock;
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _requestContextServiceMock = new Mock<IRequestContextService>();
            _redisCacheServiceMock = new Mock<IRedisCacheService>();
            _userIdentifierServiceMock = new Mock<IUserIdentifierService>();
            _cartProductRepositoryMock = new Mock<ICartProductRepository>();
            _userProfileRepositoryMock = new Mock<IUserRepository>();
            _cartServiceMock = new Mock<ICartService>();

            _orderService = new OrderService(
                _orderRepositoryMock.Object,
                _requestContextServiceMock.Object,
                _redisCacheServiceMock.Object,
                _userIdentifierServiceMock.Object,
                _cartProductRepositoryMock.Object,
                _userProfileRepositoryMock.Object,
                _cartServiceMock.Object
            );
        }
        [Fact]
        public async Task GetFilteredOrders_ShouldReturnOrders_WhenCacheIsEnabled()
        {
            // Arrange
            var request = new OrderFilterRequest { PageNumber = 1, PageSize = 30, Query = "" };
            var expectedOrders = new PaginatedResultDto<OrderDto>(
                new List<OrderDto> { new OrderDto { Id = 1, Address = "Test Address" } }, 1, 30);

            _redisCacheServiceMock
                .Setup(x => x.TryGetOrSetCacheAsync(It.IsAny<string>(), It.IsAny<Func<Task<PaginatedResultDto<OrderDto>>>>(), true, It.IsAny<TimeSpan>()))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _orderService.GetFilteredOrders(request);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(1, result.Items.First().Id);
        }
        [Fact]
        public async Task GetOrderById_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var expectedOrder = new Order { Id = orderId, Address = "Test Address" };

            _orderRepositoryMock.Setup(repo => repo.GetOrderById(orderId))
                .ReturnsAsync(expectedOrder);

            // Act
            var result = await _orderService.GetOrderById(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.Id);
            Assert.Equal("Test Address", result.Address);
        }
        [Fact]
        public async Task UpdateOrderStatus_ShouldUpdateOrderStatus()
        {
            // Arrange
            var orderId = 1;
            var newStatus = OrderStatuses.Completed;
            var order = new Order { Id = orderId, Status = OrderStatuses.Pending };

            _orderRepositoryMock.Setup(repo => repo.GetOrderById(orderId))
                .ReturnsAsync(order);
            _orderRepositoryMock.Setup(repo => repo.UpdateOrder(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            var dto = new UpdateOrderStatusDto { OrderId = orderId, Status = newStatus };

            // Act
            await _orderService.UpdateOrderStatus(dto);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.UpdateOrder(It.Is<Order>(o => o.Status == newStatus)), Times.Once);
        }
        [Fact]
        public async Task RemoveOrder_ShouldDeleteOrder_WhenCalled()
        {
            // Arrange
            var orderId = 1;
            _orderRepositoryMock.Setup(repo => repo.DeleteOrder(orderId))
                .Returns(Task.CompletedTask);

            // Act
            await _orderService.RemoveOrder(orderId);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.DeleteOrder(orderId), Times.Once);
        }
        [Fact]
        public async Task GetOrderViewModel_ShouldReturnOrderViewModel()
        {
            // Arrange
            var userId = "123";
            _userIdentifierServiceMock.Setup(service => service.GetCurrentIdentifier())
            .Returns(userId);

            var user = new AppUser { Id = userId, Email = "test@example.com", Address = "Test Street", PhoneNumber = "123456789" };
            _userProfileRepositoryMock.Setup(repo => repo.GetUserByIdAsyncNoTracking(userId))
                .ReturnsAsync(user);

            var cartViewModel = new CartViewModel { CartTotalPrice = 100 };
            _cartServiceMock.Setup(service => service.GetCartViewModel())
                .ReturnsAsync(cartViewModel);

            // Act
            var result = await _orderService.GetOrderViewModel();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("Test Street", result.Address);
            Assert.Equal("123456789", result.PhoneNumber);
            Assert.Equal(100, result.CartViewModel.CartTotalPrice);
        }
    }
}
