using BlazorApp1.Data;
using BlazorApp1.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Repositories.Interfaces
{
    public interface IBusinessRepository
    {
        public Task<List<Business>> GetAllAsync();

        public Task<List<Business>> GetPaginatedAsync(string? searchTerm, BusinessTypeEnum type, int skip, int take);

        public Task<Business?> GetById(Guid Id);

        public Task<List<Business>> GetByOwnerIdAsync(Guid ownerId);

        public  Task AddAsync(Business business);


        public Task DeleteAsync(Guid id);

        public Task SaveChangesAsync();
    }
}
