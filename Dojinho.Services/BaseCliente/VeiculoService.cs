using Dojinho.Domain.BaseCliente;
using Dojinho.Services.Interfaces.BaseCliente;

namespace Dojinho.Services.BaseCliente
{
    public class VeiculoService : IVeiculoService
    {
        /// <summary>
        /// Atualiza a flag de liberação do veículo para o próximo abastecimento (inativa a flag de liberação).
        /// </summary>
        /// <param name="veiculo"></param>
        /// <param name="bancoCliente"></param>
        public void AtualizarFlagLiberaVeiculo(Veiculo veiculo, string bancoCliente)
        {
            
        }
    }
}
