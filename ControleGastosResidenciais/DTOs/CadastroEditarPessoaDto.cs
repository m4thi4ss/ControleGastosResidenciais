using System.ComponentModel.DataAnnotations;

namespace ControleGastosResidenciais.DTOs;

//O DTO serve para controlar entrada e atualização de dados. 
//A cadastroEditarPessoaDto é para cadastro e edição de pessoas.
public class CadastroEditarPessoaDto
{
    //Nome da pessoa.
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    //Idade da pessoa.
    [Required(ErrorMessage = "A idade é obrigatória")]
    [Range(1, 150, ErrorMessage = "A idade deve ser um número positivo entre 1 e 150")]
    public int Idade { get; set; }
}
