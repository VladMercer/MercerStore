using System.ComponentModel.DataAnnotations;

namespace MercerStore.Models
{
	public class Order
	{
        public int OrderId { get; set; }
        public string PhoneNumber { get; set; }
        public string?Email { get; set; }
        public string Address { get; set; }


		public string? UserId { get; set; }
		public string? GuestId { get; set; }


        public int OrderProductListId { get; set; }
        public OrderProductList OrderProductList { get; set; }


        public DateTime Date { get; set; }
	}
}
