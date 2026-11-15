using Microsoft.AspNetCore.Mvc;
using Projeto360.Aplicacao.Contratos;
using Projeto360.Aplicacao.DTOs;

namespace Projeto360.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoriaController : ControllerBase
{
    private readonly IHistoriaServico _servico;

    public HistoriaController(IHistoriaServico servico)
    {
        _servico = servico;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        return Ok(await _servico.ListarAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var historia = await _servico.ObterPorIdAsync(id);

        if (historia == null)
            return NotFound();

        return Ok(historia);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarHistoriaDto dto)
    {
        try
        {
            var historia = await _servico.CriarAsync(dto);

            return CreatedAtAction(nameof(ObterPorId), new { id = historia.Id }, historia);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(int id, [FromBody] CriarHistoriaDto dto)
    {
        try
        {
            var historia = await _servico.EditarAsync(id, dto);

            if (historia == null)
                return NotFound();

            return Ok(historia);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var excluido = await _servico.ExcluirAsync(id);

        if (!excluido)
            return NotFound();

        return NoContent();
    }
}
