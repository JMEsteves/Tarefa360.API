using Microsoft.AspNetCore.Mvc;
using Projeto360.Aplicacao.Contratos;
using Projeto360.Aplicacao.DTOs;

namespace Projeto360.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SprintController : ControllerBase
{
    private readonly ISprintAplicacao _aplicacao;

    public SprintController(ISprintAplicacao aplicacao)
    {
        _aplicacao = aplicacao;
    }

    // GET: api/sprint
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SprintRespostaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        return Ok(await _aplicacao.ListarAsync());
    }

    // GET: api/sprint/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var sprint = await _aplicacao.ObterPorIdAsync(id);
        return sprint is null
            ? NotFound(new { mensagem = "Sprint não encontrada." })
            : Ok(sprint);
    }

    // POST: api/sprint
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarSprintDto dto)
    {
        try
        {
            var sprint = await _aplicacao.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = sprint.Id }, sprint);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // PUT: api/sprint/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Editar(int id, [FromBody] CriarSprintDto dto)
    {
        try
        {
            var sprint = await _aplicacao.EditarAsync(id, dto);
            return sprint is null
                ? NotFound(new { mensagem = "Sprint não encontrada." })
                : Ok(sprint);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // DELETE: api/sprint/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var excluida = await _aplicacao.ExcluirAsync(id);
        return excluida
            ? NoContent()
            : NotFound(new { mensagem = "Sprint não encontrada." });
    }
}