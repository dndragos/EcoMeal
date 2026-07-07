using BlazorApp1.Data;
using BlazorApp1.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Repositories.Interfaces
{
    public interface IBusinessRepository
    {
        public Task<List<Business>> GetAllAsync();

        public Task<Business?> GetById(Guid Id);

        public  Task AddAsync(Business business);


        public Task DeleteAsync(Guid id);

        public Task SaveChangesAsync();
    }
}
