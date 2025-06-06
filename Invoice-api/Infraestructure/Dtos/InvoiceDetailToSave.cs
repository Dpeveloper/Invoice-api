namespace Invoice_api.Infraestructure.Dtos
{
    public class InvoiceDetailToSave
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}