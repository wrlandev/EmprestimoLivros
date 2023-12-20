using EmprestimoLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLivros.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EmprestimoModel> Empretimos { get; set; }
    }
}
