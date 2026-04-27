using LifecycleManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new IdentityRole("User"));

        if (await userManager.FindByEmailAsync("admin@lifecycle.local") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@lifecycle.local",
                Email = "admin@lifecycle.local",
                DisplayName = "Administrator",
                EmailConfirmed = true,
                ThemePreference = "System"
            };
            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (!await context.ItemTypes.AnyAsync())
        {
            context.ItemTypes.AddRange(
                new ItemType { Name = "Domain", DefaultLeadDays = 20, Description = "Domain name registrations", IsActive = true },
                new ItemType { Name = "Contract", DefaultLeadDays = 90, Description = "Business contracts and agreements", IsActive = true },
                new ItemType { Name = "Certificate", DefaultLeadDays = 30, Description = "SSL/TLS and security certificates", IsActive = true },
                new ItemType { Name = "Certification", DefaultLeadDays = 60, Description = "Professional certifications", IsActive = true }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Items.AnyAsync())
        {
            var adminUser = await userManager.FindByEmailAsync("admin@lifecycle.local");
            var domainType = await context.ItemTypes.FirstOrDefaultAsync(t => t.Name == "Domain");
            var contractType = await context.ItemTypes.FirstOrDefaultAsync(t => t.Name == "Contract");
            var certType = await context.ItemTypes.FirstOrDefaultAsync(t => t.Name == "Certificate");

            if (adminUser != null && domainType != null && contractType != null && certType != null)
            {
                context.Items.AddRange(
                    new Item
                    {
                        Name = "example.com",
                        ItemTypeId = domainType.Id,
                        ExpirationDate = DateTime.UtcNow.AddDays(15),
                        OwnerUserId = adminUser.Id,
                        Notes = "Primary company domain",
                        Status = ItemStatus.Active,
                        CreatedDate = DateTime.UtcNow
                    },
                    new Item
                    {
                        Name = "Vendor Service Agreement",
                        ItemTypeId = contractType.Id,
                        ExpirationDate = DateTime.UtcNow.AddDays(5),
                        OwnerUserId = adminUser.Id,
                        Notes = "Annual vendor contract",
                        Status = ItemStatus.Active,
                        CreatedDate = DateTime.UtcNow
                    },
                    new Item
                    {
                        Name = "SSL Certificate - api.example.com",
                        ItemTypeId = certType.Id,
                        ExpirationDate = DateTime.UtcNow.AddDays(-5),
                        OwnerUserId = adminUser.Id,
                        Notes = "API endpoint certificate",
                        Status = ItemStatus.Active,
                        CreatedDate = DateTime.UtcNow
                    },
                    new Item
                    {
                        Name = "Secondary Domain backup.example.com",
                        ItemTypeId = domainType.Id,
                        ExpirationDate = DateTime.UtcNow.AddDays(60),
                        Status = ItemStatus.Active,
                        CreatedDate = DateTime.UtcNow
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
