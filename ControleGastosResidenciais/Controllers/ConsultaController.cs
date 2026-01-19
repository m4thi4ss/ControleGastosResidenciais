using ControleGastosResidenciais.Interfaces;
using ControleGastosResidenciais.Models;
using ControleGastosResidenciais.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastosResidenciais.Controllers;
//Controller ele vai fazer as requisições via HTTP -> Get, Post, Put, Delete.
// No ConsultaController ele vai gerenciar as consultas de totais financeiros.
[ApiController]
[Route("api/[controller]")]
public class ConsultaController : ControllerBase
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    //Injeção de dependência do repositório de pessoas, transações e categorias.
    public ConsultaController(
        IPessoaRepository pessoaRepository,
        ITransacaoRepository transacaoRepository,
        ICategoriaRepository categoriaRepository)
    {
        _pessoaRepository = pessoaRepository;
        _transacaoRepository = transacaoRepository;
        _categoriaRepository = categoriaRepository;
    }

    //GET - Requisição de leitura onde tras as informações
    //Vai consultar os totais por pessoa, vai listar as pessoas, vai exibir o total de receitas e o saldo. No final ele vai e mostrar o total geral de todas as pessoas.
    // GET - api/consulta/TotaisPorPessoa
    [HttpGet("TotaisPorPessoa")]
    public async Task<ActionResult<object>> TotaisPorPessoa()
    {
        try
        {
            var pessoas = await _pessoaRepository.ListarTodasAsync();
            var transacoes = await _transacaoRepository.ListarTodasAsync();

            var totaisPorPessoa = new List<TotalPorPessoaViewModel>();
            
            //Calcula os totais para cada pessoa.
            foreach (var pessoa in pessoas)
            {
                var transacoesDaPessoa = transacoes.Where(t => t.PessoaId == pessoa.Id).ToList();

                var totalReceitas = transacoesDaPessoa
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor);

                var totalDespesas = transacoesDaPessoa
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor);

                totaisPorPessoa.Add(new TotalPorPessoaViewModel
                {
                    PessoaId = pessoa.Id,
                    PessoaNome = pessoa.Nome,
                    PessoaIdade = pessoa.Idade,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas
                });
            }

            var totalGeralReceitas = transacoes
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var totalGeralDespesas = transacoes
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            var totalGeral = new TotalGeralPessoasViewModel
            {
                TotalGeralReceitas = totalGeralReceitas,
                TotalGeralDespesas = totalGeralDespesas
            };

            return Ok(new
            {
                totaisPorPessoa,
                totalGeral
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao consultar totais por pessoa", erro = ex.Message });
        }
    }

    //Vai consultar os totais por categoria, vai listar as categorias, vai exibir o total de receitas e o saldo. No final ele vai mostrar o total geral de todas as categorias.
    // GET - api/consulta/TotaisPorCategoria
    [HttpGet("TotaisPorCategoria")]
    public async Task<ActionResult<object>> TotaisPorCategoria()
    {
        try
        {
            var categorias = await _categoriaRepository.ListarTodasAsync();
            var transacoes = await _transacaoRepository.ListarTodasAsync();

            var totaisPorCategoria = new List<TotalPorCategoriaViewModel>();

            foreach (var categoria in categorias)
            {
                var transacoesDaCategoria = transacoes.Where(t => t.CategoriaId == categoria.Id).ToList();

                var totalReceitas = transacoesDaCategoria
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor);

                var totalDespesas = transacoesDaCategoria
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor);

                totaisPorCategoria.Add(new TotalPorCategoriaViewModel
                {
                    CategoriaId = categoria.Id,
                    CategoriaDescricao = categoria.Descricao,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas
                });
            }

            var totalGeralReceitas = transacoes
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var totalGeralDespesas = transacoes
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            var totalGeral = new TotalGeralCategoriasViewModel
            {
                TotalGeralReceitas = totalGeralReceitas,
                TotalGeralDespesas = totalGeralDespesas
            };

            return Ok(new
            {
                totaisPorCategoria,
                totalGeral
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao consultar totais por categoria", erro = ex.Message });
        }
    }
}
