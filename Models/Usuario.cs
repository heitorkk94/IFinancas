namespace IFinancas.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    // Propriedades de Navegação (Relacionamentos)
    public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
    public ICollection<Conta> Contas { get; set; } = new List<Conta>();
    public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
}
