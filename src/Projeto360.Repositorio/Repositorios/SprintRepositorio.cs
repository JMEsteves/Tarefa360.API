using Microsoft.EntityFrameworkCore;
using Projeto360.Aplicacao.Contratos;
using Projeto360.Dominio.Entidades;
using Projeto360.Repositorio.Dados;

namespace Projeto360.Repositorio.Repositorios;

public class SprintRepositorio : ISprintRepositorio
{
    private readonly Projeto360Contexto _context;

    public SprintRepositorio(Projeto360Contexto contexto)
    {
        _context = contexto;
    }

    public async Task<IEnumerable<Sprint>> ListarAsync()
    {
        return await _context.Sprints
            .Include(s => s.Projeto)
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    public async Task<Sprint?> ObterPorIdAsync(int id)
    {
        return await _context.Sprints
            .Include(s => s.Projeto)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AdicionarAsync(Sprint sprint)
    {
        await _context.Sprints.AddAsync(sprint);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Sprint sprint)
    {
        _context.Sprints.Update(sprint);
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirAsync(Sprint sprint)
    {
        _context.Sprints.Remove(sprint);
        await _context.SaveChangesAsync();
    }
}