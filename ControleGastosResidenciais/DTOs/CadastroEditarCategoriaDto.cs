using System.ComponentModel.DataAnnotations;
using ControleGastosResidenciais.Models;

namespace ControleGastosResidenciais.DTOs;
//O DTO serve para controlar entrada e atualização de dados. 
//A cadastroEditarCategoriaDto é para cadastro e edição de categorias.
public class CadastroEditarCategoriaDto
{
    //Descrição da categoria.
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    //Finalidade da categoria.  
    //Define para quais tipos de transação a categoria pode ser usada.
    [Required(ErrorMessage = "A finalidade é obrigatória")]
    public FinalidadeCategoria Finalidade { get; set; }
}
