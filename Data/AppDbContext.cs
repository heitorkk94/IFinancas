using Microsoft.EntityFrameworkCore;
using IFinancas.Models;

namespace IFinancas.Data
{
    // A classe DEVE ser public e herdar de DbContext
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Mapeamento das Models para as Tabelas do Banco de Dados
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }
    }
}