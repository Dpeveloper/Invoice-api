using Invoice_api.Domain.Entities;
using Invoice_api.Infraestructure.Dtos;
using Invoice_api.Infraestructure.Repository.Interface;

namespace Invoice_api.Manager
{
    public class CustomerManager
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerManager(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerToSaveDto customerDto)
        {
            ValidateCustomerToSave(customerDto);

            var customer = MapToEntity(customerDto);
            var created = await _customerRepository.CreateAsync(customer);

            return MapToDto(created);
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(long id)
        {
            var customer = await _customerRepository.FindByIdAsync(id)
                ?? throw new KeyNotFoundException($"Cliente con ID {id} no encontrado.");

            return MapToDto(customer);
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(MapToDto);
        }

        public async Task<CustomerDto> UpdateCustomerAsync(CustomerToSaveDto customerDto, long id)
        {
            ValidateCustomerToSave(customerDto);

            var existing = await _customerRepository.FindByIdAsync(id)
                ?? throw new KeyNotFoundException($"Cliente con ID {id} no encontrado.");

            existing.CustomerName = customerDto.Name;
            existing.CustomerNumber = customerDto.CustomerNumber;
            existing.Location = customerDto.Location;

            var updated = await _customerRepository.UpdateAsync(existing);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteCustomerAsync(long id)
        {
            return await _customerRepository.DeleteAsync(id);
        }

        private static void ValidateCustomerToSave(CustomerToSaveDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("El nombre del cliente es requerido.");

            if (dto.CustomerNumber < 10)
                throw new ArgumentException("El número del cliente debe ser mayor o igual a 10.");
        }

        private static Customer MapToEntity(CustomerToSaveDto dto)
        {
            return new Customer
            {
                CustomerName = dto.Name,
                CustomerNumber = dto.CustomerNumber,
                Location = dto.Location
            };
        }

        private static CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.CustomerName,
                CustomerNumber = customer.CustomerNumber,
                Location = customer.Location,
                Invoices = customer.Invoices.Select(i => new InvoiceDto
                {
                    InvoiceId = i.InvoiceId,
                    Date = i.Date,
                    Total = i.Total,
                    InvoiceDetails = i.InvoiceDetails.Select(d => new InvoiceDetailDto
                    {
                        id = d.Id,
                        ProductName = d.ProductName,
                        Quantity = d.Quantity,
                        UnitPrice = d.Unitprice
                    }).ToList()
                }).ToList()
            };
        }
    }
}
