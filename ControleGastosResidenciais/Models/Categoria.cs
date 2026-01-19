using System.ComponentModel.DataAnnotations;

namespace ControleGastosResidenciais.Models;

//Representa os tipos de finalidade de uma categoria.
public enum FinalidadeCategoria
{
    //Categoria pode ser usada apenas para despesas.
    Despesa = 1,

    //Categoria pode ser usada apenas para receitas.
    Receita = 2,

    //Categoria pode ser usada para despesas e receitas.
    Ambas = 3
}

//Representa uma categoria.
//Define a finalidade e a descrição da categoria.
public class Categoria
{
    //Id da categoria.
    [Key]
    public int Id { get; set; }

    //Descrição da categoria.
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    //Finalidade da categoria.
    [Required(ErrorMessage = "A finalidade é obrigatória")]
    public FinalidadeCategoria Finalidade { get; set; }

    //Lista de transações que utilizam esta categoria.
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

    //Verifica se a categoria pode ser usada para um tipo de transação específico.
    public bool PodeSerUsadaPara(TipoTransacao tipoTransacao)
    {
        return Finalidade == FinalidadeCategoria.Ambas ||
               (Finalidade == FinalidadeCategoria.Despesa && tipoTransacao == TipoTransacao.Despesa) ||
               (Finalidade == FinalidadeCategoria.Receita && tipoTransacao == TipoTransacao.Receita);
    }
}
