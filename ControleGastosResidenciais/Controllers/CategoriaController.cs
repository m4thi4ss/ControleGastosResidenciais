using ControleGastosResidenciais.DTOs;
using ControleGastosResidenciais.Interfaces;
using ControleGastosResidenciais.Models;
using ControleGastosResidenciais.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastosResidenciais.Controllers;

// Controller ele vai fazer as requisições HTTP -> Post, Get, Put, Delete
// No CategoriaController ele vai gerenciar as operações de categorias.
[ApiController] 
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaRepository _categoriaRepository;

    //Injeção de dependência do repositório de categorias.
    public CategoriaController(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    //Get - Requisição de leitura onde tras as informações
    //Nessa requisição ele vai listar todas as categorias cadastradas.
    // GET - api/categoria
    [HttpGet]
    public async Task<ActionResult<List<ListarCategoriaViewModel>>> ListarTodas()
    {
        try
        {
            var categorias = await _categoriaRepository.ListarTodasAsync();

            var viewModels = categorias.Select(c => new ListarCategoriaViewModel
            {
                Id = c.Id,
                Descricao = c.Descricao,
                Finalidade = c.Finalidade
            }).ToList();

            return Ok(viewModels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao listar categorias", erro = ex.Message });
        }
    }

    //Ele vai buscar uma categoria específica pelo id.
    // GET - api/categoria/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ListarCategoriaViewModel>> BuscarPorId(int id)
    {
        try
        {
            var categoria = await _categoriaRepository.BuscarPorIdAsync(id);

            if (categoria == null)
            {
                return NotFound(new { mensagem = "Categoria não encontrada" });
            }

            var viewModel = new ListarCategoriaViewModel
            {
                Id = categoria.Id,
                Descricao = categoria.Descricao,
                Finalidade = categoria.Finalidade
            };

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao buscar categoria", erro = ex.Message });
        }
    }

    //POST - Requisição de cadastro de dados.
    //Aqui ele vai criar uma nova categoria.
    // POST - api/categoria
    [HttpPost]
    public async Task<ActionResult<ListarCategoriaViewModel>> Criar([FromBody] CadastroEditarCategoriaDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = new Categoria
            {
                Descricao = dto.Descricao,
                Finalidade = dto.Finalidade
            };

            var categoriaCriada = await _categoriaRepository.CriarAsync(categoria);

            var viewModel = new ListarCategoriaViewModel
            {
                Id = categoriaCriada.Id,
                Descricao = categoriaCriada.Descricao,
                Finalidade = categoriaCriada.Finalidade
            };

            return CreatedAtAction(nameof(BuscarPorId), new { id = categoriaCriada.Id }, viewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao criar categoria", erro = ex.Message });
        }
    }
}
