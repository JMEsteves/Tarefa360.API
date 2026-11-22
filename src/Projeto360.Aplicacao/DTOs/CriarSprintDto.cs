
using Projeto360.Dominio.Entidades;

namespace Projeto360.Aplicacao.DTOs;

public class CriarSprintDto
{
    public string Nome { get; set; } = string.Empty;
    public int ProjetoId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFinal { get; set; }
    public SituacaoSprint Situacao { get; set; }
}