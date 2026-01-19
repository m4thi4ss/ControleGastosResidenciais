using System.ComponentModel.DataAnnotations;
using ControleGastosResidenciais.Models;

namespace ControleGastosResidenciais.DTOs;

//O DTO serve para controlar entrada e atualização de dados. 
//A cadastroEditarTransacaoDto é para cadastro e edição de transações.
public class CadastroEditarTransacaoDto
{
    //Descrição da transação.
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    //Valor da transação.
    [Required(ErrorMessage = "O valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser um número positivo")]
    public decimal Valor { get; set; }

    //Tipo da transação.
    [Required(ErrorMessage = "O tipo é obrigatório")]
    public TipoTransacao Tipo { get; set; }

    //Categoria da transação.
    [Required(ErrorMessage = "A categoria é obrigatória")]
    public int CategoriaId { get; set; }

    //Pessoa da transação.
    [Required(ErrorMessage = "A pessoa é obrigatória")]
    public int PessoaId { get; set; }
}
