using ControleGastosResidenciais.Data;
using ControleGastosResidenciais.Interfaces;
using ControleGastosResidenciais.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosResidenciais.Repositories;

//Implementação do repositório de transações.
//Responsável por todas as operações de acesso a dados relacionadas a entidade transações.
public class TransacaoRepository : ITransacaoRepository
{
    private readonly ApplicationDbContext _context;

    //Injeção de dependência do contexto do banco de dados.
    public TransacaoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //Método para listar todas as transações.
    public async Task<List<Transacao>> ListarTodasAsync()
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .OrderByDescending(t => t.Id)
            .ToListAsync();
    }

    //Método para buscar uma transação por id.
    public async Task<Transacao?> BuscarPorIdAsync(int id)
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    //Método para criar uma nova transação.
    public async Task<Transacao> CriarAsync(Transacao transacao)
    {
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
        return transacao;
    }

    //Método para listar todas as transações por pessoa id.
    public async Task<List<Transacao>> ListarPorPessoaIdAsync(int pessoaId)
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .Where(t => t.PessoaId == pessoaId)
            .OrderByDescending(t => t.Id)
            .ToListAsync();
    }

    //Método para listar todas as transações por categoria id.
    public async Task<List<Transacao>> ListarPorCategoriaIdAsync(int categoriaId)
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .Where(t => t.CategoriaId == categoriaId)
            .OrderByDescending(t => t.Id)
            .ToListAsync();
    }
}
