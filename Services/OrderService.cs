using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using BlazorApp1.Services.Interfaces;


namespace BlazorApp1.Services
{
    public class OrderService(IOrderRepository orderRepository, IPackageRepository packageRepository) : IOrderService
    {
        public async Task<List<Order>> GetAll()
        {
            return await orderRepository.GetAllAsync();
        }

        public async Task AddAsync(Order order)
        {
            foreach (var orderPackage in order.OrderPackages)
            {
                var package = await packageRepository.GetById(orderPackage.PackageId);

                if (package == null)
                    throw new Exception("Package not found.");

                if (orderPackage.Quantity > package.Quantity)
                    throw new Exception("Insufficient quantity available.");
                
                package.Quantity -= orderPackage.Quantity;
            }

            await orderRepository.AddAsync(order);
            
            await orderRepository.SaveChangesAsync();
            await packageRepository.SaveChangesAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await orderRepository.GetById(id);
        }

        public async Task UpdateStatusAsync(Guid id, StatusEnum newStatus)
        {
            await orderRepository.UpdateOrderStatusAsync(id, newStatus);
            await orderRepository.ClearTrackerAsync();
        }

        public async Task CancelOrderWithRestorationAsync(Guid orderId)
        {
            var order = await orderRepository.GetById(orderId);
            if (order != null && order.StatusId != StatusEnum.Canceled && order.StatusId != StatusEnum.Delivered)
            {
                foreach (var op in order.OrderPackages)
                {
                    await packageRepository.UpdatePackageQuantityAsync(op.PackageId, op.Quantity);
                }
                await orderRepository.UpdateOrderStatusAsync(orderId, StatusEnum.Canceled);
                await orderRepository.ClearTrackerAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await orderRepository.DeleteAsync(id);
            await orderRepository.SaveChangesAsync();
        }

        public async Task<Order?> GetPendingOrderForUserAsync(Guid userId)
        {
            return await orderRepository.GetPendingOrderForUserAsync(userId);
        }

        public async Task<List<Order>> GetFinishedOrdersForUserAsync(Guid userId, int month, int year)
        {
            return await orderRepository.GetFinishedOrdersForUserAsync(userId, month, year);
        }

        public async Task<string?> AddPackageToPendingOrderAsync(Guid userId, Guid packageId, int quantity)
        {
            if (quantity <= 0) return "Quantity must be greater than zero.";
            
            var package = await packageRepository.GetById(packageId);

            if (package == null) return "Package not found.";
            if (package.Quantity < quantity) return "Insufficient package quantity.";

            var currentTime = DateTime.Now.TimeOfDay;
            var startTime = package.PickupStart.TimeOfDay;
            var endTime = package.PickupEnd.TimeOfDay;
            bool isAvailable = startTime <= endTime 
                ? (currentTime >= startTime && currentTime <= endTime)
                : (currentTime >= startTime || currentTime <= endTime);
                
            if (!isAvailable) return $"This package is only available for pickup between {package.PickupStart:HH:mm} and {package.PickupEnd:HH:mm}.";

            var pendingOrder = await orderRepository.GetPendingOrderForUserAsync(userId);
            
            if (pendingOrder != null)
            {
                if (pendingOrder.StatusId != StatusEnum.Pending)
                {
                    return "You have an order currently processing. Please wait for it to finish.";
                }

                if (pendingOrder.BusinessId != package.BusinessId)
                {
                    return "You can only order packages from one business at a time. Please finish or cancel your current order.";
                }

                // Check if package already in order
                var existingOrderPackage = pendingOrder.OrderPackages.FirstOrDefault(op => op.PackageId == packageId);
                if (existingOrderPackage != null)
                {
                    // Increase quantity in DB securely
                    await orderRepository.IncrementOrderPackageQuantityAsync(existingOrderPackage.Id, quantity);
                }
                else
                {
                    // Add new order package
                    await orderRepository.AddOrderPackageAsync(new OrderPackage
                    {
                        Id = Guid.NewGuid(),
                        OrderId = pendingOrder.Id,
                        PackageId = packageId,
                        Quantity = quantity
                    });
                }
            }
            else
            {
                // Create new pending order
                pendingOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    BusinessId = package.BusinessId,
                    StatusId = StatusEnum.Pending,
                    OrderNumber = new Random().Next(10000, 99999),
                    CreatedAt = DateTime.UtcNow,
                    OrderPackages = new List<OrderPackage>
                    {
                        new OrderPackage
                        {
                            Id = Guid.NewGuid(),
                            PackageId = packageId,
                            Quantity = quantity
                        }
                    }
                };
                try
                {
                    await orderRepository.AddAsync(pendingOrder);
                    await orderRepository.SaveChangesAsync();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                {
                    return "Database error: Cannot place order. Your session may be invalid or the business was recently deleted. Please log out and log back in, or restart the app.";
                }
            }

            // Deduct package quantity
            await packageRepository.UpdatePackageQuantityAsync(packageId, -quantity);

            await orderRepository.ClearTrackerAsync();
            return null; // Success
        }

        public async Task<string?> RemovePackageFromPendingOrderAsync(Guid userId, Guid packageId)
        {
            var pendingOrder = await orderRepository.GetPendingOrderForUserAsync(userId);
            if (pendingOrder == null) return "No active order found.";
            if (pendingOrder.StatusId != StatusEnum.Pending) return "Cannot modify an order that is already processing.";

            var op = pendingOrder.OrderPackages.FirstOrDefault(x => x.PackageId == packageId);
            if (op == null) return "Package not found in the order.";

            // Return quantity to package
            await packageRepository.UpdatePackageQuantityAsync(packageId, op.Quantity);

            // Delete order package
            await orderRepository.RemoveOrderPackageAsync(op.Id);

            // If order is now empty, delete the order too
            if (pendingOrder.OrderPackages.Count <= 1) // 1 because it's still tracked
            {
                await orderRepository.DeleteOrderDirectlyAsync(pendingOrder.Id);
            }

            await orderRepository.ClearTrackerAsync();
            return null;
        }

        public async Task<string?> UpdatePackageQuantityAsync(Guid userId, Guid packageId, int newQuantity)
        {
            if (newQuantity <= 0) return await RemovePackageFromPendingOrderAsync(userId, packageId);

            var pendingOrder = await orderRepository.GetPendingOrderForUserAsync(userId);
            if (pendingOrder == null) return "No active order found.";
            if (pendingOrder.StatusId != StatusEnum.Pending) return "Cannot modify an order that is already processing.";

            var op = pendingOrder.OrderPackages.FirstOrDefault(x => x.PackageId == packageId);
            if (op == null) return "Package not found in the order.";

            int difference = newQuantity - op.Quantity;

            // Check if we have enough stock if increasing
            if (difference > 0)
            {
                var package = await packageRepository.GetPackageAvailabilityAsync(packageId);
                if (package == null || package.Value.Quantity < difference) return "Insufficient package quantity available.";
            }

            // Update order package quantity
            await orderRepository.SetOrderPackageQuantityAsync(op.Id, newQuantity);

            // Adjust stock
            await packageRepository.UpdatePackageQuantityAsync(packageId, -difference);

            await orderRepository.ClearTrackerAsync();
            return null;
        }
    }
}
