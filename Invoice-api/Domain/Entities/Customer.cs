using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice_api.Domain.Entities
{
    [Table("customers")]
    public class Customer
    {
        [Key]
        public long CustomerId {  get; set; }
        [Required(ErrorMessage = "You can't register a client without his name")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The name of client is too short")]
        public string CustomerName { get; set; } = default!;
        [Required(ErrorMessage = "You can't register a client without his number"), Range(10, 100)]
        public int CustomerNumber { get; set; }
        [Required(ErrorMessage = "You can't register a client without his Location")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The location is too short")]
        public string Location { get; set; } = default!;
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}