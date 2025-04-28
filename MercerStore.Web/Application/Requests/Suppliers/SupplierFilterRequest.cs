namespace MercerStore.Web.Application.Requests.Suppliers;

public record SupplierFilterRequest(int PageNumber, int PageSize, string? Query);