namespace ControleGastosResidenciais.ViewModels;

//ViewModel serve para controlar a saida de dados 
//Nessa classe ele vai listar as pessoas.
public class ListarPessoaViewModel
{
    //Id da pessoa.
    public int Id { get; set; }

    //Nome da pessoa.
    public string Nome { get; set; } = string.Empty;

    //Idade da pessoa.
    public int Idade { get; set; }
}
