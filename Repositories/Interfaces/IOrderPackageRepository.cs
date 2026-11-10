using BlazorApp1.Entities;

namespace BlazorApp1.Repositories.Interfaces
{
    public interface IOrderPackageRepository
    {
        public Task<List<OrderPackage>> GetAllAsync();

        public Task<OrderPackage?> GetById(Guid Id);

        public Task AddAsync(OrderPackage orderPackage);

        public Task DeleteAsync(Guid id);

        public Task SaveChangesAsync();
    }
}
