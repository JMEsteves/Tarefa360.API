namespace Projeto360.Aplicacao.DTOs;

public class CriarHistoriaDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int ProjetoId { get; set; }
}