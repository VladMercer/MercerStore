using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Models.Carts
{
    public class CartProduct : IEntity
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
