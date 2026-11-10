using BlazorApp1.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<List<Order>> GetAllAsync();

        public  Task<Order?> GetById(Guid Id);

        public Task AddAsync(Order order);

        public Task DeleteAsync(Guid id);

        public Task DeleteAllOrdersForUserAsync(Guid userId);

        public Task<Order?> GetPendingOrderForUserAsync(Guid userId);

        public Task<List<Order>> GetFinishedOrdersForUserAsync(Guid userId, int month, int year);

        public Task<List<Order>> GetRecentOrdersForUserAsync(Guid userId, int limit);

        public Task<int> GetTotalOrdersCountForUserAsync(Guid userId);

        public Task SaveChangesAsync();
        
        public Task UpdateOrderStatusAsync(Guid orderId, StatusEnum newStatus);
        public Task IncrementOrderPackageQuantityAsync(Guid orderPackageId, int incrementQuantity);
        public Task SetOrderPackageQuantityAsync(Guid orderPackageId, int newQuantity);
        public Task AddOrderPackageAsync(OrderPackage orderPackage);
        public Task RemoveOrderPackageAsync(Guid orderPackageId);
        public Task DeleteOrderDirectlyAsync(Guid orderId);
        public Task ClearTrackerAsync();
    }
}
