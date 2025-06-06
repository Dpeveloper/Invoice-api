using Invoice_api.Domain.Entities;

namespace Invoice_api.Infraestructure.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<T> FindByIdAsync(long id);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(long id);
    }
}
