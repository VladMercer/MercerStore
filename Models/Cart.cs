using System.Reflection.Metadata.Ecma335;

namespace MercerStore.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }

        public List<CartProduct> CartProducts { get; set; }
    }
}
