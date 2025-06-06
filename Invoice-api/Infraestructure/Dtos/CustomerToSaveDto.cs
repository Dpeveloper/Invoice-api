namespace Invoice_api.Infraestructure.Dtos
{
    public class CustomerToSaveDto
    {
        public string Name { get; set; } = default!;
        public int CustomerNumber { get; set; }
        public string Location { get; set; } = default!;
    }
}
