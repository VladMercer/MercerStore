namespace MercerStore.Web.Application.Models.Invoices;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCompany { get; set; }
    public string ContactPerson { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string TaxId { get; set; }
    public IList<Invoice> Invoices { get; set; }
}
