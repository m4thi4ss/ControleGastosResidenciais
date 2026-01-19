using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleGastosResidenciais.Models;

//Representa os tipos de transação.
public enum TipoTransacao
{
    //Transação é uma despesa.
    Despesa = 1,

    //Transação é uma receita.
    Receita = 2
}

public class Transacao
{
    //Id da transação.
    [Key]
    public int Id { get; set; }

    //Descrição da transação.
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    //Valor da transação.
    [Required(ErrorMessage = "O valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser um número positivo")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }

    //Tipo da transação.
    [Required(ErrorMessage = "O tipo é obrigatório")]
    public TipoTransacao Tipo { get; set; }

    //Categoria da transação.
    [Required(ErrorMessage = "A categoria é obrigatória")]
    public int CategoriaId { get; set; }

    //Categoria da transação.
    [ForeignKey("CategoriaId")]
    public virtual Categoria Categoria { get; set; } = null!;

    //Pessoa da transação.
    [Required(ErrorMessage = "A pessoa é obrigatória")]
    public int PessoaId { get; set; }

    //Pessoa da transação.
    [ForeignKey("PessoaId")]
    public virtual Pessoa Pessoa { get; set; } = null!;
}
