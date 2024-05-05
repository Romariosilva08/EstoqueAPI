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
                entity.ToTable("Produtos"); // Define o nome da tabela
                entity.HasKey(e => e.Id); // Define a chave primária
                // Defina outras configurações de propriedade, como tamanho máximo, restrições, etc.
            });

            // Configure as propriedades da classe Usuario para a tabela no banco de dados
            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("Usuarios"); // Define o nome da tabela
                entity.HasKey(e => e.Id); // Define a chave primária
                // Defina outras configurações de propriedade, como tamanho máximo, restrições, etc.
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
