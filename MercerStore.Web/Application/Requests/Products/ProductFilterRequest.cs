using MercerStore.Web.Infrastructure.Data.Enum.Product;

namespace MercerStore.Web.Application.Requests.Products;

public record ProductFilterRequest(
    int? CategoryId,
    int PageNumber,
    int PageSize,
    AdminProductSortOrder? SortOrder,
    AdminProductFilter? Filter);