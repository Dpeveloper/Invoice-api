namespace Invoice_api.Infraestructure.Dtos
{
    public class CustomerDto
    {
        public long CustomerId { get; set; }
        public string Name { get; set; } = default!;
        public int CustomerNumber { get; set; }
        public string Location { get; set; } = default!;
        public ICollection<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();
    }

}
