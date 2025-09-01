
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using qrmenu.Models;
using Microsoft.AspNetCore.Identity;


namespace qrmenu.Data
    {
        public class AppDbContext : IdentityDbContext<IdentityUser>
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<Restaurant> Restaurants { get; set; }
       
            public DbSet<Category> Categories { get; set; }
            public DbSet<Product> Products { get; set; }

            public DbSet<ScanLog> ScanLogs { get; set; }

    }
}


