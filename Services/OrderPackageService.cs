using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using BlazorApp1.Services.Interfaces;

namespace BlazorApp1.Services
{
    public class OrderPackageService(IOrderPackageRepository orderPackageRepository) : IOrderPackageService
    {
        public async Task<List<OrderPackage>> GetAll()
        {
            return await orderPackageRepository.GetAllAsync();
        }

        public async Task AddAsync(OrderPackage orderPackage)
        {
            await orderPackageRepository.AddAsync(orderPackage);
            await orderPackageRepository.SaveChangesAsync();
        }

        public async Task<OrderPackage?> GetByIdAsync(Guid id)
        {
            return await orderPackageRepository.GetById(id);
        }

        public async Task UpdateAsync(Guid id, OrderPackage updatedOrderPackage)
        {
            var orderPackage = await orderPackageRepository.GetById(id);
            if (orderPackage != null)
            {
                orderPackage.OrderId = updatedOrderPackage.OrderId;
                orderPackage.PackageId = updatedOrderPackage.PackageId;
                orderPackage.Quantity = updatedOrderPackage.Quantity;

                await orderPackageRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await orderPackageRepository.DeleteAsync(id);
            await orderPackageRepository.SaveChangesAsync();
        }
    }
}
