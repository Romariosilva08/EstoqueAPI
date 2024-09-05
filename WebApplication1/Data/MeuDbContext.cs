using Microsoft.EntityFrameworkCore;
using MinhaAPIEstoque.Models;

namespace MinhaAPIEstoque.Data
{
    public class MeuDbContext : DbContext
    {
        public MeuDbContext(DbContextOptions<MeuDbContext> options)
            : base(options)
        {
        }

        public DbSet<Produtos> Produtos { get; set; } 
        public DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure as propriedades da classe Produtos para a tabela no banco de dados
            modelBuilder.Entity<Produtos>(entity =>
            {
                entity.ToTable("Produtos"); 
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Preco)
                    .HasColumnType("decimal(18,2)"); 
            });

            // Configure as propriedades da classe Usuario para a tabela no banco de dados
            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(e => e.Id); 
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
