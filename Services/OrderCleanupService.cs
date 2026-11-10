using BlazorApp1.Data;
using BlazorApp1.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class OrderCleanupService(IServiceProvider serviceProvider, ILogger<OrderCleanupService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<EcoMealDbContext>();

                    var threshold = DateTime.UtcNow.AddMinutes(-30);
                    var oldOrders = await dbContext.Orders
                        .Where(o => (o.StatusId == StatusEnum.Canceled || o.StatusId == StatusEnum.Delivered) && o.CreatedAt <= threshold)
                        .Include(o => o.OrderPackages)
                        .ToListAsync(stoppingToken);

                    if (oldOrders.Any())
                    {
                        dbContext.OrderPackages.RemoveRange(oldOrders.SelectMany(o => o.OrderPackages));
                        dbContext.Orders.RemoveRange(oldOrders);
                        await dbContext.SaveChangesAsync(stoppingToken);
                        logger.LogInformation($"Deleted {oldOrders.Count} old orders.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while cleaning up old orders.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
