namespace IFinancas.Models;

public class Conta
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal SaldoInicial { get; set; }

    // Tipo da conta como string (ex: "Corrente", "Poupança", "Carteira")
    public string Tipo { get; set; } = string.Empty;

    // Relacionamento com Usuário
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
}