using ControleGastosResidenciais.Models;

namespace ControleGastosResidenciais.Interfaces;

//A interface ela serve como um contrato, ela informa como a classe deve ser, sem definir como fazer a classe.
//Esse vai ser o contrato que deve ser implementado pelo repositório de categorias.
public interface ICategoriaRepository
{
    //Método para listar todas as categorias.
    Task<List<Categoria>> ListarTodasAsync();

    //Método para buscar uma categoria por id.
    Task<Categoria?> BuscarPorIdAsync(int id);

    //Método para criar uma nova categoria.
    Task<Categoria> CriarAsync(Categoria categoria);

    //Método para verificar se uma categoria existe.
    Task<bool> ExisteAsync(int id);
}
