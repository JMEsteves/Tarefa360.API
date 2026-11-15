using Projeto360.Dominio.Entidades;

namespace Projeto360.Aplicacao.Contratos;

public interface IHistoriaRepositorio
{
    Task<IEnumerable<Historia>> ListarAsync();
    Task AdicionarAsync(Historia historia);
    Task<Historia?> ObterPorIdAsync(int id);
    Task AtualizarAsync(Historia historia);
    Task ExcluirAsync(Historia historia);
}