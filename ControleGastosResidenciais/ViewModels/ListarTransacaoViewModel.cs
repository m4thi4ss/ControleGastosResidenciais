using ControleGastosResidenciais.Models;

namespace ControleGastosResidenciais.ViewModels;

//ViewModel serve para controlar a saida de dados 
//Nessa classe ele vai listar as transações.
public class ListarTransacaoViewModel
{
    //Id da transação.
    public int Id { get; set; }

    //Descrição da transação.
    public string Descricao { get; set; } = string.Empty;

    //Valor da transação.
    public decimal Valor { get; set; }

    //Tipo da transação.
    public TipoTransacao Tipo { get; set; }

    //Texto do tipo da transação.
    public string TipoTexto => Tipo == TipoTransacao.Despesa ? "Despesa" : "Receita";

    //Id da categoria.
    public int CategoriaId { get; set; }

    //Descrição da categoria.
    public string CategoriaDescricao { get; set; } = string.Empty;

    //Id da pessoa.
    public int PessoaId { get; set; }

    //Nome da pessoa.
    public string PessoaNome { get; set; } = string.Empty;
}
