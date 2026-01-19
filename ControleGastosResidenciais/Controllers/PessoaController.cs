using ControleGastosResidenciais.DTOs;
using ControleGastosResidenciais.Interfaces;
using ControleGastosResidenciais.Models;
using ControleGastosResidenciais.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastosResidenciais.Controllers;
//Controller ele vai fazer as requisições via HTTP -> Get, Post, Put, Delete.
// No PessoaController ele vai gerenciar as operações de pessoas.
[ApiController]
[Route("api/[controller]")]
public class PessoaController : ControllerBase
{
    private readonly IPessoaRepository _pessoaRepository;

    //Injeção de dependência do repositório de pessoas.
    public PessoaController(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    //GET - Requisição de leitura onde tras as informações
    //Nessa requisição ele vai listar todas as pessoas cadastradas.
    //GET - api/pessoa
    [HttpGet]
    public async Task<ActionResult<List<ListarPessoaViewModel>>> ListarTodas()
    {
        try
        {
            var pessoas = await _pessoaRepository.ListarTodasAsync();

            var viewModels = pessoas.Select(p => new ListarPessoaViewModel
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade
            }).ToList();

            return Ok(viewModels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao listar pessoas", erro = ex.Message });
        }
    }

    //Ele vai buscar uma pessoa específica pelo seu id.
    //GET - api/pessoa/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ListarPessoaViewModel>> BuscarPorId(int id)
    {
        try
        {
            var pessoa = await _pessoaRepository.BuscarPorIdAsync(id);

            if (pessoa == null)
            {
                return NotFound(new { mensagem = "Pessoa não encontrada" });
            }

            var viewModel = new ListarPessoaViewModel
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade
            };

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao buscar pessoa", erro = ex.Message });
        }
    }

    //POST - Requisição de cadastro de dados.
    //Aqui ele vai criar uma nova pessoa.
    //POST - api/pessoa
    [HttpPost]
    public async Task<ActionResult<ListarPessoaViewModel>> Criar([FromBody] CadastroEditarPessoaDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Idade = dto.Idade
            };

            var pessoaCriada = await _pessoaRepository.CriarAsync(pessoa);

            var viewModel = new ListarPessoaViewModel
            {
                Id = pessoaCriada.Id,
                Nome = pessoaCriada.Nome,
                Idade = pessoaCriada.Idade
            };

            return CreatedAtAction(nameof(BuscarPorId), new { id = pessoaCriada.Id }, viewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao criar pessoa", erro = ex.Message });
        }
    }

    //DELETE - Requisição de deletar dados.
    //Aqui ele vai deletar uma pessoa específica pelo seu id.
    //DELETE - api/pessoa/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Deletar(int id)
    {
        try
        {
            var pessoa = await _pessoaRepository.BuscarPorIdAsync(id);

            if (pessoa == null)
            {
                return NotFound(new { mensagem = "Pessoa não encontrada" });
            }

            await _pessoaRepository.DeletarAsync(pessoa);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao deletar pessoa", erro = ex.Message });
        }
    }
}
