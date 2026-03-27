using Dojinho.Domain.BaseCliente;

namespace Dojinho.Data.Interfaces.BaseCliente
{
    public interface ICondutorRepository
    {
        void CadastrarSenha(Condutor condutor, string senha, string bancoCliente);
    }
}
