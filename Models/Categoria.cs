namespace IFinancas.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public string CorHex { get; set; } = "#000098";

    public int? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
} 