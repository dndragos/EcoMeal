using BlazorApp1.Entities;

using BlazorApp1.Entities;

namespace BlazorApp1.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAll();
        public Task AddAsync(Order order);
        public Task<Order?> GetByIdAsync(Guid id);
        public Task UpdateStatusAsync(Guid id, StatusEnum newStatus);
        public Task CancelOrderWithRestorationAsync(Guid orderId);
        public Task DeleteAsync(Guid id);

        public Task<Order?> GetPendingOrderForUserAsync(Guid userId);

        public Task<List<Order>> GetFinishedOrdersForUserAsync(Guid userId, int month, int year);

        public Task<string?> AddPackageToPendingOrderAsync(Guid userId, Guid packageId, int quantity);

        public Task<string?> RemovePackageFromPendingOrderAsync(Guid userId, Guid packageId);
        
        public Task<string?> UpdatePackageQuantityAsync(Guid userId, Guid packageId, int newQuantity);
    }
}
