namespace Projeto360.Dominio.Entidades;

public class Historia
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public Projeto? Projeto { get; set; }
    public int ProjetoId { get; set; }
}