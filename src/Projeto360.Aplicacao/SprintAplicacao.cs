using System.Text.RegularExpressions;
using Projeto360.Aplicacao.Contratos;
using Projeto360.Aplicacao.DTOs;
using Projeto360.Dominio.Entidades;

namespace Projeto360.Aplicacao;

public class SprintAplicacao : ISprintAplicacao
{
    // Letras (com acento), números e espaços.
    private static readonly Regex NomeValido = new(@"^[A-Za-zÀ-ÿ0-9 ]+$");

    private readonly ISprintRepositorio _sprintRepositorio;

    public SprintAplicacao(ISprintRepositorio sprintRepositorio)
    {
        _sprintRepositorio = sprintRepositorio;
    }

    public async Task<IEnumerable<SprintRespostaDto>> ListarAsync()
    {
        var sprints = await _sprintRepositorio.ListarAsync();
        return sprints.Select(Mapear);
    }

    public async Task<SprintRespostaDto?> ObterPorIdAsync(int id)
    {
        var sprint = await _sprintRepositorio.ObterPorIdAsync(id);
        return sprint == null ? null : Mapear(sprint);
    }

    public async Task<SprintRespostaDto> CriarAsync(CriarSprintDto dto)
    {
        Validar(dto);

        var sprint = new Sprint
        {
            Nome = dto.Nome.Trim(),
            ProjetoId = dto.ProjetoId,
            DataInicio = dto.DataInicio,
            DataFinal = dto.DataFinal,
            Situacao = dto.Situacao
        };

        await _sprintRepositorio.AdicionarAsync(sprint);

        var criada = await _sprintRepositorio.ObterPorIdAsync(sprint.Id);
        return Mapear(criada ?? sprint);
    }

    public async Task<SprintRespostaDto?> EditarAsync(int id, CriarSprintDto dto)
    {
        Validar(dto);

        var sprint = await _sprintRepositorio.ObterPorIdAsync(id);
        if (sprint == null)
        {
            return null;
        }

        sprint.Nome = dto.Nome.Trim();
        sprint.ProjetoId = dto.ProjetoId;
        sprint.DataInicio = dto.DataInicio;
        sprint.DataFinal = dto.DataFinal;
        sprint.Situacao = dto.Situacao;

        await _sprintRepositorio.AtualizarAsync(sprint);

        var atualizada = await _sprintRepositorio.ObterPorIdAsync(id);
        return Mapear(atualizada ?? sprint);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var sprint = await _sprintRepositorio.ObterPorIdAsync(id);
        if (sprint == null)
        {
            return false;
        }

        await _sprintRepositorio.ExcluirAsync(sprint);
        return true;
    }

    // Regras de negócio da sprint.
    private static void Validar(CriarSprintDto dto)
    {
        var nome = dto.Nome?.Trim() ?? string.Empty;

        if (nome.Length < 3 || nome.Length > 100)
        {
            throw new ArgumentException("O nome da sprint deve ter entre 3 e 100 caracteres.");
        }

        if (!NomeValido.IsMatch(nome))
        {
            throw new ArgumentException("O nome da sprint não pode conter caracteres especiais.");
        }

        if (dto.ProjetoId <= 0)
        {
            throw new ArgumentException("O projeto da sprint é obrigatório.");
        }

        if (dto.DataInicio == default)
        {
            throw new ArgumentException("A data de início é obrigatória.");
        }

        if (dto.DataFinal == default)
        {
            throw new ArgumentException("A data de fim é obrigatória.");
        }

        if (dto.DataFinal <= dto.DataInicio)
        {
            throw new ArgumentException("A data de fim deve ser posterior à data de início.");
        }

        if (!Enum.IsDefined(typeof(SituacaoSprint), dto.Situacao))
        {
            throw new ArgumentException("A situação da sprint é inválida.");
        }
    }

    private static SprintRespostaDto Mapear(Sprint sprint)
    {
        return new SprintRespostaDto
        {
            Id = sprint.Id,
            Nome = sprint.Nome,
            ProjetoId = sprint.ProjetoId,
            ProjetoNome = sprint.Projeto?.Nome ?? string.Empty,
            DataInicio = sprint.DataInicio,
            DataFinal = sprint.DataFinal,
            Situacao = sprint.Situacao
        };
    }
}