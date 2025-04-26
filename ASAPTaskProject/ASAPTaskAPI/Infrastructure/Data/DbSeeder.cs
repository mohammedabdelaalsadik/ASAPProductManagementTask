using ASAPTaskAPI.Domain.Entities;

namespace ASAPTaskAPI.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                });
            }

            if (!context.Products.Any())
            {
                context.Products.Add(new Product { Name = "Item 1", Description = "Test", Price = 100 });
                context.Products.Add(new Product { Name = "Item 2", Description = "Test", Price = 18 });
                context.Products.Add(new Product { Name = "Item 3", Description = "Test", Price = 10 });
                context.Products.Add(new Product { Name = "Item 4", Description = "Test", Price = 10 });
                context.Products.Add(new Product { Name = "Item 5", Description = "Test", Price = 100 });
                context.Products.Add(new Product { Name = "Item 6", Description = "Test", Price = 108 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
                context.Products.Add(new Product { Name = "Item 7", Description = "Test", Price = 107 });
            }

            context.SaveChanges();
        }
    }
}
