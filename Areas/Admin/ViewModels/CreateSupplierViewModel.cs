using MercerStore.Models.Invoice;
using System.ComponentModel.DataAnnotations;

namespace MercerStore.Areas.Admin.ViewModels
{
    public class CreateSupplierViewModel
    {
        [Required]
        [Display(Name = "Название поставщика")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Юр. лицо / Физ. лицо")]
        public bool IsCompany { get; set; }
        [Required]
        [Display(Name = "Контактное лицо")]
        public string ContactPerson { get; set; }
        [Required]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "ИНН")]
        public string TaxId { get; set; }
    }
}
