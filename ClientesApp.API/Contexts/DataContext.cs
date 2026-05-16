using ClientesApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientesApp.API.Contexts;

public class DataContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("BDClientes");
    }

    public DbSet<Cliente> Clientes { get; set; }
}