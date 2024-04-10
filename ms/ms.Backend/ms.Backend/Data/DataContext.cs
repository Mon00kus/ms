using Microsoft.EntityFrameworkCore;
using ms.Backend.Domain.Models;

namespace ms.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}