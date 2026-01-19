using System.ComponentModel.DataAnnotations;

namespace ControleGastosResidenciais.Models;

//Representa uma pessoa.
//Vai ter as informações básicas como id, nome e idade.
public class Pessoa
{
    //Id da pessoa.
    [Key]
    public int Id { get; set; }

    //Nome da pessoa.
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    //Idade da pessoa.
    [Required(ErrorMessage = "A idade é obrigatória")]
    [Range(1, 150, ErrorMessage = "A idade deve ser um número positivo entre 1 e 150")]
    public int Idade { get; set; }

    //Lista de transações que pertencem a esta pessoa.
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

    //Verifica se a pessoa é menor de idade.
    public bool EhMenorDeIdade => Idade < 18;
}
