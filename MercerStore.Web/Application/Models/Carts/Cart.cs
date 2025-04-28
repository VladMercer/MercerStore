using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Models.Carts;

public class Cart : IEntity
{
    public string AppUserId { get; set; }
    public IList<CartProduct>? CartProducts { get; set; }
    public int Id { get; set; }
}