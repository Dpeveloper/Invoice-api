using Invoice_api.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice_api.Domain
{
    [Table(name:"invoiceDetails")]
    public class InvoiceDetail
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "The product can't be empty")]
        [StringLength(200, MinimumLength =  5, ErrorMessage = "The name of product is too short")]
        public string ProductName { get; set; } = default!;
        [Required(ErrorMessage = "You can't register a product without quantity avaiable"), Range(1,int.MaxValue)]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "You can't register a product without unit price"), Range(1, int.MaxValue)]
        public double Unitprice { get; set; }
        [Required(ErrorMessage = "You can't register a product without sub"), Range(1, int.MaxValue)]
        public double SubTotal { get; set; }
        public long InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; } = default!;
    }
}