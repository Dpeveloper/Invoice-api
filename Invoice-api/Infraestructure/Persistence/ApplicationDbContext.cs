using Invoice_api.Domain;
using Invoice_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Invoice_api.Infraestructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoicesDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
