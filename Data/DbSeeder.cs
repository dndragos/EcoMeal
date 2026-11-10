using BlazorApp1.Constants;
using BlazorApp1.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Data

{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var context = services.GetRequiredService<EcoMealDbContext>();
            
            foreach(var role in AppRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var adminEmail = configuration["SeedAdmin:Email"];
            var adminPassword = configuration["SeedAdmin:Password"];

            if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
            {
                var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
                if (existingAdmin == null)
                {
                    var admin = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        FullName = "Admin"
                    };

                    var result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, AppRoles.Admin);
                    }
                }
            }

            // Seed Dummy Users, Businesses, and Packages
            string[] names = { "Alice Smith", "Bob Johnson", "Charlie Brown", "Diana Prince", "Ethan Hunt", 
                               "Fiona Gallagher", "George Costanza", "Hannah Abbott", "Ian Malcolm", "Julia Roberts",
                               "Kevin McCallister", "Laura Palmer", "Michael Scott", "Nina Simone", "Oscar Martinez" };

            var dummyUsers = new List<ApplicationUser>();

            foreach (var name in names)
            {
                var email = name.Split(' ')[0].ToLower() + "@example.com";
                // Password must be at least 8 characters long to satisfy Identity requirements
                var password = name.Split(' ')[0] + "12345!";
                
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        FullName = name
                    };
                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, AppRoles.BusinessManager);
                    }
                }
                dummyUsers.Add(user);
            }

            // Seed Businesses
            if (!await context.Businesses.AnyAsync(b => b.Name == "The Rusty Spoon"))
            {
                var businesses = new List<Business>
                {
                    new Business { Name = "The Rusty Spoon", Description = "Classic American Diner", Address = "101 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[0].Id, ImageUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Burger Haven", Description = "Best burgers", Address = "102 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[1].Id, ImageUrl = "https://images.unsplash.com/photo-1550547660-d9450f859349?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Pasta Palace", Description = "Italian cuisine", Address = "103 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[2].Id, ImageUrl = "https://images.unsplash.com/photo-1473093295043-cdd812d0e601?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Sushi Central", Description = "Fresh sushi", Address = "104 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[3].Id, ImageUrl = "https://images.unsplash.com/photo-1579871494447-9811cf80d66c?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Taco Fiesta", Description = "Mexican food", Address = "105 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[4].Id, ImageUrl = "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?auto=format&fit=crop&w=500&q=60" },

                    new Business { Name = "Fresh Foods Market", Description = "Organic produce", Address = "201 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[5].Id, ImageUrl = "https://images.unsplash.com/photo-1578916171728-46686eac8d58?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Value Mart", Description = "Discount groceries", Address = "202 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[6].Id, ImageUrl = "https://images.unsplash.com/photo-1534723452862-4c874018d66d?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Green Grocers", Description = "Fresh veg", Address = "203 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[7].Id, ImageUrl = "https://images.unsplash.com/photo-1542838132-92c53300491e?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Mega Market", Description = "Everything", Address = "204 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[8].Id, ImageUrl = "https://images.unsplash.com/photo-1604719312566-8912e9227c6a?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Corner Store", Description = "Convenience", Address = "205 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[9].Id, ImageUrl = "https://images.unsplash.com/photo-1583258292688-d0213dc5a3a8?auto=format&fit=crop&w=500&q=60" },

                    new Business { Name = "Morning Brew", Description = "Artisan coffee", Address = "301 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[10].Id, ImageUrl = "https://images.unsplash.com/photo-1554118811-1e0d58224f24?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "The Daily Grind", Description = "Espresso", Address = "302 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[11].Id, ImageUrl = "https://images.unsplash.com/photo-1497935586351-b67a49e012bf?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Espresso Yourself", Description = "Specialty drinks", Address = "303 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[12].Id, ImageUrl = "https://images.unsplash.com/photo-1509042239860-f550ce710b93?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Bean There", Description = "Reading cafe", Address = "304 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[13].Id, ImageUrl = "https://images.unsplash.com/photo-1445116572660-236099ec97a0?auto=format&fit=crop&w=500&q=60" },
                    new Business { Name = "Cafe Mocha", Description = "Chocolate", Address = "305 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[14].Id, ImageUrl = "https://images.unsplash.com/photo-1511920170033-f8396924c348?auto=format&fit=crop&w=500&q=60" },
                };

                context.Businesses.AddRange(businesses);
                await context.SaveChangesAsync();

                // Seed Packages
                var packages = new List<Package>();                var rnd = new Random();
                
                string[] restaurantPackagesPhotos = {
                    "https://images.unsplash.com/photo-1544025162-d76694265947?auto=format&fit=crop&w=500&q=60",
                    "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?auto=format&fit=crop&w=500&q=60",
                    "https://images.unsplash.com/photo-1564834724105-918b73d1b9e0?auto=format&fit=crop&w=500&q=60"
                };
                
                string[] supermarketPackagesPhotos = {
                    "https://images.unsplash.com/photo-1610832958506-aa56368176cf?auto=format&fit=crop&w=500&q=60",
                    "https://images.unsplash.com/photo-1542838132-92c53300491e?auto=format&fit=crop&w=500&q=60",
                    "https://images.unsplash.com/photo-1588964895597-cfccd6e2b0d9?auto=format&fit=crop&w=500&q=60"
                };

                string[] cafePackagesPhotos = {
                    "https://images.unsplash.com/photo-1509042239860-f550ce710b93?auto=format&fit=crop&w=500&q=60",
                    "https://images.unsplash.com/photo-1483695028939-5bb13f8648b0?auto=format&fit=crop&w=500&q=60",
                    "https://images.unsplash.com/photo-1550617931-e17a7b70dce2?auto=format&fit=crop&w=500&q=60"
                };

                foreach (var business in businesses)
                {
                    for (int i = 0; i < 3; i++) // MINIMUM 3 packages
                    {
                        var photos = business.BusinessTypeId == BusinessTypeEnum.Restaurant ? restaurantPackagesPhotos :
                                     business.BusinessTypeId == BusinessTypeEnum.Supermarket ? supermarketPackagesPhotos : cafePackagesPhotos;

                        packages.Add(new Package
                        {
                            BusinessId = business.Id,
                            Name = $"{business.Name} Surprise {i + 1}",
                            Description = "A delicious assortment of leftover items carefully packed.",
                            Price = Math.Round((decimal)(rnd.NextDouble() * 10 + 2), 2), // random price 2-12
                            Quantity = rnd.Next(1, 10), // random quantity 1-9
                            PickupStart = DateTime.Today.AddHours(i * 8), // 0, 8, 16
                            PickupEnd = DateTime.Today.AddHours((i + 1) * 8).AddMinutes(-1), // 7:59, 15:59, 23:59
                            PackageTypeId = business.BusinessTypeId == BusinessTypeEnum.Restaurant ? PackageTypeEnum.SurpriseMeal : 
                                            business.BusinessTypeId == BusinessTypeEnum.Supermarket ? PackageTypeEnum.Groceries : PackageTypeEnum.Pastry,
                            ImageUrl = photos[i]
                        });
                    }
                }
                context.Packages.AddRange(packages);
                await context.SaveChangesAsync();
            }
            
            await UpdateExistingPackagesAsync(context);
        }

        private static async Task UpdateExistingPackagesAsync(EcoMealDbContext context)
        {
            var packages = context.Packages.ToList();
            if (!packages.Any()) return;
            
            bool changed = false;
            
            // Cleanup orphan packages
            var orphanPackages = packages.Where(p => !context.Businesses.Any(b => b.Id == p.BusinessId)).ToList();
            if (orphanPackages.Any())
            {
                context.Packages.RemoveRange(orphanPackages);
                changed = true;
                packages = packages.Except(orphanPackages).ToList();
            }

            // Cleanup orphan orders
            var orphanOrders = context.Orders.Where(o => !context.Businesses.Any(b => b.Id == o.BusinessId)).ToList();
            if (orphanOrders.Any())
            {
                context.Orders.RemoveRange(orphanOrders);
                changed = true;
            }

            foreach(var package in packages)
            {
                int i = Math.Abs(package.Id.GetHashCode()) % 3;
                var newStart = DateTime.Today.AddHours(i * 8);
                var newEnd = DateTime.Today.AddHours((i + 1) * 8).AddMinutes(-1);
                
                if (package.PickupStart != newStart || package.PickupEnd != newEnd)
                {
                    package.PickupStart = newStart;
                    package.PickupEnd = newEnd;
                    changed = true;
                }
                
                if (package.Quantity == 0)
                {
                    package.Quantity = 5;
                    changed = true;
                }
            }
            
            if (changed)
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
