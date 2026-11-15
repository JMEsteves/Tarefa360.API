using Microsoft.EntityFrameworkCore;
using Projeto360.Aplicacao.Contratos;
using Projeto360.Dominio.Entidades;
using Projeto360.Repositorio.Dados;

namespace Projeto360.Repositorio.Repositorios;

public class HistoriaRepositorio : IHistoriaRepositorio
{
    private readonly Projeto360Contexto _context;

    public HistoriaRepositorio(Projeto360Contexto contexto)
    {
        _context = contexto;
    }

    public async Task<IEnumerable<Historia>> ListarAsync(){
        return await _context.Historias
        .Include(h => h.Projeto)
        .AsNoTracking()
        .OrderBy(h => h.Id)
        .ToListAsync();
    }

    public async Task AdicionarAsync(Historia historia)
    {
        await _context.Historias.AddAsync(historia);
        await _context.SaveChangesAsync();
    }

    public async Task<Historia?> ObterPorIdAsync(int id)
    {
        return await _context.Historias
        .Include(h => h.Projeto)
        .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task AtualizarAsync(Historia historia)
    {
        _context.Historias.Update(historia);
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirAsync(Historia historia)
    {
        _context.Historias.Remove(historia);
        await _context.SaveChangesAsync();
    }
}
