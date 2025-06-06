namespace Invoice_api.Infraestructure.Dtos
{
    public class InvoiceToSaveDto
    {
        public long CustomerId { get; set; }
        public List<InvoiceDetailToSave> InvoiceDetails { get; set; } = new List<InvoiceDetailToSave>();
    }
}
