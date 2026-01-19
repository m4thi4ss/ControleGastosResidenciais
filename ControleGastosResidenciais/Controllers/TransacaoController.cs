using ControleGastosResidenciais.DTOs;
using ControleGastosResidenciais.Interfaces;
using ControleGastosResidenciais.Models;
using ControleGastosResidenciais.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastosResidenciais.Controllers;

//Controller ele vai fazer as requisições via HTTP -> Get, Post, Put, Delete.
// No TransacaoController ele vai gerenciar as operações de transações.
[ApiController]
[Route("api/[controller]")]
public class TransacaoController : ControllerBase
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    //Injeção de dependência do repositório de transações, pessoas e categorias.
    public TransacaoController(
        ITransacaoRepository transacaoRepository,
        IPessoaRepository pessoaRepository,
        ICategoriaRepository categoriaRepository)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository = pessoaRepository;
        _categoriaRepository = categoriaRepository;
    }

    //GET - Requisição de leitura onde tras as informações
    //Nessa requisição ele vai listar todas as transações cadastradas.
    //GET - api/transacao
    [HttpGet]
    public async Task<ActionResult<List<ListarTransacaoViewModel>>> ListarTodas()
    {
        try
        {
            var transacoes = await _transacaoRepository.ListarTodasAsync();

            var viewModels = transacoes.Select(t => new ListarTransacaoViewModel
            {
                Id = t.Id,
                Descricao = t.Descricao,
                Valor = t.Valor,
                Tipo = t.Tipo,
                CategoriaId = t.CategoriaId,
                CategoriaDescricao = t.Categoria.Descricao,
                PessoaId = t.PessoaId,
                PessoaNome = t.Pessoa.Nome
            }).ToList();

            return Ok(viewModels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao listar transações", erro = ex.Message });
        }
    }

    //Ele vai buscar uma transação específica pelo seu id.
    //GET - api/transacao/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ListarTransacaoViewModel>> BuscarPorId(int id)
    {
        try
        {
            var transacao = await _transacaoRepository.BuscarPorIdAsync(id);

            if (transacao == null)
            {
                return NotFound(new { mensagem = "Transação não encontrada" });
            }

            var viewModel = new ListarTransacaoViewModel
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo,
                CategoriaId = transacao.CategoriaId,
                CategoriaDescricao = transacao.Categoria.Descricao,
                PessoaId = transacao.PessoaId,
                PessoaNome = transacao.Pessoa.Nome
            };

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao buscar transação", erro = ex.Message });
        }
    }

    //POST - Requisição de cadastro de dados.
    //Aqui ele vai criar uma nova transação.
    //POST - api/transacao
    [HttpPost]
    public async Task<ActionResult<ListarTransacaoViewModel>> Criar([FromBody] CadastroEditarTransacaoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoa = await _pessoaRepository.BuscarPorIdAsync(dto.PessoaId);
            if (pessoa == null)
            {
                return BadRequest(new { mensagem = "Pessoa não encontrada" });
            }

            if (pessoa.EhMenorDeIdade && dto.Tipo != TipoTransacao.Despesa)
            {
                return BadRequest(new { mensagem = "Menores de idade podem cadastrar apenas despesas" });
            }

            var categoria = await _categoriaRepository.BuscarPorIdAsync(dto.CategoriaId);
            if (categoria == null)
            {
                return BadRequest(new { mensagem = "Categoria não encontrada" });
            }

            if (!categoria.PodeSerUsadaPara(dto.Tipo))
            {
                return BadRequest(new { 
                    mensagem = $"A categoria '{categoria.Descricao}' não pode ser usada para transações do tipo '{dto.Tipo}'" 
                });
            }

            var transacao = new Transacao
            {
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Tipo = dto.Tipo,
                CategoriaId = dto.CategoriaId,
                PessoaId = dto.PessoaId
            };

            var transacaoCriada = await _transacaoRepository.CriarAsync(transacao);

            var transacaoCompleta = await _transacaoRepository.BuscarPorIdAsync(transacaoCriada.Id);

            var viewModel = new ListarTransacaoViewModel
            {
                Id = transacaoCompleta!.Id,
                Descricao = transacaoCompleta.Descricao,
                Valor = transacaoCompleta.Valor,
                Tipo = transacaoCompleta.Tipo,
                CategoriaId = transacaoCompleta.CategoriaId,
                CategoriaDescricao = transacaoCompleta.Categoria.Descricao,
                PessoaId = transacaoCompleta.PessoaId,
                PessoaNome = transacaoCompleta.Pessoa.Nome
            };

            return CreatedAtAction(nameof(BuscarPorId), new { id = transacaoCompleta.Id }, viewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao criar transação", erro = ex.Message });
        }
    }
}
