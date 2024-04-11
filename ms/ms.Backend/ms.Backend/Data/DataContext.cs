using Microsoft.EntityFrameworkCore;
using ms.Backend.Domain.Models;
using System.Diagnostics.Metrics;

namespace ms.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Serial_tullave).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Lastname);
        }
    }
}