namespace ControleGastosResidenciais.ViewModels;

//ViewModel serve para controlar a saida de dados 
//Nessa classe ele vai listar o total por categoria.
public class TotalPorCategoriaViewModel
{
    //Id da categoria.
    public int CategoriaId { get; set; }

    //Descrição da categoria.
    public string CategoriaDescricao { get; set; } = string.Empty;

    //Total de receitas.
    public decimal TotalReceitas { get; set; }

    //Total de despesas.
    public decimal TotalDespesas { get; set; }

    //Saldo entre receitas e despesas.
    public decimal Saldo => TotalReceitas - TotalDespesas;
}


//Nessa classe ele vai listar o total geral por categorias.
public class TotalGeralCategoriasViewModel
{
    //Total de receitas geral.
    public decimal TotalGeralReceitas { get; set; }

    //Total de despesas geral.
    public decimal TotalGeralDespesas { get; set; }

    //Saldo entre receitas e despesas geral.
    public decimal SaldoLiquido => TotalGeralReceitas - TotalGeralDespesas;
}
