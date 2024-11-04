using MercerStore.Models;
using Microsoft.Build.Evaluation;

namespace MercerStore.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? SKU { get; set; }
        public string MainImageUrl { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
       
    }
}
