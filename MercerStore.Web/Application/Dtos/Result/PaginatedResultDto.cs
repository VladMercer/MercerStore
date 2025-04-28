namespace MercerStore.Web.Application.Dtos.Result;

public class PaginatedResultDto<T>
{
    public PaginatedResultDto(IEnumerable<T> items, int totalItems, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
    }

    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
