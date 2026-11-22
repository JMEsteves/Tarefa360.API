using Projeto360.Dominio.Entidades;

namespace Projeto360.Aplicacao.Contratos;

public interface ISprintRepositorio
{
    Task<IEnumerable<Sprint>> ListarAsync();
    Task<Sprint?> ObterPorIdAsync(int id);
    Task AdicionarAsync(Sprint sprint);
    Task AtualizarAsync(Sprint sprint);
    Task ExcluirAsync(Sprint sprint);
}