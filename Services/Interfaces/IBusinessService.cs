using BlazorApp1.Entities;
using BlazorApp1.Repositories;

namespace BlazorApp1.Services.Interfaces
{
    public interface IBusinessService
    {
        public Task<List<Business>> GetAll();
        public Task AddAsync(Business business);
    }
}
