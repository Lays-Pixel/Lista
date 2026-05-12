using Microsoft.EntityFrameworkCore;
using Lista.Models;
namespace Lista.Data
{
    public class ListaContext : DbContext
    {
        
        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Tarefa> Tarefa { get; set; }

        public ListaContext(DbContextOptions<ListaContext> options) : base(options) { }

    }
}
