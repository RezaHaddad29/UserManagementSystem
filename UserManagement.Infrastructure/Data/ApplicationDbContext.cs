using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // اضافه کردن یک Constructor بدون ورودی برای EF Core (مورد نیاز در زمان Migration)
        public ApplicationDbContext() { }

        public DbSet<User> Users { get; set; } // مثال برای یک مدل دامنه
    }
}
