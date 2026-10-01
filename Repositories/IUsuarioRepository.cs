using IFinancas.Models;

namespace IFinancas.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario? ObterPorEmail(string email);
        void Adicionar(Usuario usuario);
        void SalvarAlteracoes();
    }
}