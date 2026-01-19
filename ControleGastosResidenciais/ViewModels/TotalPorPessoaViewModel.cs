namespace ControleGastosResidenciais.ViewModels;

//ViewModel serve para controlar a saida de dados 
//Nessa classe ele vai listar o total por pessoa.
public class TotalPorPessoaViewModel
{
    //Id da pessoa.
    public int PessoaId { get; set; }

    //Nome da pessoa.
    public string PessoaNome { get; set; } = string.Empty;

    //Idade da pessoa.
    public int PessoaIdade { get; set; }

    //Total de receitas.
    public decimal TotalReceitas { get; set; }

    //Total de despesas.
    public decimal TotalDespesas { get; set; }

    //Saldo entre receitas e despesas.
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

//Nessa classe ele vai listar o total geral por pessoas.
public class TotalGeralPessoasViewModel
{
    //Total de receitas geral.
    public decimal TotalGeralReceitas { get; set; }

    //Total de despesas geral.
    public decimal TotalGeralDespesas { get; set; }

    //Saldo entre receitas e despesas geral.
    public decimal SaldoLiquido => TotalGeralReceitas - TotalGeralDespesas;
}
