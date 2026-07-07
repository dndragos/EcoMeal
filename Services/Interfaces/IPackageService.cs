using BlazorApp1.Entities;

namespace BlazorApp1.Services.Interfaces
{
    public interface IPackageService
    {
        public Task<List<Package>> GetAll();
        public Task AddAsync(Package package);
    }
}
