using ControleGastosResidenciais.Models;

namespace ControleGastosResidenciais.ViewModels;

//ViewModel serve para controlar a saida de dados 
//Nessa classe ele vai listar as categorias.
public class ListarCategoriaViewModel
{
    //Id da categoria.
    public int Id { get; set; }

    //Descrição da categoria.
    public string Descricao { get; set; } = string.Empty;

    //Finalidade da categoria.
    public FinalidadeCategoria Finalidade { get; set; }

    //Texto da finalidade da categoria.
    public string FinalidadeTexto => Finalidade switch
    {
        FinalidadeCategoria.Despesa => "Despesa",
        FinalidadeCategoria.Receita => "Receita",
        FinalidadeCategoria.Ambas => "Ambas",
        _ => "Desconhecido"
    };
}
