using Microsoft.EntityFrameworkCore;

namespace Loja.ConsoleApp
{
    public class LojaContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; } /*'Produtos' neste contexto é tabela do banco de dados*/
        public DbSet<Compra> Compras { get; set; } /*'Compras' neste contexto é tabela do banco de dados*/
        public DbSet<Promocao> Promocoes { get; set; } /*'Promocoes' neste contexto é tabela do banco de dados*/
        public DbSet<Cliente> Clientes { get; set; } /*'Clientes' neste contexto é tabela do banco de dados*/

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //cria a chave primaria da classe 'PromocaoProduto' (a chave primaria é composta pela chave PromocaoId e ProdutoId)
            modelBuilder
                .Entity<PromocaoProduto>()
                .HasKey(promocaoProduto => new { 
                    promocaoProduto.PromocaoId, promocaoProduto.ProdutoId,
                });

            //cria a tabela Enderecos?
            modelBuilder
                .Entity<Endereco>()
                .ToTable("Enderecos");

            //cria ShadowProperty
            modelBuilder
                .Entity<Endereco>()
                .Property<int>("ClienteId"); /*ShadowProperty: cria uma coluna na tabela que não está mapeada na classe*/

            //cria a chave primaria da classe 'Endereco'
            modelBuilder
               .Entity<Endereco>()
               .HasKey("ClienteId");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LojaDB;Trusted_Connection=true;");
        }
    }
}