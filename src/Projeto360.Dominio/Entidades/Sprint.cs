namespace Projeto360.Dominio.Entidades;


public class Sprint
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFinal { get; set; }
    public SituacaoSprint Situacao { get; set; } = SituacaoSprint.Planejada;

    // Chave estrangeira e navegação para Projeto.
    public int ProjetoId { get; set; }
    public Projeto? Projeto { get; set; }
}