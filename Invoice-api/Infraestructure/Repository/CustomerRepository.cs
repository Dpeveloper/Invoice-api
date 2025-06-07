using Invoice_api.Domain.Entities;
using Invoice_api.Infraestructure.Persistence;
using Invoice_api.Infraestructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Invoice_api.Infraestructure.Repository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateAsync(Customer entity)
        {
            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Customer?> FindByIdAsync(long id)
        {
            return await _context.Customers
                .Include(c => c.Invoices)
                .ThenInclude(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.Include(c => c.Invoices).ToListAsync();
        }

        public async Task<Customer> UpdateAsync(Customer entity)
        {
            _context.Customers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
