namespace MercerStore.Web.Application.Dtos.InvoiceDto
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string? CompanyName { get; set; }
        public string ManagerId { get; set; }
        public DateTime DateReceived { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? EditDate { get; set; }

    }
}
