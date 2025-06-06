using Invoice_api.Domain.Entities;
using Invoice_api.Infraestructure.Persistence;
using Invoice_api.Infraestructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Invoice_api.Infraestructure.Repository
{
    public class InvoiceRepository : IRepository<Invoice>
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> CreateAsync(Invoice entity)
        {
            await _context.Invoices.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Invoices.FindAsync(id);
            if (entity == null) return false;

            _context.Invoices.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Invoice> FindByIdAsync(long id)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await _context.Invoices
                .Include(x => x.Customer)
                .Include(x => x.InvoiceDetails)
                .ToListAsync();
        }

        public async Task<Invoice> UpdateAsync(Invoice entity)
        {
            _context.Invoices.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }

}
