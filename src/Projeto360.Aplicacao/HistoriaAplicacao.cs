using Projeto360.Aplicacao.Contratos;
using Projeto360.Aplicacao.DTOs;
using Projeto360.Dominio.Entidades;

namespace Projeto360.Aplicacao;

public class HistoriaAplicacao : IHistoriaAplicacao
{
    private readonly IHistoriaRepositorio _historiaRepositorio;

    public HistoriaAplicacao(IHistoriaRepositorio historiaRepositorio)
    {
        _historiaRepositorio = historiaRepositorio;
    }

    public async Task<IEnumerable<HistoriaRespostaDto>> ListarAsync()
    {
        var historias = await _historiaRepositorio.ListarAsync();
        return historias.Select(Mapear);
    }

    public async Task<HistoriaRespostaDto> CriarAsync(CriarHistoriaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            throw new ArgumentException("O nome da história é obrigatório.");
        }

        if (dto.ProjetoId <= 0)
        {
            throw new ArgumentException("O projeto da história é obrigatório.");
        }

        var historia = new Historia
        {
            Nome = dto.Nome.Trim(),
            Descricao = dto.Descricao?.Trim(),
            ProjetoId = dto.ProjetoId
        };

        await _historiaRepositorio.AdicionarAsync(historia);

        var criada = await _historiaRepositorio.ObterPorIdAsync(historia.Id);

        return Mapear(criada ?? historia);
    }

    public async Task<HistoriaRespostaDto?> ObterPorIdAsync(int id)
    {
        var historia = await _historiaRepositorio.ObterPorIdAsync(id);

        if (historia == null)
        {
            return null;
        }

        return Mapear(historia);
    }

    public async Task<HistoriaRespostaDto?> EditarAsync(int id, CriarHistoriaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            throw new ArgumentException("O nome da história é obrigatório.");
        }

        if (dto.ProjetoId <= 0)
        {
            throw new ArgumentException("O projeto da história é obrigatório.");
        }

        var historia = await _historiaRepositorio.ObterPorIdAsync(id);

        if (historia == null)
        {
            return null;
        }

        historia.Nome = dto.Nome.Trim();
        historia.Descricao = dto.Descricao?.Trim();
        historia.ProjetoId = dto.ProjetoId;

        await _historiaRepositorio.AtualizarAsync(historia);

        var atualizada = await _historiaRepositorio.ObterPorIdAsync(id);

        return Mapear(atualizada ?? historia);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var historia = await _historiaRepositorio.ObterPorIdAsync(id);

        if(historia == null)
            return false;

        await _historiaRepositorio.ExcluirAsync(historia);

        return true;
    }

    private static HistoriaRespostaDto Mapear(Historia historia)
    {
        return new HistoriaRespostaDto
        {
            Id = historia.Id,
            Nome = historia.Nome,
            Descricao = historia.Descricao,
            ProjetoId = historia.ProjetoId,
            ProjetoNome = historia.Projeto?.Nome ?? string.Empty
        };
    }
}