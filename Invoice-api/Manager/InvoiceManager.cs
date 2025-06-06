using Invoice_api.Domain;
using Invoice_api.Domain.Entities;
using Invoice_api.Infraestructure.Dtos;
using Invoice_api.Infraestructure.Repository;
using Invoice_api.Infraestructure.Repository.Interface;

namespace Invoice_api.Manager
{
    public class InvoiceManager
    {
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<Customer> _customerRepository;

        public InvoiceManager(IRepository<Invoice> invoiceRepository, IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceToSaveDto invoiceDto)
        {
            if (invoiceDto.InvoiceDetails == null || invoiceDto.InvoiceDetails.Count == 0)
            {
                throw new ArgumentException("La factura debe contener al menos un detalle.");
            }

            if (invoiceDto.CustomerId <= 0)
            {
                throw new ArgumentException("Debe especificarse un ID de cliente válido.");
            }

            var customer = await _customerRepository.FindByIdAsync(invoiceDto.CustomerId);
            if (customer == null)
            {
                throw new ArgumentException("El cliente especificado no existe.");
            }

            double calculatedTotal = 0;

            foreach (var detail in invoiceDto.InvoiceDetails)
            {
                if (detail.Quantity <= 0)
                {
                    throw new ArgumentException($"La cantidad del producto '{detail.ProductName}' debe ser mayor a 0.");
                }

                if (detail.UnitPrice <= 0)
                {
                    throw new ArgumentException($"El precio unitario del producto '{detail.ProductName}' debe ser mayor a 0.");
                }

                double subtotal = detail.Quantity * detail.UnitPrice;
                calculatedTotal += subtotal;
            }


            Invoice invoice = ToEntity(invoiceDto);
            invoice.Date = DateTime.UtcNow;
            invoice.Customer = customer;
            invoice.Total = calculatedTotal;

            invoice = await _invoiceRepository.CreateAsync(invoice);
            return ToDto(invoice);
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(long id)
        {
            var invoice = await _invoiceRepository.FindByIdAsync(id);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"No se encontró ninguna factura con el ID {id}.");
            }

            return ToDto(invoice);
        }

        public async Task<InvoiceDto> UpdateInvoiceAsync(InvoiceToSaveDto invoiceDto, long id)
        {
            var existingInvoice = await _invoiceRepository.FindByIdAsync(id);
            if (existingInvoice == null)
            {
                throw new KeyNotFoundException($"No se encontró ninguna factura con el ID {id}.");
            }

            // Validar detalles de la factura
            if (invoiceDto.InvoiceDetails == null || invoiceDto.InvoiceDetails.Count == 0)
            {
                throw new ArgumentException("La factura debe contener al menos un detalle.");
            }

            double calculatedTotal = invoiceDto.InvoiceDetails.Sum(d => d.Quantity * d.UnitPrice);

            // Actualizar campos modificables
            existingInvoice.Total = calculatedTotal;
            existingInvoice.CustomerId = invoiceDto.CustomerId > 0 ? invoiceDto.CustomerId : existingInvoice.CustomerId;

            existingInvoice.InvoiceDetails = invoiceDto.InvoiceDetails.Select(d => new InvoiceDetail
            {
                Quantity = d.Quantity,
                SubTotal = d.UnitPrice,
                ProductName = d.ProductName
            }).ToList();

            existingInvoice = await _invoiceRepository.UpdateAsync(existingInvoice);

            return ToDto(existingInvoice);
        }


        public async Task<bool> DeleteInvoiceAsync(long id)
        {
            return await _invoiceRepository.DeleteAsync(id);
        }

        private Invoice ToEntity(InvoiceToSaveDto invoiceDto)
        {
            return new Invoice
            {
                CustomerId = invoiceDto.CustomerId,
                InvoiceDetails = invoiceDto.InvoiceDetails.Select(d => new InvoiceDetail
                {
                    Quantity = d.Quantity,
                    Unitprice = d.UnitPrice,
                    ProductName = d.ProductName
                }).ToList()
            };
        }

        private InvoiceDto ToDto(Invoice invoice)
        {
            return new InvoiceDto
            {
                InvoiceId = invoice.InvoiceId,
                Date = invoice.Date,
                Total = invoice.Total,
                Customer = new CustomerDto
                {
                    CustomerId = invoice.Customer.CustomerId,
                    Name = invoice.Customer.CustomerName,
                    CustomerNumber = invoice.Customer.CustomerNumber,
                    Location = invoice.Customer.Location
                },
                InvoiceDetails = invoice.InvoiceDetails.Select(d => new InvoiceDetailDto
                {
                    ProductName = d.ProductName,
                    Quantity = d.Quantity,
                    UnitPrice = d.Unitprice
                }).ToList()
            };
        }
    }
}
