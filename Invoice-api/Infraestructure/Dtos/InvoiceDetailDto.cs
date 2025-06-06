namespace Invoice_api.Infraestructure.Dtos
{
    public class InvoiceDetailDto
    {
        public long id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
