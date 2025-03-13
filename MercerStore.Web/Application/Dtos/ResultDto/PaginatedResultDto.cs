using System.Text.Json.Serialization;


namespace MercerStore.Web.Application.Dtos.ResultDto
{
    public class PaginatedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public PaginatedResultDto(IEnumerable<T> items, int totalItems, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        }
    }
}