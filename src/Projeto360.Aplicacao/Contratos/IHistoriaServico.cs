using Projeto360.Aplicacao.DTOs;

namespace Projeto360.Aplicacao.Contratos;

public interface IHistoriaServico
{
    Task<IEnumerable<HistoriaRespostaDto>> ListarAsync();

    Task<HistoriaRespostaDto> CriarAsync(CriarHistoriaDto dto);

    Task<HistoriaRespostaDto?> ObterPorIdAsync(int id);

    Task<HistoriaRespostaDto?> EditarAsync(int id, CriarHistoriaDto dto);

    Task<bool> ExcluirAsync(int id);

}