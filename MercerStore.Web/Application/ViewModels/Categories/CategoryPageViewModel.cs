using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.ViewModels.Categories
{
    public class CategoryPageViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int SelectedCategoryId { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public string CurrentFilter { get; set; }
    }
}
