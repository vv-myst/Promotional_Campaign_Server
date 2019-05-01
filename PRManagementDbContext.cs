#region Auto generated information. Please do not modify

// DunnhumbyHomeWork DunnhumbyHomeWork PRManagementDbContext.cs
// bila007 Bilangi, Vivek-Vardhan, IT Collection International
// 2019-05-01 12:54
// 2019-04-28 22:00

#endregion

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
