using BlazorApp1.Data;
using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Repositories
{
    public class OrderRepository(EcoMealDbContext context) : IOrderRepository
    {
        public async Task<List<Order>> GetAllAsync()
        {
            return await context.Orders
                .Include(o => o.User)
                .Include(o => o.Business)
                .Include(o => o.Status)
                .ToListAsync();
        }

        public async Task<Order?> GetById(Guid Id)
        {
            return await context.Orders
                .Include(o => o.User)
                .Include(o => o.Business)
                .Include(o => o.Status)
                .Include(o => o.OrderPackages)
                .FirstOrDefaultAsync(o => o.Id == Id);
        }

        public async Task AddAsync(Order order)
        {
            await context.Orders.AddAsync(order);
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await context.Orders
                .Include(o => o.OrderPackages)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return;

            context.OrderPackages.RemoveRange(order.OrderPackages);
            context.Orders.Remove(order);
        }

        public async Task DeleteAllOrdersForUserAsync(Guid userId)
        {
            var orders = await context.Orders
                .Include(o => o.OrderPackages)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            var orderPackages = orders.SelectMany(o => o.OrderPackages).ToList();

            context.OrderPackages.RemoveRange(orderPackages);
            context.Orders.RemoveRange(orders);
            await context.SaveChangesAsync();
        }

        public async Task<Order?> GetPendingOrderForUserAsync(Guid userId)
        {
            return await context.Orders
                .Include(o => o.Business)
                .Include(o => o.OrderPackages)
                    .ThenInclude(op => op.Package)
                .FirstOrDefaultAsync(o => o.UserId == userId && (o.StatusId == StatusEnum.Pending || o.StatusId == StatusEnum.Approved));
        }

        public async Task<List<Order>> GetFinishedOrdersForUserAsync(Guid userId, int month, int year)
        {
            return await context.Orders
                .Include(o => o.Business)
                .Include(o => o.OrderPackages)
                    .ThenInclude(op => op.Package)
                .Where(o => o.UserId == userId && 
                           (o.StatusId == StatusEnum.Canceled || o.StatusId == StatusEnum.Delivered) &&
                           o.CreatedAt.Month == month && o.CreatedAt.Year == year)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Order>> GetRecentOrdersForUserAsync(Guid userId, int limit)
        {
            return await context.Orders
                .Include(o => o.Business)
                .Include(o => o.Status)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> GetTotalOrdersCountForUserAsync(Guid userId)
        {
            return await context.Orders.CountAsync(o => o.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, StatusEnum newStatus)
        {
            await context.Orders
                .Where(o => o.Id == orderId)
                .ExecuteUpdateAsync(s => s.SetProperty(o => o.StatusId, newStatus));
        }

        public async Task IncrementOrderPackageQuantityAsync(Guid orderPackageId, int incrementQuantity)
        {
            await context.OrderPackages
                .Where(op => op.Id == orderPackageId)
                .ExecuteUpdateAsync(s => s.SetProperty(op => op.Quantity, op => op.Quantity + incrementQuantity));
        }

        public async Task SetOrderPackageQuantityAsync(Guid orderPackageId, int newQuantity)
        {
            await context.OrderPackages
                .Where(x => x.Id == orderPackageId)
                .ExecuteUpdateAsync(s => s.SetProperty(op => op.Quantity, newQuantity));
        }

        public async Task AddOrderPackageAsync(OrderPackage orderPackage)
        {
            context.OrderPackages.Add(orderPackage);
            await context.SaveChangesAsync();
        }

        public async Task RemoveOrderPackageAsync(Guid orderPackageId)
        {
            await context.OrderPackages
                .Where(x => x.Id == orderPackageId)
                .ExecuteDeleteAsync();
        }

        public async Task DeleteOrderDirectlyAsync(Guid orderId)
        {
            await context.Orders
                .Where(o => o.Id == orderId)
                .ExecuteDeleteAsync();
        }

        public Task ClearTrackerAsync()
        {
            context.ChangeTracker.Clear();
            return Task.CompletedTask;
        }
    }
}