using BlazorApp1.Entities;

namespace BlazorApp1.Repositories.Interfaces
{
    public interface IPackageRepository
    {
        public Task<List<Package>> GetAllAsync();
        public Task<Package?> GetById(Guid Id);
        public Task AddAsync(Package package);
        public Task DeleteAsync(Guid id);
        public Task SaveChangesAsync();
    }
}
