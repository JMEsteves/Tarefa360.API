using Projeto360.Aplicacao.DTOs;

namespace Projeto360.Aplicacao.Contratos;

public interface ISprintAplicacao
{
    Task<IEnumerable<SprintRespostaDto>> ListarAsync();
    Task<SprintRespostaDto?> ObterPorIdAsync(int id);
    Task<SprintRespostaDto> CriarAsync(CriarSprintDto dto);
    Task<SprintRespostaDto?> EditarAsync(int id, CriarSprintDto dto);
    Task<bool> ExcluirAsync(int id);
}