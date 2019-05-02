using DunnhumbyHomeWork.DbModel;
using Microsoft.EntityFrameworkCore;

namespace DunnhumbyHomeWork
{
    public class PRManagementDbContext : DbContext
    {
        public PRManagementDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    }
}
