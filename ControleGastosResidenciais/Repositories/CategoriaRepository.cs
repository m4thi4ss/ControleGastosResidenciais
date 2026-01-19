using ControleGastosResidenciais.Data;
using ControleGastosResidenciais.Interfaces;
using ControleGastosResidenciais.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosResidenciais.Repositories;

//Implementação do repositório de categorias.
//Responsável por todas as operações de acesso a dados relacionadas a entidade categorias.
public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext _context;

    //Injeção de dependência do contexto do banco de dados.
    public CategoriaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //Método para listar todas as categorias.
    public async Task<List<Categoria>> ListarTodasAsync()
    {
        return await _context.Categorias
            .OrderBy(c => c.Descricao)
            .ToListAsync();
    }

    //Método para buscar uma categoria por id.
    public async Task<Categoria?> BuscarPorIdAsync(int id)
    {
        return await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    //Método para criar uma nova categoria.
    public async Task<Categoria> CriarAsync(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    //Método para verificar se uma categoria existe.
    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Categorias.AnyAsync(c => c.Id == id);
    }
}
