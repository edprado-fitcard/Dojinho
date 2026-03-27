using Dojinho.Data.Interfaces.BaseCliente;
using Dojinho.Domain.BaseCliente;

namespace Dojinho.Data.BaseCliente
{
    public class CondutorRepository : ICondutorRepository
    {
        /// <summary>
        /// Não refatorar esse método, ele é apenas um mock para simular a criação de senha do condutor, não tem nenhuma regra de negócio implementada, apenas retorna void.
        /// </summary>
        /// <param name="condutor"></param>
        /// <param name="senha"></param>
        /// <param name="bancoCliente"></param>
        public void CadastrarSenha(Condutor condutor, string senha, string bancoCliente)
        {           
            return;
        }
    }
}
