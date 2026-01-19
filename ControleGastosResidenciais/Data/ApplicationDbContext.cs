using ControleGastosResidenciais.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosResidenciais.Data;

//Contexto do banco de dados.
//Ele vai gerenciar as entidades e configurar o banco de dados.
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    //Tabela de pessoas cadastradas no sistema.
    public DbSet<Pessoa> Pessoas { get; set; }

    //Tabela de categorias cadastradas no sistema.
    public DbSet<Categoria> Categorias { get; set; }

    //Tabela de transações cadastradas no sistema.
    public DbSet<Transacao> Transacoes { get; set; }

    //Configuração das entidades e relacionamentos entre as entidades.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Configuração da entidade Pessoa.
        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Idade).IsRequired();
        });

        //Configuração da entidade Categoria.
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Finalidade).IsRequired();
        });

        //Configuração da entidade Transacao.
        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Tipo).IsRequired();

            //Relacionamento em Categoria
            entity.HasOne(e => e.Categoria)
                  .WithMany(c => c.Transacoes)
                  .HasForeignKey(e => e.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);

            //Relacionamento em Pessoa. Quando a pessoa é deletada, todas as transações relacionadas a ela também são deletadas.
            entity.HasOne(e => e.Pessoa)
                  .WithMany(p => p.Transacoes)
                  .HasForeignKey(e => e.PessoaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
