using BlazorApp1.Entities;

namespace BlazorApp1.Services.Interfaces
{
    public interface IOrderPackageService
    {
        public Task<List<OrderPackage>> GetAll();
        public Task AddAsync(OrderPackage orderPackage);
        public Task<OrderPackage?> GetByIdAsync(Guid id);
        public Task UpdateAsync(Guid id, OrderPackage updatedOrderPackage);
        public Task DeleteAsync(Guid id);
    }
}
