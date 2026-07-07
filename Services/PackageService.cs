using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using BlazorApp1.Services.Interfaces;

namespace BlazorApp1.Services
{
    public class PackageService(IPackageRepository packageRepository) : IPackageService
    {
        public async Task<List<Package>> GetAll()
        {
            return await packageRepository.GetAllAsync();
        }

        public async Task AddAsync(Package package)
        {
            await packageRepository.AddAsync(package);
            await packageRepository.SaveChangesAsync();
        }
    }
}
