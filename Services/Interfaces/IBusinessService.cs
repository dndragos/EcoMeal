using BlazorApp1.Entities;
using BlazorApp1.Repositories;

namespace BlazorApp1.Services.Interfaces
{
    public interface IBusinessService
    {
        public Task<List<Business>> GetAll();
        public Task<List<Business>> GetPaginatedAsync(string? searchTerm, BusinessTypeEnum type, int skip, int take);
        public Task AddAsync(Business business);
        public Task<Business?> GetByIdAsync(Guid id);
        public Task UpdateAsync(Guid id, Business updatedBusiness);
        public Task DeleteAsync(Guid id);

    }
}
