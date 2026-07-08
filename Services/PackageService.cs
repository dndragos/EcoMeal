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
        public async Task<Package?> GetByIdAsync(Guid id)
        {
            return await packageRepository.GetById(id);
        }

        public async Task UpdateAsync(Guid id, Package updatedPackage)
        {
            var package = await packageRepository.GetById(id);
            if (package != null)
            {
                package.Name = updatedPackage.Name;
                package.Description = updatedPackage.Description;
                package.ImageUrl = updatedPackage.ImageUrl;
                package.Price = updatedPackage.Price;
                package.Quantity = updatedPackage.Quantity;
                package.PickupStart = updatedPackage.PickupStart;
                package.PickupEnd = updatedPackage.PickupEnd;
                package.PackageTypeId = updatedPackage.PackageTypeId;
                package.BusinessId = updatedPackage.BusinessId;

                await packageRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await packageRepository.DeleteAsync(id);
            await packageRepository.SaveChangesAsync();
        }
    }
}
