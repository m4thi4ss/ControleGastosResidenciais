using ControleGastosResidenciais.Data;
using ControleGastosResidenciais.Interfaces;
using ControleGastosResidenciais.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosResidenciais.Repositories;

//Implementação do repositório de pessoas.
//Responsável por todas as operações de acesso a dados relacionadas a entidade pessoas.
public class PessoaRepository : IPessoaRepository
{
    private readonly ApplicationDbContext _context;

    //Injeção de dependência do contexto do banco de dados.
    public PessoaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //Método para listar todas as pessoas.
    public async Task<List<Pessoa>> ListarTodasAsync()
    {
        return await _context.Pessoas
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    //Método para buscar uma pessoa por id.
    public async Task<Pessoa?> BuscarPorIdAsync(int id)
    {
        return await _context.Pessoas
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    //Método para criar uma nova pessoa.
    public async Task<Pessoa> CriarAsync(Pessoa pessoa)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
        return pessoa;
    }

    //Método para deletar uma pessoa.
    public async Task DeletarAsync(Pessoa pessoa)
    {
        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
    }

    //Método para verificar se uma pessoa existe.
    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Pessoas.AnyAsync(p => p.Id == id);
    }
}
