using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Invoice_api.Domain.Entities
{
    [Table(name:"invoices")]
    public class Invoice
    {
        [Key]
        public long InvoiceId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "The field {0} must be greater than {1}."), Range(1,double.MaxValue)]
        public double Total { get; set; }
        [Required(ErrorMessage = "The field cliente must be save"), NotNull]
        public long CustomerId { get; set; }
        [Required(ErrorMessage = "The invoced must be have minimum one invocice detail"), MinLength(1)]

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }
}
