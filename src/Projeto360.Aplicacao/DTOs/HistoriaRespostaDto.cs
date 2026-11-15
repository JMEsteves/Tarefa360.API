namespace Projeto360.Aplicacao.DTOs;

public class HistoriaRespostaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int ProjetoId { get; set; }
    public string ProjetoNome { get; set; } = string.Empty;
}