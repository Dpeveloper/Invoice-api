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
            if (string.IsNullOrWhiteSpace(customerDto.Name) || customerDto.CustomerNumber < 10)
            {
                throw new ArgumentException("El nombre del cliente es requerido y el número debe ser válido.");
            }

            Customer customer = ToEntity(customerDto);
            customer = await _customerRepository.CreateAsync(customer);
            return ToDto(customer);
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(long id)
        {
            var customer = await _customerRepository.FindByIdAsync(id);
            if (customer == null)
            {
                throw new KeyNotFoundException($"No se encontró ningún cliente con el ID {id}.");
            }

            return ToDto(customer);
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(ToDto);
        }

        public async Task<CustomerDto> UpdateCustomerAsync(CustomerToSaveDto customerDto, long id)
        {
            var existingCustomer = await _customerRepository.FindByIdAsync(id);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"No se encontró ningún cliente con el ID {id}.");
            }

            existingCustomer.CustomerNumber = customerDto.CustomerNumber;
            existingCustomer.CustomerName = customerDto.Name;
            existingCustomer.Location = customerDto.Location;

            existingCustomer = await _customerRepository.UpdateAsync(existingCustomer);

            return ToDto(existingCustomer);
        }

        public async Task<bool> DeleteCustomerAsync(long id)
        {
            return await _customerRepository.DeleteAsync(id);
        }

        private Customer ToEntity(CustomerToSaveDto customerDto)
        {
            return new Customer
            {
                CustomerName = customerDto.Name,
                CustomerNumber = customerDto.CustomerNumber,
                Location = customerDto.Location
            };
        }

        private CustomerDto ToDto(Customer customer)
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
                    Total = i.Total
                }).ToList()
            };
        }
    }
}
