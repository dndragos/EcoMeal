using BlazorApp1.Entities;

namespace BlazorApp1.Services.Interfaces
{
    public interface IPackageService
    {
        public Task<List<Package>> GetAll();
        public Task AddAsync(Package package);
        public Task<Package?> GetByIdAsync(Guid id);
        public Task UpdateAsync(Guid id, Package updatedPackage);
        public Task DeleteAsync(Guid id);
    }
}
