using ControleGastosResidenciais.Models;

namespace ControleGastosResidenciais.Interfaces;

//A interface ela serve como um contrato, ela informa como a classe deve ser, sem definir como fazer a classe.
//Esse vai ser o contrato que deve ser implementado pelo repositório de pessoas.
public interface IPessoaRepository
{
    //Método para listar todas as pessoas.
    Task<List<Pessoa>> ListarTodasAsync();

    //Método para buscar uma pessoa por id.
    Task<Pessoa?> BuscarPorIdAsync(int id);

    //Método para criar uma nova pessoa.    
    Task<Pessoa> CriarAsync(Pessoa pessoa);

    //Método para deletar uma pessoa.
    Task DeletarAsync(Pessoa pessoa);

    //Método para verificar se uma pessoa existe.
    Task<bool> ExisteAsync(int id);
}
