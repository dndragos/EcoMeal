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
                    new Business { Name = "The Rusty Spoon", Description = "Classic American Diner", Address = "101 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[0].Id },
                    new Business { Name = "Burger Haven", Description = "Best burgers", Address = "102 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[1].Id },
                    new Business { Name = "Pasta Palace", Description = "Italian cuisine", Address = "103 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[2].Id },
                    new Business { Name = "Sushi Central", Description = "Fresh sushi", Address = "104 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[3].Id },
                    new Business { Name = "Taco Fiesta", Description = "Mexican food", Address = "105 Main St", BusinessTypeId = BusinessTypeEnum.Restaurant, OwnerId = dummyUsers[4].Id },

                    new Business { Name = "Fresh Foods Market", Description = "Organic produce", Address = "201 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[5].Id },
                    new Business { Name = "Value Mart", Description = "Discount groceries", Address = "202 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[6].Id },
                    new Business { Name = "Green Grocers", Description = "Fresh veg", Address = "203 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[7].Id },
                    new Business { Name = "Mega Market", Description = "Everything", Address = "204 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[8].Id },
                    new Business { Name = "Corner Store", Description = "Convenience", Address = "205 Oak St", BusinessTypeId = BusinessTypeEnum.Supermarket, OwnerId = dummyUsers[9].Id },

                    new Business { Name = "Morning Brew", Description = "Artisan coffee", Address = "301 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[10].Id },
                    new Business { Name = "The Daily Grind", Description = "Espresso", Address = "302 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[11].Id },
                    new Business { Name = "Espresso Yourself", Description = "Specialty drinks", Address = "303 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[12].Id },
                    new Business { Name = "Bean There", Description = "Reading cafe", Address = "304 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[13].Id },
                    new Business { Name = "Cafe Mocha", Description = "Chocolate", Address = "305 Pine St", BusinessTypeId = BusinessTypeEnum.Cafe, OwnerId = dummyUsers[14].Id },
                };

                context.Businesses.AddRange(businesses);
                await context.SaveChangesAsync();

                // Seed Packages
                var packages = new List<Package>();
                foreach (var business in businesses)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        packages.Add(new Package
                        {
                            BusinessId = business.Id,
                            Name = $"{business.Name} Package {i}",
                            Description = "A delicious assortment of leftover items.",
                            Price = 5.99m + i,
                            Quantity = i * 2,
                            PickupStart = DateTime.Now,
                            PickupEnd = DateTime.Now.AddHours(2),
                            PackageTypeId = business.BusinessTypeId == BusinessTypeEnum.Restaurant ? PackageTypeEnum.SurpriseMeal : 
                                            business.BusinessTypeId == BusinessTypeEnum.Supermarket ? PackageTypeEnum.Groceries : PackageTypeEnum.Pastry
                        });
                    }
                }
                context.Packages.AddRange(packages);
                await context.SaveChangesAsync();
            }
        }
    }
}
