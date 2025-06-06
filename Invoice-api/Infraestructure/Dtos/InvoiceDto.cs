namespace Invoice_api.Infraestructure.Dtos
{
    public class InvoiceDto
    {
        public long InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public CustomerDto Customer { get; set; }
        public List<InvoiceDetailDto> InvoiceDetails { get; set; } = new List<InvoiceDetailDto>();
    }
}
