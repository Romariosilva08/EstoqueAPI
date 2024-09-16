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


        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetalhesPedido> DetalhesPedidos { get; set; }

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

            // Configure as propriedades da classe Pedido para a tabela no banco de dados
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("Pedidos");
                entity.HasKey(e => e.Id);

                // Configure propriedades adicionais se necessário
            });

            // Configure as propriedades da classe DetalhesPedido para a tabela no banco de dados
            modelBuilder.Entity<DetalhesPedido>(entity =>
            {
                entity.ToTable("DetalhesPedidos");
                entity.HasKey(e => e.Id);

                // Configure propriedades adicionais se necessário
            });



            base.OnModelCreating(modelBuilder);
        }
    }
}
