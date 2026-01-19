using ControleGastosResidenciais.Models;

namespace ControleGastosResidenciais.Interfaces;

//A interface ela serve como um contrato, ela informa como a classe deve ser, sem definir como fazer a classe.
//Esse vai ser o contrato que deve ser implementado pelo repositório de transações. 
public interface ITransacaoRepository
{
    //Método para listar todas as transações.
    Task<List<Transacao>> ListarTodasAsync();

    //Método para buscar uma transação por id.
    Task<Transacao?> BuscarPorIdAsync(int id);

    //Método para criar uma nova transação.
    Task<Transacao> CriarAsync(Transacao transacao);

    //Método para listar todas as transações por pessoa id.
    Task<List<Transacao>> ListarPorPessoaIdAsync(int pessoaId);

    //Método para listar todas as transações por categoria id.
    Task<List<Transacao>> ListarPorCategoriaIdAsync(int categoriaId);
}
